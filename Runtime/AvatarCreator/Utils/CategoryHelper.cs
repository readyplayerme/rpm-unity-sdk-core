﻿using System.Collections.Generic;
using System.Linq;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public static class CategoryHelper
    {
        public static IEnumerable<AssetType> GetCategories(BodyType bodyType)
        {
            return PartnerCategoryMap
                .Select(a => a.Value)
                .Where(category => category.IsCompatibleCategory(bodyType))
                .ToList();
        }

        public static readonly Dictionary<string, AssetType> PartnerCategoryMap = new Dictionary<string, AssetType>
        {
            { "faceshape", AssetType.FaceShape },
            { "eyeshape", AssetType.EyeShape },
            { "eye", AssetType.EyeColor },
            { "eyebrows", AssetType.EyebrowStyle },
            { "noseshape", AssetType.NoseShape },
            { "lipshape", AssetType.LipShape },
            { "beard", AssetType.BeardStyle },
            { "hair", AssetType.HairStyle },
            { "outfit", AssetType.Outfit },
            { "shirt", AssetType.Shirt },
            { "glasses", AssetType.Glasses },
            { "facemask", AssetType.FaceMask },
            { "facewear", AssetType.Facewear },
            { "headwear", AssetType.Headwear },
            { "hairColor", AssetType.HairColor },
            { "eyebrowColor", AssetType.EyebrowColor },
            { "beardColor", AssetType.BeardColor },
            { "bottom", AssetType.Bottom },
            { "top", AssetType.Top },
            { "footwear", AssetType.Footwear }
        };

        public static bool IsOutfitAsset(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Outfit:
                case AssetType.Shirt:
                case AssetType.Bottom:
                case AssetType.Top:
                case AssetType.Footwear:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsFaceAsset(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.FaceShape:
                case AssetType.EyeShape:
                case AssetType.EyeColor:
                case AssetType.EyebrowStyle:
                case AssetType.NoseShape:
                case AssetType.LipShape:
                case AssetType.BeardStyle:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsCompatibleCategory(this AssetType assetType, BodyType bodyType)
        {
            // Filter asset type based on body type.
            if (bodyType == BodyType.FullBody)
            {
                return assetType != AssetType.Shirt;
            }
            return assetType != AssetType.Outfit;
        }

        public static bool IsOptionalAsset(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Top:
                case AssetType.Bottom:
                case AssetType.Footwear:
                case AssetType.Outfit:
                case AssetType.Shirt:
                case AssetType.EyebrowStyle:
                    return false;
                default:
                    return !assetType.IsColorAsset();
            }
        }

        public static bool IsColorAsset(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.EyeColor:
                case AssetType.BeardColor:
                case AssetType.EyebrowColor:
                case AssetType.HairColor:
                case AssetType.SkinColor:
                    return true;
                default:
                    return false;
            }
        }
    }
}
