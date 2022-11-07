using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    [CreateAssetMenu(fileName = "Avatar Configuration", menuName = "Scriptable Objects/Ready Player Me/Avatar Configuration", order = 2)]
    [HelpURL("https://docs.readyplayer.me/ready-player-me/integration-guides/unity-sdk/how-to-use#load-avatars-at-runtime")]
    public class AvatarConfig : ScriptableObject
    {
        public MeshLod MeshLod;
        public Pose Pose;
        public TextureAtlas TextureAtlas;
        [Range(256, 1024)]
        public int TextureSizeLimit = 1024;
        public bool UseHands;
        [HideInInspector]
        public List<string> MorphTargets = new List<string>();
        
        private static readonly string LOCAL_SAVE_FOLDER = "Ready Player Me/Settings";
        private static readonly string DEFAULT_ASSET_NAME = "AvatarConfig.asset";
#if !DISABLE_AUTO_INSTALLER
        private static readonly string DEFAULT_ASSET_PATH = "Packages/com.unity.images-library/Resources/AvatarConfig.asset";
#else
        private static readonly string DEFAULT_ASSET_PATH = "Assets/TEMP/AvatarConfig.asset";
#endif
        
        public static AvatarConfig CreateSettingsObject()
        {
            var localPath = $"Assets/{LOCAL_SAVE_FOLDER}/{DEFAULT_ASSET_NAME}";
            var absolutePath = $"{Application.dataPath}/{LOCAL_SAVE_FOLDER}/{DEFAULT_ASSET_NAME}";
            if (File.Exists(absolutePath))
            {
                return AssetDatabase.LoadAssetAtPath<AvatarConfig>($"{localPath}");
            }
            if(!Directory.Exists( $"{Application.dataPath}/{LOCAL_SAVE_FOLDER}"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/{LOCAL_SAVE_FOLDER}");
            }
            var defaultSettings = AssetDatabase.LoadAssetAtPath<AvatarConfig>(DEFAULT_ASSET_PATH);
            var newSettings = Instantiate(defaultSettings);

            AssetDatabase.CreateAsset(newSettings, $"{localPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newSettings;
        }
    }
}
