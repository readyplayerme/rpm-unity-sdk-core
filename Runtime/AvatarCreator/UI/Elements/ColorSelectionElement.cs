using System.Threading.Tasks;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class ColorSelectionElement : SelectionElement
    {
        [Header("Properties")]
        [SerializeField] private AssetType assetType;
        private AssetColor[] colorAssets;
        private readonly AvatarAPIRequests avatarAPIRequests = new AvatarAPIRequests();

        public async Task LoadColorPalette(AvatarProperties avatarProperties)
        {
            colorAssets = await avatarAPIRequests.GetColorsByCategory(avatarProperties.Id, assetType);
        }

        public async void LoadAndCreateButtons(AvatarProperties avatarProperties)
        {
            await LoadColorPalette(avatarProperties);
            ClearButtons();
            CreateButtons(colorAssets, (button, asset) =>
            {
                button.SetColor(asset.HexColor);
            });
        }
    }
}
