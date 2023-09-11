using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class CoreSettingsHandler
    {
        private const string RESOURCE_PATH = "Settings/CoreSettings";
        public const string PROJECT_RELATIVE_ASSET_PATH = "Assets/Ready Player Me/Resources/Settings/CoreSettings.asset";
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";

        public static CoreSettings CoreSettings
        {
            get
            {
                if (coreSettings != null) return coreSettings;
                coreSettings = Resources.Load<CoreSettings>(RESOURCE_PATH);
#if UNITY_EDITOR
                if (coreSettings == null)
                {
                    coreSettings = CreateSettings();
                }
#endif
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
            coreSettings = Resources.Load<CoreSettings>(RESOURCE_PATH);
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
#endif
    }
}
