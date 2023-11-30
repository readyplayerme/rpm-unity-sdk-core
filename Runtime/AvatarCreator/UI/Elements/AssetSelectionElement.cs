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
        [SerializeField] private Category category;
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

        public async Task LoadTemplateData()
        {
            partnerAssets = await partnerAssetsRequests.Get(category, bodyType, gender);
        }

        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            
            CreateButtons(partnerAssets,
                async (button, asset) =>
            {
                var downloadHandler = new DownloadHandlerTexture();
                var webRequestDispatcher = new WebRequestDispatcher();
                var url = $"{asset.ImageUrl}?w=64";
                var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler);
                response.ThrowIfError();
                
                UpdateButtonIcon(asset.Id, response.Texture);
            });
        }
    }
}
