using System.IO;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public class ReadyPlayerMeSettings : ScriptableObject
    {
        public string partnerSubdomain = "demo";
        public AvatarLoaderSettings AvatarLoaderSettings;

        private static readonly string LOCAL_SAVE_FOLDER = "Ready Player Me/Settings";
        private static readonly string DEFAULT_ASSET_NAME = "ReadyPlayerMeSettings.asset";
#if !DISABLE_AUTO_INSTALLER
        private static readonly string DEFAULT_ASSET_PATH = "Packages/com.unity.images-library/Resources/ReadyPlayerMeSettings.asset";
#else
        private static readonly string DEFAULT_ASSET_PATH = "Assets/TEMP/ReadyPlayerMeSettings.asset";
#endif
        
        public static void GetCreateSettingsAsset()
        {
            var localPath = $"Assets/{LOCAL_SAVE_FOLDER}/{DEFAULT_ASSET_NAME}";
            var absolutePath = $"{Application.dataPath}/{LOCAL_SAVE_FOLDER}/{DEFAULT_ASSET_NAME}";
            if(File.Exists(absolutePath))return;
            if(!Directory.Exists( $"{Application.dataPath}/{LOCAL_SAVE_FOLDER}"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/{LOCAL_SAVE_FOLDER}");
            }
            var defaultSettings = AssetDatabase.LoadAssetAtPath<ReadyPlayerMeSettings>(DEFAULT_ASSET_PATH);
            var newSettings = Instantiate(defaultSettings);
            var loaderSettings = AvatarLoaderSettings.GetCreateSettingsAsset();
            newSettings.AvatarLoaderSettings = loaderSettings;
            AssetDatabase.CreateAsset(newSettings, $"{localPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}