using System.Linq;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{

    /// <summary>
    /// Manages the selection of assets for a given body type, gender, and asset type.
    /// This class is responsible for loading asset data and creating corresponding buttons for each asset.
    /// It extends the functionality of SelectionElement to handle PartnerAsset-specific interactions.
    /// </summary>
    public class AssetSelectionElement : SelectionElement
    {
        [Header("Properties")]
        [SerializeField] private BodyType bodyType;
        [SerializeField] private OutfitGender gender;
        [SerializeField] private AssetType assetType;
        [SerializeField] private int iconSize = 64;

        private PartnerAsset[] partnerAssets;
        private PartnerAssetsRequests partnerAssetsRequests;

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

        /// <summary>
        /// Loads template data based on the current settings of asset type, body type, and gender.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation of loading asset data.</returns>
        public async Task LoadTemplateData()
        {
            partnerAssets = await partnerAssetsRequests.Get(assetType, bodyType, gender);
        }

        /// <summary>
        /// Asynchronously loads the template data and creates button elements for each asset. 
        /// Buttons are created with icons fetched based on the asset's image URL and icon size.
        /// </summary>
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
