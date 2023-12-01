using System.Linq;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AssetSelectionElement : SelectionElement
    {
        private const string TAG = nameof(AssetSelectionElement);

        [Header("Events")]
        [Space(5)]
        [SerializeField] private BodyType bodyType;
        [SerializeField] private OutfitGender gender;
        [SerializeField] private AssetType assetType;
        private PartnerAsset[] partnerAssets;
        private PartnerAssetsRequests partnerAssetsRequests;
        [SerializeField] private int iconSize = 64;

        private void Awake()
        {
            partnerAssetsRequests = new PartnerAssetsRequests(CoreSettingsHandler.CoreSettings.AppId);
        }

        public void SetBodyType(BodyType bodyType)
        {
            this.bodyType = bodyType;
        }

        public void SetGender(OutfitGender gender)
        {
            this.gender = gender;
        }

        public async Task LoadTemplateData()
        {
            partnerAssets = await partnerAssetsRequests.Get(assetType, bodyType, gender);
        }

        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            CreateButtons(partnerAssets.ToArray(), async (button, asset) =>
            {
                var webRequestDispatcher = new WebRequestDispatcher();
                var url = iconSize > 0 ? $"{asset.ImageUrl}?w={iconSize}" : asset.ImageUrl;
                var texture = await webRequestDispatcher.DownloadTexture(url);
                button.SetIcon(texture);
            });
        }
    }
}
