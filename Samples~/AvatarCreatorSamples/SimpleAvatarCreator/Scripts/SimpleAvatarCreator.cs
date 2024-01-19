using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.Core;
using UnityEngine;
#pragma warning disable CS4014
#pragma warning disable CS1998 

namespace ReadyPlayerMe.Samples.SimpleAvatarCreator
{
    public class SimpleAvatarCreator : MonoBehaviour
    {
        [SerializeField] private List<AssetSelectionElement> assetSelectionElements;
        [SerializeField] private List<ColorSelectionElement> colorSelectionElements;
        [SerializeField] private RuntimeAnimatorController animationController;
        [SerializeField] private GameObject loading;
      
        private readonly BodyType bodyType = BodyType.FullBody;
        private readonly OutfitGender gender = OutfitGender.Masculine;

        private AvatarManager avatarManager;
        private GameObject avatar;

        private async void Start()
        {
            await AuthManager.LoginAsAnonymous();
            avatarManager = new AvatarManager();

            loading.SetActive(true);
            GetAssets();
            var avatarProperties = await GetAvatar();
            GetColors(avatarProperties);
            loading.SetActive(false);
        }

        private void OnEnable()
        {
            foreach (var element in assetSelectionElements)
            {
                element.onAssetSelected.AddListener(OnAssetSelection);
            }

            foreach (var element in colorSelectionElements)
            {
                element.onAssetSelected.AddListener(OnAssetSelection);
            }
        }

        private void OnDisable()
        {
            foreach (var element in assetSelectionElements)
            {
                element.onAssetSelected.RemoveListener(OnAssetSelection);
            }

            foreach (var element in colorSelectionElements)
            {
                element.onAssetSelected.RemoveListener(OnAssetSelection);
            }
        }

        private async void OnAssetSelection(IAssetData assetData)
        {
            loading.SetActive(true);
            var newAvatar = await avatarManager.UpdateAsset(assetData.AssetType, bodyType, assetData.Id);
            Destroy(avatar);
            avatar = newAvatar;
            SetElements();
            loading.SetActive(false);
        }

        private async void GetAssets()
        {
            foreach (var element in assetSelectionElements)
            {
                element.LoadAndCreateButtons(gender);
            }
        }

        private void GetColors(AvatarProperties avatarProperties)
        {
            foreach (var element in colorSelectionElements)
            {
                element.LoadAndCreateButtons(avatarProperties);
            }
        }

        private async Task<AvatarProperties> GetAvatar()
        {
            var avatarTemplateFetcher = new AvatarTemplateFetcher();
            var templates = await avatarTemplateFetcher.GetTemplates();
            var avatarTemplate = templates[1];

            var templateAvatarProps = await avatarManager.CreateAvatarFromTemplate(avatarTemplate.Id, bodyType);
            avatar = templateAvatarProps.Item1;
            SetElements();
            return templateAvatarProps.Item2;
        }

        private void SetElements()
        {
            avatar.AddComponent<MouseRotationHandler>();
            avatar.AddComponent<AvatarRotator>();
            avatar.GetComponent<Animator>().runtimeAnimatorController = animationController;
        }
    }
}
