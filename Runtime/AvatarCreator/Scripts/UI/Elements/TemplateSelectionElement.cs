using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class is responsible for fetching avatar template data,
    /// including icons, and creating buttons for each template.
    /// It serves as a self-contained UI element for avatar template selection.
    /// It extends the functionality of SelectionElement to handle AvatarTemplate-specific interactions.
    /// </summary>
    public class TemplateSelectionElement : SelectionElement
    {
        private const string TAG = nameof(TemplateSelectionElement);
        private List<AvatarTemplateData> avatarTemplates;
        private AvatarTemplateFetcher avatarTemplateFetcher;

        private void Awake()
        {
            avatarTemplateFetcher = new AvatarTemplateFetcher();
        }

        /// <summary>
        /// Asynchronously loads avatar template data and creates button elements for each template.
        /// Each button is created with an icon fetched from the template's image URL.
        /// </summary>
        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            CreateButtons(avatarTemplates.ToArray(), async (button, asset) =>
            {
                var webRequestDispatcher = new WebRequestDispatcher();
                var url = $"{asset.ImageUrl}";
                var texture = await webRequestDispatcher.DownloadTexture(url);
                button.SetIcon(texture);
            });
        }

        /// <summary>
        /// Loads avatar template data asynchronously.
        /// This method fetches data but does not handle icon rendering.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation of loading avatar template data.</returns>
        public async Task LoadTemplateData()
        {
            avatarTemplates = await avatarTemplateFetcher.GetTemplates();
            if (avatarTemplates == null || avatarTemplates.Count == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found");
            }
        }
    }
}
