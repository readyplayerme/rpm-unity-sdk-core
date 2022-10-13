using System;
using UnityEditor;

namespace ReadyPlayerMe.Core
{
    [InitializeOnLoad]
    public static class AutoModuleLoad
    {
        /// <summary>
        /// Event for when package is imported or when project with package is opened.
        /// </summary>
        public static Action Startup;
        private const string AUTO_LOAD_MODULES = "AutoLoadModules";
        private static string[] ModulesToLoad;
        private static int moduleInstallCount = 0;
        static AutoModuleLoad()
        {
            EditorApplication.update += Update;
            ModulesToLoad = new[]
            {
                ModuleInfo.NEWTONSOFT_PACKAGE, 
                ModuleInfo.PackageMap[ModuleInfo.AVATAR_LOADER_PACKAGE], 
                ModuleInfo.PackageMap[ModuleInfo.WEB_VIEW_PACKAGE],
                ModuleInfo.PackageMap[ModuleInfo.GLTFAST_PACKAGE]
            };
            ModuleManager.OnAddComplete += OnAddModuleComplete;
        }

        private static void OnAddModuleComplete()
        {
            moduleInstallCount++;
            AddModule();
        }

        private static void Update()
        {
            if (SessionState.GetBool(AUTO_LOAD_MODULES, false)) return;
            SessionState.SetBool(AUTO_LOAD_MODULES, true);
            AddModule();
        }

        private static void AddModule()
        {
            switch (moduleInstallCount)
            {
                case 0:
                    ModuleManager.Add(ModuleInfo.NEWTONSOFT_PACKAGE);
                    break;
                case 1:
                    ModuleManager.Add(ModuleInfo.PackageMap[ModuleInfo.AVATAR_LOADER_PACKAGE]);
                    break;
                case 2: 
                    ModuleManager.Add(ModuleInfo.PackageMap[ModuleInfo.WEB_VIEW_PACKAGE]);
                    break;
                case 3:
                    ModuleManager.Add(ModuleInfo.PackageMap[ModuleInfo.GLTFAST_PACKAGE]);
                    break;
                case 4:
                    ModuleManager.OnAddComplete -= OnAddModuleComplete;
                    break;
                
            }
        }
    }
}
