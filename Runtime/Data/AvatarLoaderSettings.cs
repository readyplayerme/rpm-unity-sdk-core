using System.IO;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    [CreateAssetMenu(fileName = "Avatar Loader Settings", menuName = "Scriptable Objects/Ready Player Me/Avatar Loader Settings", order = 1)]
    public class AvatarLoaderSettings : ScriptableObject
    {
        public const string RESOURCE_PATH = "Ready Player Me/Settings";
        public bool AvatarCachingEnabled;
        public AvatarConfig AvatarConfig;
        private static readonly string DEFAULT_ASSET_NAME = "AvatarLoaderSettings.asset";

#if !DISABLE_AUTO_INSTALLER
        private static readonly string DEFAULT_ASSET_PATH = "Packages/com.readyplayerme.core/Settings/AvatarLoaderSettings.asset";
#else
        private static readonly string DEFAULT_ASSET_PATH = "Assets/Ready Player Me/Core/Settings/AvatarLoaderSettings.asset";
#endif

        public static AvatarLoaderSettings GetCreateSettingsAsset()
        {
            var localPath = $"Assets/{RESOURCE_PATH}/{DEFAULT_ASSET_NAME}";
            var absolutePath = $"{Application.dataPath}/{RESOURCE_PATH}/{DEFAULT_ASSET_NAME}";
            if (File.Exists(absolutePath))
            {
                return AssetDatabase.LoadAssetAtPath<AvatarLoaderSettings>($"{localPath}");
            }
            if (!Directory.Exists($"{Application.dataPath}/{RESOURCE_PATH}"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/{RESOURCE_PATH}");
            }
            var defaultSettings = AssetDatabase.LoadAssetAtPath<AvatarLoaderSettings>(DEFAULT_ASSET_PATH);
            AvatarLoaderSettings newSettings = Instantiate(defaultSettings);
            AssetDatabase.CreateAsset(newSettings, $"{localPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newSettings;
        }
    }
}
