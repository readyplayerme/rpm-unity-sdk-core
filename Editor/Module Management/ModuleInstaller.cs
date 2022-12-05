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
                InstallModules();
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
            ModuleInfo[] missingModules = GetMissingModuleNames();

            if (missingModules.Length > 0)
            {
                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);
                var installedModuleCount = 0;
                
                foreach (var module in missingModules)
                {
                    var progress = installedModuleCount / missingModules.Length;
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", progress);
                    AddModuleRequest(module.Identifier);
                }
                
                EditorUtility.ClearProgressBar();

                EditorAssetLoader.CreateSettingsAssets();
            }
        }

        public static void AddModuleRequest(string name)
        {
            var addRequest = Client.Add(name);
            while (!addRequest.IsCompleted)
                Thread.Sleep(THREAD_SLEEP_TIME);
            
            if (addRequest.Error != null)
            {
                Debug.Log("Error: " + addRequest.Error.message);
            }
        }
        
        [MenuItem("Test/Load")]
        private static ModuleInfo[] GetMissingModuleNames()
        {
            var installed = GetPackageList();
            var missing = ModuleList.Modules.Where(m => installed.All(i => m.name != i.name));
            
            return missing.ToArray();
        }
        
        public static bool IsModuleInstalled(ModuleInfo module)
        {
            return GetPackageList().Any(info => info.name == module.name);
        }
        
        private static PackageCollection GetPackageList()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(THREAD_SLEEP_TIME);
 
            if (listRequest.Error != null)
            {
                Debug.Log("Error: " + listRequest.Error.message);
                return null;
            }

            return listRequest.Result;
        }
    }
}
