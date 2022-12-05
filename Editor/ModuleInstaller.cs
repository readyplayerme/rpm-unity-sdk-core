using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Threading;
using UnityEditor.PackageManager;



namespace ReadyPlayerMe.Core.Editor
{
    public static class ModuleInstaller 
    {
        private const string PROGRESS_BAR_TITLE = "Ready Player Me";

        public static Action ModuleInstallComplete;
        
        public static void Init()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(100);
            if (HasAnyMissingModule())
            {
                InstallModules();
            }
        }

        private static void InstallModules()
        {
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);

            var count = ModuleList.Modules.Length;
            for(var i = 0; i < count; i++)
            {
                var packages = GetPackageList();
                
                var module = ModuleList.Modules[i];
                
                if (packages.All(info => info.name != module.name))
                {
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", i * count * 0.1f + 0.1f);
                    AddModule(module.Identifier);
                }
                else
                {
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"All modules are loaded.", 1);
                }
            }
            EditorAssetLoader.CreateSettingsAssets();
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(100);
            EditorUtility.ClearProgressBar();
            ModuleInstallComplete?.Invoke();
        }

        private static bool HasAnyMissingModule()
        {
            var packages = GetPackageList();
            var modules = ModuleList.Modules;
            return modules.Select(module => packages.All(info => info.name != module.name)).FirstOrDefault();
        }

        private static PackageCollection GetPackageList()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(20);
 
            if (listRequest.Error != null)
            {
                Debug.Log("Error: " + listRequest.Error.message);
                return null;
            }
            
            return listRequest.Result;
        }

        private static void AddModule(string name)
        {
            var addRequest = Client.Add(name);
            while (!addRequest.IsCompleted)
                Thread.Sleep(20);
            if (addRequest.Error != null)
            {
                Debug.Log("Error: " + addRequest.Error.message);
            }
        }
    }
}
