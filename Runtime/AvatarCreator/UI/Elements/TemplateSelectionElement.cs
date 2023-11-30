using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class can be used as a self contained UI element that can fetch AvatarTemplates
    /// and create it's own buttons.
    /// </summary>
    public class TemplateSelectionElement : SelectionElement
    {
        private const string TAG = nameof(TemplateSelectionElement);

        private List<AvatarTemplateData> avatarTemplateDataList;
        private AvatarTemplateFetcher avatarTemplateFetcher;
        
        private void Awake()
        {
            avatarTemplateFetcher = new AvatarTemplateFetcher();
        }

        /// <summary>
        /// This function will automatically fetch the template data including the icon renders and create buttons.
        /// </summary>
        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            CreateButtons();
            await LoadTemplateRenders();
        }

        /// <summary>
        /// Loads avatar template data without fetching the icon renders.
        /// </summary>
        public async Task LoadTemplateData()
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
            avatarTemplateDataList = await avatarTemplateFetcher.GetTemplatesWithRenders(OnIconLoaded);
        }

        private void OnIconLoaded(AvatarTemplateData avatarTemplate)
        {
            UpdateButtonIcon(avatarTemplate.Id, avatarTemplate.Texture);
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
                var button = CreateButton(avatarTemplateDataList[i].Id);
                var templateData = avatarTemplateDataList[i];
                if (templateData.Texture != null)
                {
                    button.SetIcon(templateData.Texture);
                }
                button.AddListener(() => AssetSelected(templateData));
            }
        }
    }
}
