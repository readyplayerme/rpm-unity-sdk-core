using Newtonsoft.Json;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public struct PartnerAsset
    {
        public string Id;
        [JsonProperty("type"), JsonConverter(typeof(CategoryConverter))]
        public Category Category;
        [JsonConverter(typeof(GenderConverter))]
        public OutfitGender Gender;
        [JsonProperty("iconUrl")]
        public string ImageUrl;
        [JsonProperty("lockedCategories")]
        public string[] LockedCategories;
    }
}
