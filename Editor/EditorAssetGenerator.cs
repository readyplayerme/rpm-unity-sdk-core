using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public static class EditorAssetGenerator
    {
        private const string SETTINGS_SAVE_FOLDER = "Ready Player Me/Resources/Settings";
        private const string PARTNER_SUB_DOMAIN_ASSET_NAME = "PartnerSubDomainSettings.asset";

        public static void CreateSettingsAssets()
        {
            DirectoryUtility.ValidateDirectory($"{Application.dataPath}/{SETTINGS_SAVE_FOLDER}");
            AssetDatabase.Refresh();
            CreatePartnerSubDomainSetting();
        }

        private static void CreatePartnerSubDomainSetting()
        {
            var partnerSubDomainSettings = ScriptableObject.CreateInstance<PartnerSubdomainSettings>();
            partnerSubDomainSettings.Subdomain = "demo";

            AssetDatabase.CreateAsset(partnerSubDomainSettings, $"Assets/{SETTINGS_SAVE_FOLDER}/{PARTNER_SUB_DOMAIN_ASSET_NAME}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
