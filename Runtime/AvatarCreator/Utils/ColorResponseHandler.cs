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

        public static Dictionary<Category, AssetColor[]> GetColorsFromResponse(string response)
        {
            var responseData = JObject.Parse(response);
            ColorResponse colorResponse = ((JObject) responseData["data"])!.ToObject<ColorResponse>();
            return ResponseToColorMap(colorResponse);
        }

        private static Dictionary<Category, AssetColor[]> ResponseToColorMap(ColorResponse colorResponse)
        {
            var colorPalettes = new Dictionary<Category, AssetColor[]>();
            colorPalettes.Add(Category.SkinColor, ConvertToAssetColors(colorResponse.skin, Category.SkinColor));
            colorPalettes.Add(Category.EyebrowColor, ConvertToAssetColors(colorResponse.eyebrow, Category.EyebrowColor));
            colorPalettes.Add(Category.BeardColor, ConvertToAssetColors(colorResponse.beard, Category.BeardColor));
            colorPalettes.Add(Category.HairColor, ConvertToAssetColors(colorResponse.hair, Category.HairColor));
            return colorPalettes;
        }

        
        private static AssetColor[] ConvertToAssetColors(IReadOnlyList<string> hexColors, Category category)
        {
            AssetColor[] assetColors = new AssetColor[hexColors.Count];
            for (int i = 0; i < hexColors.Count; i++)
            {
                assetColors[i] = new AssetColor(i.ToString(), category, hexColors[i]);
            }
            return assetColors;
        }
    }
}
