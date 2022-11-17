using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public class ReadyPlayerMeSettings : ScriptableObject
    {
        public string partnerSubdomain = "demo";
        public AvatarLoaderSettings AvatarLoaderSettings;
        private const string SETTINGS_PATH = "Settings/ReadyPlayerMeSettings";

        public void SaveSubdomain(string newSubdomain)
        {
            partnerSubdomain = newSubdomain;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static ReadyPlayerMeSettings LoadSettings()
        {
#if DISABLE_AUTO_INSTALLER
            return AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>($"Assets/Ready Player Me/Core/Settings/ReadyPlayerMeSettings.asset");
#else
            return Resources.Load<ReadyPlayerMeSettings>(SETTINGS_PATH);
#endif
        }
    }
}
