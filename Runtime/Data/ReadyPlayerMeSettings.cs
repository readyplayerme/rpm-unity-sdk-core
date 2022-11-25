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
                    instance = Resources.Load<ReadyPlayerMeSettings>(SETTINGS_RESOURCE_PATH);
                }

                return instance;
            }
        }

        public static string GetPartnerSubdomain() => Instance.partnerSubdomain;
        public static AvatarLoaderSettings GetAvatarLoaderSettings() => Instance.avatarLoaderSettings;
    }
}
