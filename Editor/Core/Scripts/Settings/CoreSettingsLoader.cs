using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class CoreSettingsLoader
    {
        private const string PROJECT_RELATIVE_ASSET_PATH = "Assets/Ready Player Me/Resources/Settings/CoreSettings.asset";
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";

        static CoreSettingsLoader()
        {
            EnsureSettingsExist();
        }

        public static void EnsureSettingsExist()
        {
            if (CoreSettingsHandler.CoreSettings == null && AssetDatabase.LoadAssetAtPath<CoreSettings>(PROJECT_RELATIVE_ASSET_PATH) == null)
            {
                CreateSettings();
            }
        }

        private static void CreateSettings()
        {
            Debug.Log($"Create Seed Settings at {PROJECT_RELATIVE_ASSET_PATH}");
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            var newSettings = ScriptableObject.CreateInstance<CoreSettings>();
            AssetDatabase.CreateAsset(newSettings, PROJECT_RELATIVE_ASSET_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
