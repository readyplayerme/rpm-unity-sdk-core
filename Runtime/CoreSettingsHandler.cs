﻿using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class CoreSettingsHandler
    {
        private const string RESOURCE_PATH = "Settings/CoreSettings";
        public const string PROJECT_RELATIVE_PATH = "Assets/Ready Player Me/Resources/Settings/CoreSettings.asset";
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";
        public static CoreSettings CoreSettings
        {
            get
            {
                if (coreSettings == null)
                {
                    coreSettings = Resources.Load<CoreSettings>(RESOURCE_PATH);
#if UNITY_EDITOR
                    if (coreSettings == null)
                    {
                        coreSettings = CreateCoreSettings();
                    }
#endif
                }
                return coreSettings;
            }
        }

        private static CoreSettings coreSettings;

#if UNITY_EDITOR
        public static void SaveSubDomain(string subDomain)
        {
            coreSettings.Subdomain = subDomain;
            Save();
        }
        
        public static void Save()
        {
            EditorUtility.SetDirty(coreSettings);
            AssetDatabase.SaveAssets();

        }
        
        public static CoreSettings CreateCoreSettings()
        {
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            var newSettings = ScriptableObject.CreateInstance<CoreSettings>();
            newSettings.Subdomain = "demo";

            AssetDatabase.CreateAsset(coreSettings, PROJECT_RELATIVE_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newSettings;
        }
#endif
    }
}
