using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public static class AssetTypeExtensions
    {
        private static readonly Dictionary<AssetType, string> ColorPropertyByAssetType = new Dictionary<AssetType, string>
        {
            { AssetType.SkinColor, "skin" },
            { AssetType.BeardColor, "beard" },
            { AssetType.EyebrowColor, "eyebrow" },
            { AssetType.HairColor, "hair" }
        };

        public static string GetColorProperty(this AssetType assetType)
        {
            return ColorPropertyByAssetType.TryGetValue(assetType, out var property) ? property : string.Empty;
        }
    }
}
