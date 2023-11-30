using System.Threading.Tasks;

using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class ColorSelectionElement : SelectionElement
    {
        private const string TAG = nameof(AssetSelectionElement);
        
        [SerializeField] private Category category;
        private AssetColor[] colorAssets;
        private readonly AvatarAPIRequests avatarAPIRequests = new AvatarAPIRequests();

        public async Task LoadColorPalette(AvatarProperties avatarProperties)
        {
            colorAssets = await avatarAPIRequests.GetColorsByCategory(avatarProperties.Id, category);
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
