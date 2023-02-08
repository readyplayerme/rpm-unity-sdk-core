using UnityEditor;
using UnityEngine;
using ReadyPlayerMe.Core.Data;

namespace ReadyPlayerMe.Core.Editor
{
    public static class EditorAssetGenerator
    {
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";
        private const string CORE_SETTINGS_ASSET_NAME = "CoreSettings.asset";

        public static void CreateSettingsAssets()
        {
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            AssetDatabase.Refresh();
            CreateCoreSettings();
        }

        private static void CreateCoreSettings()
        {
            var coreSettings = ScriptableObject.CreateInstance<CoreSettings>();
            coreSettings.Subdomain = "demo";

            AssetDatabase.CreateAsset(coreSettings, $"Assets/{SETTINGS_SAVE_FOLDER}/{CORE_SETTINGS_ASSET_NAME}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
