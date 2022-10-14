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
        private static string[] modulesToLoad;
        private static int moduleInstallCount = 0;

        static AutoModuleLoad()
        {
            AssetDatabase.Refresh();
            EditorApplication.update += Update;
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
            if (moduleInstallCount > ModuleList.Modules.Length)
            {
                ModuleManager.OnAddComplete -= OnAddModuleComplete;
            }
            else
            {
                ModuleManager.Add(GetIdentifier(ModuleList.Modules[moduleInstallCount]));
            }
        }

        private static string GetIdentifier(ModuleInfo info)
        {
            if (info.gitUrl == string.Empty)
            {
                return info.name;
            }

            var branch = info.branch != string.Empty ? $"#{info.branch}" : "";
            return info.gitUrl + branch;
        }
    }
}
