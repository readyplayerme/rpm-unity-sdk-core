using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Threading;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class ModuleInstaller
    {
        private const int THREAD_SLEEP_TIME = 20;
        private const string PROGRESS_BAR_TITLE = "Ready Player Me";
        private const string CORE_MODULE_NAME = "com.readyplayerme.core";

        #region Register Package Events
        static ModuleInstaller()
        {
            Events.registeredPackages += OnRegisteredPackages;
            Events.registeringPackages += OnRegisteringPackages;
        }

        private static void OnRegisteredPackages(PackageRegistrationEventArgs args)
        {
            // Core Module installed
            if (args.added != null && args.added.Any(p => p.name == CORE_MODULE_NAME))
            {
                Debug.Log("Core installed");
                if (HasAnyMissingModule())
                {
                    InstallModules();
                }
            }
            
            Events.registeredPackages -= OnRegisteredPackages;
        }
        
        private static void OnRegisteringPackages(PackageRegistrationEventArgs args)
        {
            // Core module uninstalled
            if (args.removed != null && args.removed.Any(p => p.name == "com.readyplayerme.core"))
            {
                Debug.Log("Core uninstalled");
                // Remove modules that depend on core here, or not?
            }
            
            Events.registeringPackages -= OnRegisteringPackages;
        }
        #endregion
        
        private static void InstallModules()
        {
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);

            var count = ModuleList.Modules.Length;
            for(var i = 0; i < count; i++)
            {
                var packages = GetPackageList();
                
                var module = ModuleList.Modules[i];
                
                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", i * count * 0.1f + 0.1f);
                AddModuleRequest(module.Identifier);
            }
            
            EditorAssetLoader.CreateSettingsAssets();
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted) Thread.Sleep(THREAD_SLEEP_TIME);
            EditorUtility.ClearProgressBar();
        }

        public static void AddModuleRequest(string name)
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
        
        private static bool HasAnyMissingModule()
        {
            var packageNames = GetPackageList().Select(p => p.name);
            var moduleNames = ModuleList.Modules.Select(m => m.name);
            var hasMissingModule = moduleNames.Except(packageNames).Any();
            
            Debug.Log($"Package Names: {packageNames}");
            Debug.Log($"Package Names: {moduleNames}");
            Debug.Log($"Any missing?: {hasMissingModule}");
            
            // returns true if any package name is missing in packages list
            return hasMissingModule;
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
    }
}
