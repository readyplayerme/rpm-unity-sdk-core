using System;

namespace ReadyPlayerMe.AvatarCreator
{
    
    [Serializable]
    public struct ColorLibrary
    {
        public AssetColor[] Skin;
        public AssetColor[] Eyebrow;
        public AssetColor[] Beard;
        public AssetColor[] Hair;
    }

    [Serializable]
    public struct AssetColor : IAssetData
    {
        public string Id { get; set; }
        public Category Category { get; set; }
        public string HexColor;
        
        public AssetColor(string id, Category category, string hexColor)
        {
            Id = id;
            Category = category;
            HexColor = hexColor;
        }
    }
}