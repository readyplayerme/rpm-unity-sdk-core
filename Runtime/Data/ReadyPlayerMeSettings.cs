using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public class ReadyPlayerMeSettings : ScriptableObject
    {
        private const string SETTINGS_RESOURCE_PATH = "Settings/ReadyPlayerMeSettings";
        
        public string partnerSubdomain = "demo";
        public AvatarLoaderSettings avatarLoaderSettings;
        
        private static ReadyPlayerMeSettings instance;
        public static ReadyPlayerMeSettings Instance
        {
            get
            {
                if (instance == null)
                {
#if DISABLE_AUTO_INSTALLER && UNITY_EDITOR
                    instance = AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>(
                        $"Assets/Ready Player Me/Core/{SETTINGS_RESOURCE_PATH}.asset");
#else
                    instance = Resources.Load<ReadyPlayerMeSettings>(SETTINGS_RESOURCE_PATH);
#endif
                }

                return instance;
            }
        }

        public static string GetPartnerSubdomain() => Instance.partnerSubdomain;
        public static AvatarLoaderSettings GetAvatarLoaderSettings() => Instance.avatarLoaderSettings;
    }
}
