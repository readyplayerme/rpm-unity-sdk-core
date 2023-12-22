using System.Threading.Tasks;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class is responsible for fetching color data based on avatar properties,
    /// and creating buttons for each color.
    /// It extends the functionality of SelectionElement to handle color-specific interactions.
    /// </summary>
    public class ColorSelectionElement : SelectionElement
    {
        [Header("Properties")]
        [SerializeField, AssetTypeFilter(AssetFilter.Color)] private AssetType assetType;
        private AssetColor[] colorAssets;
        private readonly AvatarAPIRequests avatarAPIRequests = new AvatarAPIRequests();

        public async Task LoadColorPalette(AvatarProperties avatarProperties)
        {
            colorAssets = await avatarAPIRequests.GetAvatarColors(avatarProperties.Id, assetType);
        }

        public async void LoadAndCreateButtons(AvatarProperties avatarProperties)
        {
            await LoadColorPalette(avatarProperties);
            CreateButtons(colorAssets, (button, asset) =>
            {
                button.SetColor(asset.HexColor);
            });
        }
    }
}
