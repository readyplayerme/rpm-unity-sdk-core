using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class CoreSettings
    {
        public const string SETTINGS_PATH = "Settings/PartnerSubDomainSettings";

        public static PartnerSubDomainSettings PartnerSubDomainSettings
        {
            get
            {
                if (partnerSubDomainSettings == null)
                {
                    partnerSubDomainSettings = Resources.Load<PartnerSubDomainSettings>(SETTINGS_PATH);
                }
                return partnerSubDomainSettings;
            }
        }

        private static PartnerSubDomainSettings partnerSubDomainSettings;

        public static void SaveSubDomain(string subDomain)
        {
            partnerSubDomainSettings.Subdomain = subDomain;
#if UNITY_EDITOR
            EditorUtility.SetDirty(partnerSubDomainSettings);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
