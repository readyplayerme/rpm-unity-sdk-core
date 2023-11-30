using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    public class ColorSelectionElement : MonoBehaviour
    {
        private const string TAG = nameof(AssetSelection);
        private CancellationToken token = default;

        [Header("Events")]
        [Space(5)]
        public UnityEvent<string, Category> onColorSelected;
        [SerializeField] private Category category;
        private ColorPalette colorPalette;
        private readonly AvatarAPIRequests avatarAPIRequests = new AvatarAPIRequests();
        private SelectionElement selectionElement;

        private void Awake()
        {
            selectionElement = GetComponent<SelectionElement>();
        }

        public async Task LoadColorPalette(AvatarProperties avatarProperties)
        {
            var colorPalettes = await avatarAPIRequests.GetAllAvatarColors(avatarProperties.Id);
            colorPalette = colorPalettes.FirstOrDefault(x => x.category == category);
        }

        public async void LoadAndCreateButtons(AvatarProperties avatarProperties)
        {
            await LoadColorPalette(avatarProperties);
            CreateButtons();
        }

        public void CreateButtons()
        {
            if (colorPalette.hexColors.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            for (int i = 0; i < colorPalette.hexColors.Length; i++)
            {
                CreateButton(i.ToString(), colorPalette.hexColors[i]);
            }
        }

        public void CreateButton(string id, string hexColor)
        {
            var button = selectionElement.CreateButton();
            button.SetColor(hexColor);
            button.AddListener(() => AssetSelected(id, category));
        }

        /// <summary>
        /// This function is called when a template button is clicked.
        /// </summary>
        /// <param name="partnerAssetData">This data is used passed in the AssetSelected event</param>
        private void AssetSelected(string id, Category category)
        {
            onColorSelected?.Invoke(id, category);
        }
    }
}
