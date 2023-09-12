using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;
using UnityEngine;
using UnityEditor;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class CoreSettingsManager
    {
        public const string PROJECT_RELATIVE_ASSET_PATH = "Assets/Ready Player Me/Resources/Settings/CoreSettings.asset";
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";

        private static CoreSettings coreSettings;

        static CoreSettingsManager()
        {
            EnsureSettingsExist();
        }

        public static void SetEnableAnalytics(bool isEnabled)
        {
            coreSettings.EnableAnalytics = isEnabled;
            Save();
        }

        public static void SetEnableLogging(bool isEnabled)
        {
            coreSettings.EnableLogging = isEnabled;
            Save();
        }

        public static void SaveSubDomain(string subDomain)
        {
            if (string.IsNullOrEmpty(subDomain) || coreSettings.Subdomain == subDomain) return;
            coreSettings.Subdomain = subDomain;
            Save();
        }

        public static void SaveAppId(string appId)
        {
            coreSettings.AppId = appId;
            Save();
        }

        public static void Save()
        {
            EditorUtility.SetDirty(coreSettings);
            AssetDatabase.SaveAssets();
        }

        public static void EnsureSettingsExist()
        {
            coreSettings = CoreSettings.Load();
            if (coreSettings == null)
            {
                coreSettings = CreateSettings();
            }
        }

        private static CoreSettings CreateSettings()
        {
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            var newSettings = ScriptableObject.CreateInstance<CoreSettings>();
            AssetDatabase.CreateAsset(newSettings, PROJECT_RELATIVE_ASSET_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newSettings;
        }
    }
}
