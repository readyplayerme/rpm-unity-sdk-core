using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace ReadyPlayerMe.AvatarCreator
{
    public class ColorSelectionElement : SelectionElement
    {
        private const string TAG = nameof(AssetSelectionElement);

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
            CreateButtons();
        }

        public void CreateButtons()
        {
            if (colorAssets.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            for (int i = 0; i < colorAssets.Length; i++)
            {
                var colorAsset = colorAssets[i];
                var button = CreateButton(colorAsset.Id);
                button.SetColor(colorAsset.HexColor);
                button.AddListener(() => AssetSelected(colorAsset));
                Debug.Log($"Create button {colorAsset.Id}");
            }
        }
    }
}
