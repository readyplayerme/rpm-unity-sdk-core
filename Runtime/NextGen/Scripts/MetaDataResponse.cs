using System;
using UnityEngine;

namespace ReadyPlayerMe.NextGen
{
    [Serializable]
    public class MetaDataResponseWrapper
    {
        public MetaDataResponse data;
    }

    [Serializable]
    public class HexColorData
    {
        public string skin;
        public string eye;
        public HexColorTwoTone hair;
        public HexColorTwoTone beard;
        public HexColorTwoTone eyebrows;
    }

    [Serializable]
    public class HexColorTwoTone
    {
        public string primary;
        public string secondary;
    }

    [Serializable]
    public class MetaDataResponse
    {
        public string id;
        public string updatedAt;
        public HexColorData colors;

        public AvatarMetaData GetMetaData()
        {
            DateTime.TryParse(updatedAt, out var dateTime);
            var color = new Color();
            var skinToneHex = colors.skin;
            // Ensure the hex string starts with a '#' character
            if (skinToneHex != null && !skinToneHex.StartsWith("#"))
            {
                skinToneHex = "#" + skinToneHex;
            }

            if (!ColorUtility.TryParseHtmlString(skinToneHex, out color))
            {
                color = default;
            }
            var metaData = new AvatarMetaData
            {
                AvatarId = id,
                SkinColor = color,
                UpdatedAt = dateTime,
                AvatarColors = colors
            };
            return metaData;
        }
    }
}
