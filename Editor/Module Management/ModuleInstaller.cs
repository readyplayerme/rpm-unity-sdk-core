using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     Class <c>ModuleInstaller</c> is responsible for checking and installing all modules (Unity packages) required for
    ///     the Ready Player Me Unity SDK from their Git URL's.
    /// </summary>
    [InitializeOnLoad]
    public static class ModuleInstaller
    {
        private const string TAG = nameof(ModuleInstaller);

        private const int THREAD_SLEEP_TIME = 100;
        private const string PROGRESS_BAR_TITLE = "Ready Player Me";
        private const string GLTFAST_SYMBOL = "GLTFAST";
        private const string READY_PLAYER_ME_SYMBOL = "READY_PLAYER_ME";
        private const string GLTFAST_NAME = "com.atteneder.gltfast";
        private const string WEBVIEW_NAME = "webview";

        private const string MODULE_INSTALLATION_SUCCESS_MESSAGE =
            "All the modules are installed successfully. Ready Player Me avatar system is ready to use.";
        private const string MODULE_INSTALLATION_FAILURE_MESSAGE = "Something went wrong while installing modules.";
        private const string ALL_MODULES_ARE_INSTALLED = "All modules are installed.";
        private const string INSTALLING_MODULES = "Installing modules...";

        private static bool modulesInstalled;

        static ModuleInstaller()
        {
# if RPM_DEVELOPMENT
            modulesInstalled = true;
#endif
            if (!modulesInstalled)
            {
                InstallModules();
                EditorApplication.delayCall += DelayCreateCoreSettings;
            }

#if !GLTFAST
            if (IsModuleInstalled(GLTFAST_NAME))
            {
                AddGltfastSymbol();
                AddScriptingDefineSymbolToAllBuildTargetGroups(READY_PLAYER_ME_SYMBOL);
            }
#endif

        }

        private static void DelayCreateCoreSettings()
        {
            EditorApplication.delayCall -= DelayCreateCoreSettings;
            CoreSettingsLoader.EnsureSettingsExist();
        }

        /// <summary>
        ///     Installs the missing modules and displays a progress bar to notify the user.
        /// </summary>
        private static void InstallModules()
        {
            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, INSTALLING_MODULES, 0);
            Thread.Sleep(THREAD_SLEEP_TIME);

            var missingModules = GetMissingModuleNames();

            if (missingModules.Length > 0)
            {
                var installedModuleCount = 0f;

                foreach (ModuleInfo module in missingModules)
                {
                    var progress = installedModuleCount++ / missingModules.Length;
                    EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", progress);
                    AddModuleRequest(module.Identifier);
                }

                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, ALL_MODULES_ARE_INSTALLED, 1);
                Thread.Sleep(THREAD_SLEEP_TIME);
            }
            EditorUtility.ClearProgressBar();
            modulesInstalled = true;
        }

        /// <summary>
        ///     Request UPM to install the given module with the identifier.
        /// </summary>
        /// <param name="identifier">The Unity package identifier of the module to be installed.</param>
        public static void AddModuleRequest(string identifier)
        {
            PackageManagerHelper.AddPackage(identifier);
        }

        /// <summary>
        ///     Get modules from <c>ModuleList</c> that are not installed.
        /// </summary>
        /// <returns>An array of <c>ModuleInfo</c> for all the missing modules</returns>
        private static ModuleInfo[] GetMissingModuleNames()
        {
            var installed = PackageManagerHelper.GetPackageList();
            var missingModules = ModuleList.Modules.Where(m => installed.All(i => m.name != i.name)).ToList();
            
            if(!missingModules.Any(module => module.name.Contains(GLTFAST_NAME)))
            {
                missingModules = missingModules.Where(module => !module.name.Contains(WEBVIEW_NAME)).ToList();
            }
            return missingModules.ToArray();
        }

        /// <summary>
        ///     Check if the given module with the name is currently installed.
        /// </summary>
        /// <param name="name">Name of the module.</param>
        /// <returns>A boolean <c>true</c> if the module is installed.</returns>
        public static bool IsModuleInstalled(string name)
            => PackageManagerHelper.IsPackageInstalled(name);

        public static void AddGltfastSymbol()
        {
            AddScriptingDefineSymbolToAllBuildTargetGroups(GLTFAST_SYMBOL);
        }

        private static void AddScriptingDefineSymbolToAllBuildTargetGroups(string defineSymbol)
        {
            foreach (BuildTarget target in Enum.GetValues(typeof(BuildTarget)))
            {
                var group = BuildPipeline.GetBuildTargetGroup(target);

                if (group == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                List<string> defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').Select(d => d.Trim()).ToList();

                if (defineSymbols.Contains(defineSymbol)) continue;
                defineSymbols.Add(defineSymbol);
                try
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols.ToArray()));
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Could not set RPM " + defineSymbol + " defines for build target: " + target + " group: " + group + " " + e);
                }
            }

            CompilationPipeline.RequestScriptCompilation();
        }

        /// <summary>
        ///     Check all modules installed successfully
        /// </summary>
        private static void ValidateModules()
        {
            var packageList = PackageManagerHelper.GetPackageList();
            var allModuleInstalled = true;
            foreach (ModuleInfo module in ModuleList.Modules)
            {
                if (packageList.All(x => x.name != module.name))
                {
                    allModuleInstalled = false;
                }
            }

            if (allModuleInstalled)
            {
                SDKLogger.Log(TAG, MODULE_INSTALLATION_SUCCESS_MESSAGE);
                AssetDatabase.Refresh();
                CompilationPipeline.RequestScriptCompilation();
            }
            else
            {
                SDKLogger.LogWarning(TAG, MODULE_INSTALLATION_FAILURE_MESSAGE);
            }
        }
    }
}
