using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ReadyPlayerMe.Core.Editor
{
    public static class ModuleUpdater
    {
        private const string PACKAGE_JSON = "package.json";
        private const string PACKAGE_DOMAIN = "com.readyplayerme";

        private const string GITHUB_WEBSITE = "https://github.com";
        private const string GITHUB_API_URL = "https://api.github.com/repos";
        
        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorUtilities.InvokeOnLoad(nameof(ModuleUpdater), CheckForNewReleases);
        }

        [MenuItem("Ready Player Me/Check For Updates")]
        public static void CheckForNewReleases()
        {
            // Get PackageInfo array from RPM Module package.json files
            var packages = AssetDatabase.FindAssets("package")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.Contains(PACKAGE_JSON) && path.Contains(PACKAGE_DOMAIN))
                .Select(PackageInfo.FindForAssetPath)
                .ToArray();

            if (packages.Length == 0)
            {
                Debug.Log("No Ready Player Me modules found.");
            }
            
            // Turn package_name@repo_url#branch_name into https://api.github.com/repos/readyplayerme/repo_name/releases 
            foreach (var package in packages)
            {
                var repoUrl = package.packageId.Split('@')[1];
                var apiUrl = repoUrl.Replace(GITHUB_WEBSITE, GITHUB_API_URL);
                var releasesUrl = apiUrl.Split(new[] { ".git#" }, StringSplitOptions.None)[0] + "/releases";
                var packageUrl = repoUrl.Split('#')[0];
                
                // Experimental or prerelease packages might look like 0.1.0-exp.1, remove after dash to parse with Version
                var version = package.version.Split('-')[0];
                FetchReleases(package.name, packageUrl, releasesUrl, new Version(version));
            }
        }

        // Fetch release details from github and extract version to compare with local module version
        private static async void FetchReleases(string name, string packageUrl, string releasesUrl, Version current)
        {
            var request = UnityWebRequest.Get(releasesUrl);
            var asyncOperation = request.SendWebRequest();
            
            while (!asyncOperation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = request.downloadHandler.text;
                var releases = JsonConvert.DeserializeObject<Release[]>(response);
                var versions = releases?.Select(r => new Version(r.Tag.Substring(1).Split('-')[0])).ToArray();
                var latest = versions?.Max();

                if (latest > current)
                {
                    DisplayUpdateDialog(name, current, latest, packageUrl);
                }
            }
            else
            {
                Debug.Log($"Failed to fetch { name } releases. Error: {request.error} ");
            }
        }

        // Display a dialog for notifying developer about new module version.
        private static void DisplayUpdateDialog(string name, Version current, Version latest, string url)
        {
            var message = $"New update available for {name}\nCurrent version: {current}\nLatest version: {latest}";
            var shouldUpdate = EditorUtility.DisplayDialog("Update Packages", message, "Update", "Skip");

            if (shouldUpdate)
            {
                url += "#v" + latest;
                UpdateModule(name, url, current, latest);
            }
            else
            {
                // TODO: Bring analytics here
                Debug.LogWarning("Update skipped.");
            }
        }

        // Remove old module and add the new version.
        private static void UpdateModule(string name, string url, Version current, Version latest)
        {
            RemoveRequest removeRequest = Client.Remove(name);
            while (!removeRequest.IsCompleted) Thread.Sleep(20);
            
            AddRequest addRequest = Client.Add(url);
            while (!addRequest.IsCompleted) Thread.Sleep(20);

            Debug.Log($"Updated {name} from v{current} to v{latest}");
        }
    }
    
    // Class used for casting release json data, only tag field is required.
    internal class Release
    {
        [JsonProperty("tag_name")]
        public string Tag;
    }
}
