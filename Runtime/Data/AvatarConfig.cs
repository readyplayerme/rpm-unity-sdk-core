using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    [CreateAssetMenu(fileName = "Avatar Configuration", menuName = "Scriptable Objects/Ready Player Me/Avatar Configuration", order = 2),
     HelpURL("https://docs.readyplayer.me/ready-player-me/integration-guides/unity-sdk/how-to-use#load-avatars-at-runtime")]
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
        
#if !DISABLE_AUTO_INSTALLER
        public const string DEFAULT_CONFIG_FOLDER = "Packages/com.readyplayerme.core/Configurations";
#else
        private const string DEFAULT_CONFIG_FOLDER = "Assets/Ready Player Me/Core/Configurations";
#endif
        
        public static AvatarConfig CreateFromDefault(string configName)
        {
            var defaultConfig = AssetDatabase.LoadAssetAtPath<AvatarConfig>($"{DEFAULT_CONFIG_FOLDER}/{configName}.asset");
            var newSettings = CreateInstance<AvatarConfig>();
            newSettings.Pose = defaultConfig.Pose;
            newSettings.MeshLod = defaultConfig.MeshLod;
            newSettings.MorphTargets = defaultConfig.MorphTargets;
            newSettings.UseHands = defaultConfig.UseHands;
            newSettings.TextureSizeLimit = defaultConfig.TextureSizeLimit;
            return newSettings;
        }
    }
}
