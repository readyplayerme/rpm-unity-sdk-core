using System;
using System.Linq;
using UnityEditor;
using System.Threading;
using UnityEditor.PackageManager;
using System.Collections.Generic;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class ModuleInstaller
    {
        private const string TAG = nameof(ModuleInstaller);
        
        private const int THREAD_SLEEP_TIME = 100;
        private const string PROGRESS_BAR_TITLE = "Ready Player Me";
        private const string RPM_SCRIPTING_SYMBOL = "READY_PLAYER_ME";
        private const string CORE_MODULE_NAME = "com.readyplayerme.core";

        #region Register Package Events
        static ModuleInstaller()
        {
            Events.registeredPackages += OnRegisteredPackages;
            Events.registeringPackages += OnRegisteringPackages;
        }

        // Called when a package is added, removed or changed.
        private static void OnRegisteredPackages(PackageRegistrationEventArgs args)
        {
            // Core Module installed
            if (args.added != null && args.added.Any(p => p.name == CORE_MODULE_NAME))
            {
                InstallModules();
                AppendScriptingSymbol();
                EditorAssetLoader.CreateSettingsAssets();
            }
            
            Events.registeredPackages -= OnRegisteredPackages;
        }
        
        // Called when a package is about to be added, removed or changed.
        private static void OnRegisteringPackages(PackageRegistrationEventArgs args)
        {
            // Core module uninstalled
            if (args.removed != null && args.removed.Any(p => p.name == "com.readyplayerme.core"))
            {
                // Remove modules that depend on core here, or not?
            }
            
            Events.registeringPackages -= OnRegisteringPackages;
        }
        #endregion
        
        // Installs the missing modules and displays a progress bar to notify the user.
        private static void InstallModules()
        {
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);
            Thread.Sleep(THREAD_SLEEP_TIME);
            
            var missingModules = GetMissingModuleNames();

            if (missingModules.Length > 0)
            {
                var installedModuleCount = 0f;
                
                foreach (var module in missingModules)
                {
                    var progress = installedModuleCount++ / missingModules.Length;
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", progress);
                    AddModuleRequest(module.Identifier);
                }

                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "All modules are installed.", 1);
                Thread.Sleep(THREAD_SLEEP_TIME);
            }
            
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        ///     Request UPM to install the given module with the identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the module to be installed.</param>
        public static void AddModuleRequest(string identifier)
        {
            var addRequest = Client.Add(identifier);
            while (!addRequest.IsCompleted)
                Thread.Sleep(THREAD_SLEEP_TIME);
            
            if (addRequest.Error != null)
            {
                SDKLogger.Log(TAG, "Error: " + addRequest.Error.message);
            }
        }
        
        // Get the modules which are in ModuleList but currently not installed.
        private static ModuleInfo[] GetMissingModuleNames()
        {
            var installed = GetPackageList();
            var missing = ModuleList.Modules.Where(m => installed.All(i => m.name != i.name));
            
            return missing.ToArray();
        }

        /// <summary>
        ///     Checks if the given module with the name is currently installed.
        /// </summary>
        /// <param name="name">Name of the module.</param>
        /// <returns>Returns <c>true</c> if the module is installed.</returns>
        public static bool IsModuleInstalled(string name)
        {
            return GetPackageList().Any(info => info.name == name);
        }
        
        // Get the list of unity packages installed in the current project.
        private static PackageInfo[] GetPackageList()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(THREAD_SLEEP_TIME);

            if (listRequest.Error != null)
            {
                SDKLogger.Log(TAG, "Error: " + listRequest.Error.message);
                return Array.Empty<PackageInfo>();
            }

            return listRequest.Result.ToArray();
        }
        
        // Append RPM scripting symbol to player settings.
        private static void AppendScriptingSymbol()
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            var symbols = new HashSet<string>(defineSymbols.Split(';')) { RPM_SCRIPTING_SYMBOL };
            var newDefineString = string.Join(";", symbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, newDefineString);
        }
    }
}
