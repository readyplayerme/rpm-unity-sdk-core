using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class can be used as a self contained UI element that can fetch AvatarTemplates
    /// and create it's own buttons.
    /// </summary>
    public class TemplateAvatarElement : MonoBehaviour
    {
        private const string TAG = nameof(TemplateAvatarElement);

        [Header("UI Elements")]
        [Space(5)]
        [SerializeField] private ButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;

        [Header("Events")]
        [Space(5)]
        public UnityEvent<TemplateData> onTemplateSelected;

        private List<TemplateData> avatarTemplateDataList;
        private ButtonElement[] templateAvatarButtons;
        private TemplateFetcher templateFetcher;

        private void Awake()
        {
            templateFetcher = new TemplateFetcher();
        }

        /// <summary>
        /// This function will automatically fetch the template data including the icon renders and create buttons.
        /// </summary>
        public async void LoadAndCreateButtons()
        {
            await LoadTemplateRenders();
            CreateButtons();
        }

        /// <summary>
        /// Loads avatar template data without fetching the icon renders.
        /// </summary>
        public async void LoadTemplateData()
        {
            avatarTemplateDataList = await templateFetcher.GetTemplates();
            if (avatarTemplateDataList == null || avatarTemplateDataList.Count == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found");
            }
        }

        /// <summary>
        /// Loads avatar template data and icon renders. This will wait for all the icons to be downloaded.
        /// </summary>
        public async Task LoadTemplateRenders()
        {
            avatarTemplateDataList = await templateFetcher.GetTemplatesWithRenders();
        }

        /// <summary>
        /// Creates buttons from the loaded template data and sets the icon and button event. 
        /// </summary>
        public void CreateButtons()
        {
            if (avatarTemplateDataList.Count == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            templateAvatarButtons = new ButtonElement[avatarTemplateDataList.Count];
            for (var i = 0; i < avatarTemplateDataList.Count; i++)
            {
                var button = Instantiate(buttonElementPrefab, buttonContainer);
                var templateData = avatarTemplateDataList[i];
                button.SetIcon(templateData.Texture);
                button.AddListener(() => TemplateSelected(button.transform, templateData));
                templateAvatarButtons[i] = button;
            }
        }

        private void ClearButtons()
        {
            if (templateAvatarButtons == null) return;

            foreach (var button in templateAvatarButtons)
            {
                Destroy(button.gameObject);
            }
            templateAvatarButtons = null;
        }

        /// <summary>
        /// This function is called when a template button is clicked.
        /// </summary>
        /// <param name="buttonTransform">The buttonTransform is used to position the selectedIcon to indicate which button was last selected</param>
        /// <param name="templateData">This data is used passed in the onSelectedTemplate event</param>
        private void TemplateSelected(Transform buttonTransform, TemplateData templateData)
        {
            onTemplateSelected?.Invoke(templateData);
            SetButtonSelected(buttonTransform);
        }

        /// <summary>
        /// Sets the position and parent of the SelectedIcon to indicate which button was last selected.
        /// </summary>
        /// <param name="buttonTransform"></param>
        private void SetButtonSelected(Transform buttonTransform)
        {
            selectedIcon.transform.SetParent(buttonTransform);
            selectedIcon.transform.localPosition = Vector3.zero;
            selectedIcon.SetActive(true);
        }
    }
}
