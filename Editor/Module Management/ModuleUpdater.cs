using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ReadyPlayerMe.Core.Editor
{
    public static class ModuleUpdater
    {
        private const string PACKAGE_DOMAIN = "com.readyplayerme";
        private const string PACKAGE_JSON = "package.json";

        private const string GITHUB_WEBSITE = "https://github.com";
        private const string GITHUB_API_URL = "https://api.github.com/repos";

        public static void GetCurrentRelease()
        {
            List<PackageInfo> packages = AssetDatabase.FindAssets("package")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x => x.Contains(PACKAGE_JSON) && x.Contains(PACKAGE_DOMAIN))
                .Select(PackageInfo.FindForAssetPath)
                .ToList();

            if (packages.Count == 0)
            {
                Debug.LogWarning($"No Ready Player Me packages found.");
            }

            foreach (var package in packages)
            {
                var repoUrl = package.packageId.Split('@')[1];
                var releasesUrl = repoUrl.Replace(GITHUB_WEBSITE, GITHUB_API_URL)
                                              .Split(new[] { ".git#" }, StringSplitOptions.None)[0] + "/releases";
                var packageUrl = repoUrl.Split('#')[0];
                var version = package.version.Split('-')[0];
                FetchReleases(package.name, packageUrl, releasesUrl, new Version(version));
            }
        }

        private static async void FetchReleases(string packageName, string packageUrl, string releasesUrl,
            Version currentVersion)
        {
            UnityWebRequest request = UnityWebRequest.Get(releasesUrl);
            UnityWebRequestAsyncOperation op = request.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = request.downloadHandler.text;
                Release[] releases = JsonConvert.DeserializeObject<Release[]>(response);

                Version[] versions = releases.Select(r => new Version(r.Tag.Substring(1).Split('-')[0])).ToArray();

                Version latestVersion = versions.Max();

                if (latestVersion > currentVersion)
                {
                    PromptForUpdate(packageName, currentVersion, latestVersion, packageUrl);
                }
            }
            else
            {
                Debug.Log($"Failed to fetch { packageName } releases. Error: {request.error} ");
            }
        }

        private static void PromptForUpdate(string packageName, Version currentVersion, Version latestVersion,
            string packageUrl)
        {
            bool shouldUpdate = EditorUtility.DisplayDialog("Update Packages",
                $"New update available for {packageName}\nCurrent version: {currentVersion}\nLatest version: {latestVersion}",
                "Update",
                "Skip");

            if (shouldUpdate)
            {
                packageUrl += "#v" + latestVersion;
                Update(packageName, packageUrl, currentVersion, latestVersion);
            }
            else
            {
                // TODO: Bring analytics here
                Debug.LogWarning("Update skipped.");
            }
        }

        private static async void Update(string name, string url, Version current, Version latest)
        {
            RemoveRequest removeRequest = Client.Remove(name);
            while (!removeRequest.IsCompleted) Thread.Sleep(20);
            
            AddRequest addRequest = Client.Add(url);
            while (!addRequest.IsCompleted) Thread.Sleep(20);

            Debug.Log($"Updated {name} from v{current} to v{latest}");
        }
    }
    
    internal class Release
    {
        [JsonProperty("tag_name")] public string Tag;
    }
}
