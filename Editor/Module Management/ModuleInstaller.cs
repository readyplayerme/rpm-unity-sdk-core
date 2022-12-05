using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Threading;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    public class ModuleInstaller 
    {
        private const string PROGRESS_BAR_TITLE = "Ready Player Me";

        public static Action ModuleInstallComplete;

#if !DISABLE_AUTO_INSTALLER
        static ModuleInstaller()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(20);
            if (HasAnyMissingModule())
            {
                EditorApplication.update += InstallModules;
            }
        }
#endif

        private static void InstallModules()
        {
            EditorApplication.update -= InstallModules; //ensure it only runs once
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);

            var count = ModuleList.Modules.Length;
            for(var i = 0; i < count; i++)
            {
                var packages = GetPackageList();
                
                var module = ModuleList.Modules[i];
                
                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", i * count * 0.1f + 0.1f);
                AddModule(module.Identifier);
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
            var packageNames = GetPackageList().Select(p => p.name);
            var moduleNames = ModuleList.Modules.Select(m => m.name);
            
            // returns true if any package name is missing in packages list
            return moduleNames.Except(packageNames).Any();
        }
        
        public static bool IsModuleInstalled(ModuleInfo module)
        {
            return GetPackageList().Any(info => info.name == module.name);
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

        public static void AddModule(string name)
        {
            var packages = GetPackageList();

            if (packages.All(info => info.name != name))
            {
                var addRequest = Client.Add(name);
                while (!addRequest.IsCompleted)
                    Thread.Sleep(20);
                
                if (addRequest.Error != null)
                {
                    Debug.Log("Error: " + addRequest.Error.message);
                }
            }
            else
            {
                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Module {name} installed.", 1);
            }
        }
    }
}
