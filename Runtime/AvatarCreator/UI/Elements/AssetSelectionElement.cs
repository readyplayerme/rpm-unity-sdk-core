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
        private readonly IconFetcher iconFetcher = new IconFetcher();

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
            CreateButtons();
            await LoadIcons();
        }

        public void CreateButtons()
        {
            if (partnerAssets == null || partnerAssets.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            foreach (var partnerAsset in partnerAssets)
            {
                var button = CreateButton(partnerAsset.Id);
                var partnerAssetData = partnerAsset;
                button.AddListener(() => AssetSelected(partnerAssetData));
            }
        }

        public async Task LoadIcons()
        {
            if (partnerAssets == null || partnerAssets.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }

            foreach (var partnerAsset in partnerAssets)
            {
                var texture = await iconFetcher.GetIcon(partnerAsset.ImageUrl);
                OnIconLoaded(partnerAsset, texture);
            }
        }

        private void OnIconLoaded(PartnerAsset partnerAsset, Texture texture)
        {
            UpdateButtonIcon(partnerAsset.Id, texture);
        }
    }
}
