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
    [RequireComponent(typeof(SelectionElement))]
    public class AvatarTemplateSelection : MonoBehaviour
    {
        private const string TAG = nameof(AvatarTemplateSelection);

        [Header("Events")]
        [Space(5)]
        public UnityEvent<AvatarTemplateData> onTemplateSelected;

        private List<AvatarTemplateData> avatarTemplateDataList;
        private AvatarTemplateFetcher avatarTemplateFetcher;
        private SelectionElement selectionElement;

        private void Awake()
        {
            avatarTemplateFetcher = new AvatarTemplateFetcher();
            selectionElement = GetComponent<SelectionElement>();
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
            avatarTemplateDataList = await avatarTemplateFetcher.GetTemplates();
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
            avatarTemplateDataList = await avatarTemplateFetcher.GetTemplatesWithRenders();
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
            for (var i = 0; i < avatarTemplateDataList.Count; i++)
            {
                var button = selectionElement.CreateButton();
                var templateData = avatarTemplateDataList[i];
                button.SetIcon(templateData.Texture);
                button.AddListener(() => TemplateSelected(templateData));
            }
        }

        /// <summary>
        /// This function is called when a template button is clicked.
        /// </summary>
        /// <param name="buttonTransform">The buttonTransform is used to position the selectedIcon to indicate which button was last selected</param>
        /// <param name="avatarTemplateData">This data is used passed in the onSelectedTemplate event</param>
        private void TemplateSelected(AvatarTemplateData avatarTemplateData)
        {
            onTemplateSelected?.Invoke(avatarTemplateData);
        }
    }
}
