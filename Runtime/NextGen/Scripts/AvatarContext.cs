using System;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.NextGen
{
    public class AvatarContext : Context
    {
        public bool AvatarCachingEnabled;
        public AvatarConfig AvatarConfig;
        public AvatarUri AvatarUri;
        public byte[] Bytes;
        public object Data;
        public AvatarMetadata Metadata;
        public string ParametersHash;
        public AvatarRenderSettings RenderSettings;
        public string Url;
        public bool IsUpdateRequired;
    }

    [Serializable]
    public struct AvatarMetaData
    {
        public string AvatarId;
        public Color SkinColor;
        public DateTime UpdatedAt;
        public HexColorData AvatarColors;
    }

    public class AvatarData : MonoBehaviour
    {
        public string AvatarId => AvatarMetaData.AvatarId;
        public AvatarMetaData AvatarMetaData { get; private set; }

        public void SetMetaData(AvatarMetaData metaData)
        {
            AvatarMetaData = metaData;
        }
    }
}
