using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    public struct IAssetData
    {
        public string Id;
        public Texture Texture;
        public string ImageUrl;
        public string Color;
        public Category Category;
    }

    [RequireComponent(typeof(SelectionElement))]
    public class AssetSelection : MonoBehaviour
    {
        private const string TAG = nameof(AssetSelection);
        private CancellationToken token = default;

        [Header("Events")]
        [Space(5)]
        public UnityEvent<PartnerAsset> onAssetSelected;
        [SerializeField] private BodyType bodyType;
        [SerializeField] private OutfitGender gender;
        [SerializeField] private Category category;
        private PartnerAsset[] partnerAssets;
        private PartnerAssetsRequests partnerAssetsRequests;
        private SelectionElement selectionElement;

        private void Awake()
        {
            partnerAssetsRequests = new PartnerAssetsRequests(CoreSettingsHandler.CoreSettings.AppId);
            selectionElement = GetComponent<SelectionElement>();
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
            partnerAssets = await partnerAssetsRequests.Get(category, bodyType, gender, token);
        }

        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            await CreateButtons();
        }

        public async Task CreateButtons()
        {
            if (partnerAssets.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            foreach (var partnerAsset in partnerAssets)
            {
                var downloadHandler = new DownloadHandlerTexture();
                var webRequestDispatcher = new WebRequestDispatcher();
                var url = $"{partnerAsset.ImageUrl}?w=64";
                var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler, ctx: token);
                response.ThrowIfError();
                OnAssetIconDownloaded(partnerAsset, response.Texture);
            }
        }

        public void OnAssetIconDownloaded(PartnerAsset partnerAsset, Texture texture)
        {
            var button = selectionElement.CreateButton();
            var partnerAssetData = partnerAsset;
            button.SetIcon(texture);
            button.AddListener(() => AssetSelected(partnerAssetData));
        }

        /// <summary>
        /// This function is called when a template button is clicked.
        /// </summary>
        /// <param name="partnerAssetData">This data is used passed in the AssetSelected event</param>
        private void AssetSelected(PartnerAsset partnerAssetData)
        {
            onAssetSelected?.Invoke(partnerAssetData);
        }
    }
}
