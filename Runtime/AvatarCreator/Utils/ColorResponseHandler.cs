using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ReadyPlayerMe.AvatarCreator
{
    public static class ColorResponseHandler
    {
        [Serializable]
        public struct ColorResponse
        {
            public string[] skin;
            public string[] eyebrow;
            public string[] beard;
            public string[] hair;
        }

        public static Dictionary<AssetType, AssetColor[]> GetColorsFromResponse(string response)
        {
            var responseData = JObject.Parse(response);
            ColorResponse colorResponse = ((JObject) responseData["data"])!.ToObject<ColorResponse>();
            return ResponseToColorMap(colorResponse);
        }

        private static Dictionary<AssetType, AssetColor[]> ResponseToColorMap(ColorResponse colorResponse)
        {
            var colorPalettes = new Dictionary<AssetType, AssetColor[]>();
            colorPalettes.Add(AssetType.SkinColor, ConvertToAssetColors(colorResponse.skin, AssetType.SkinColor));
            colorPalettes.Add(AssetType.EyebrowColor, ConvertToAssetColors(colorResponse.eyebrow, AssetType.EyebrowColor));
            colorPalettes.Add(AssetType.BeardColor, ConvertToAssetColors(colorResponse.beard, AssetType.BeardColor));
            colorPalettes.Add(AssetType.HairColor, ConvertToAssetColors(colorResponse.hair, AssetType.HairColor));
            return colorPalettes;
        }

        private static AssetColor[] ConvertToAssetColors(IReadOnlyList<string> hexColors, AssetType assetType)
        {
            AssetColor[] assetColors = new AssetColor[hexColors.Count];
            for (int i = 0; i < hexColors.Count; i++)
            {
                assetColors[i] = new AssetColor(i.ToString(), assetType, hexColors[i]);
            }
            return assetColors;
        }
    }
}
