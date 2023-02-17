using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class CoreSettingsHandler
    {
        private const string SETTINGS_PATH = "Settings/CoreSettings";

        public static CoreSettings CoreSettings
        {
            get
            {
                if (coreSettings == null)
                {
                    coreSettings = Resources.Load<CoreSettings>(SETTINGS_PATH);
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

        public static void SaveSubDomain(string subDomain)
        {
            coreSettings.Subdomain = subDomain;
            Save();
        }

        public static void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(coreSettings);
            AssetDatabase.SaveAssets();
#endif
        }
        
#if UNITY_EDITOR
        private static CoreSettings CreateCoreSettings()
        {
            var coreSettings = ScriptableObject.CreateInstance<CoreSettings>();
            coreSettings.Subdomain = "demo";

            AssetDatabase.CreateAsset(coreSettings, $"Assets/Ready Player Me/Resources/Settings/CoreSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return coreSettings;
        }
#endif
    }
}
