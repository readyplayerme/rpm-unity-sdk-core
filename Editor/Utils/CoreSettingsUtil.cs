using ReadyPlayerMe.Core.Data;
using UnityEngine;
using UnityEditor;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class CoreSettingsUtil
    {
        private const string PROJECT_RELATIVE_ASSET_PATH = "Assets/Ready Player Me/Resources/Settings/CoreSettings.asset";
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";

        static CoreSettingsUtil()
        {
            EnsureSettingsExist();
        }

        public static void SetEnableAnalytics(bool isEnabled)
        {
            CoreSettingsHandler.CoreSettings.EnableAnalytics = isEnabled;
            Save();
        }

        public static void SetEnableLogging(bool isEnabled)
        {
            CoreSettingsHandler.CoreSettings.EnableLogging = isEnabled;
            Save();
        }

        public static void SaveSubDomain(string subDomain)
        {
            if (string.IsNullOrEmpty(subDomain) || CoreSettingsHandler.CoreSettings.Subdomain == subDomain) return;
            CoreSettingsHandler.CoreSettings.Subdomain = subDomain;
            Save();
        }

        public static void SaveAppId(string appId)
        {
            CoreSettingsHandler.CoreSettings.AppId = appId;
            Save();
        }

        public static void Save()
        {
            EditorUtility.SetDirty(CoreSettingsHandler.CoreSettings);
            AssetDatabase.SaveAssets();
        }

        public static void EnsureSettingsExist()
        {
            if (CoreSettingsHandler.CoreSettings == null)
            {
                CreateSettings();
            }
        }

        private static void CreateSettings()
        {
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            var newSettings = ScriptableObject.CreateInstance<CoreSettings>();
            AssetDatabase.CreateAsset(newSettings, PROJECT_RELATIVE_ASSET_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
