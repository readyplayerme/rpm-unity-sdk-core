using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ReadyPlayerMe.Core
{
    /// <summary>
    /// This is a scriptable object used to configure and store the settings to apply to the avatar during the avatar loading
    /// process.
    /// </summary>
    [CreateAssetMenu(fileName = "Avatar Config", menuName = "Ready Player Me/Avatar Configuration", order = 2),
     HelpURL("https://docs.readyplayer.me/ready-player-me/integration-guides/unity-sdk/how-to-use#load-avatars-at-runtime")]
    public class AvatarConfig : ScriptableObject
    {
        [FormerlySerializedAs("MeshLod"), Tooltip("The mesh level of detail.")]
        public Lod Lod;
        [Tooltip("The resting pose of the avatars skeleton.")]
        public Pose Pose;
        [Tooltip("If set to NONE the mesh, materials and textures will not be combined into 1. (or 2 if an assets texture contains transparency)")]
        public TextureAtlas TextureAtlas;
        [Range(256, 1024), Tooltip("If set to none the mesh, materials and textures will not be combined into 1. (2 if an assets texture contains transparency)")]
        public int TextureSizeLimit = 1024;
        [Tooltip("Add textures which avatar will include")]
        public TextureChannel[] TextureChannel =
        {
            Core.TextureChannel.BaseColor,
            Core.TextureChannel.Normal,
            Core.TextureChannel.MetallicRoughness,
            Core.TextureChannel.Emissive,
            Core.TextureChannel.Occlusion
        };
        [Tooltip("Only works for halfbody avatars. If enabled, avatars will load with hand meshes included.")]
        public bool UseHands;
        [Tooltip("Enable if you want to use Draco compression. More effective on complex meshes.")]
        public bool UseDracoCompression;
        [Tooltip("Enable if you want to use Mesh Optimization compression. More effective on meshes with morph targets.")]
        public bool UseMeshOptCompression;
        [Tooltip("Add the shader which will be applied after avatar is loaded.")]
        public Shader Shader;
        [Tooltip("Assign properties for the shader used to assign the texture channels.")]
        public List<ShaderProperty> ShaderProperties = new List<ShaderProperty>
        {
            new ShaderProperty(Core.TextureChannel.BaseColor, "_MainTex"),
            new ShaderProperty(Core.TextureChannel.Normal, "_BumpMap"),
            new ShaderProperty(Core.TextureChannel.MetallicRoughness, string.Empty),
            new ShaderProperty(Core.TextureChannel.Emissive, string.Empty),
            new ShaderProperty(Core.TextureChannel.Occlusion, string.Empty),
        };

        public List<string> MorphTargets = new List<string>();
    }
}
