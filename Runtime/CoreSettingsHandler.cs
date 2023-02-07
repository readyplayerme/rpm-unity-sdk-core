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
    }
}
