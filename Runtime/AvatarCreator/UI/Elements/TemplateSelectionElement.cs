using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class can be used as a self contained UI element that can fetch AvatarTemplates
    /// and create it's own buttons.
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
        /// This function will automatically fetch the template data including the icon renders and create buttons.
        /// </summary>
        public async void LoadAndCreateButtons()
        {
            await LoadTemplateData();
            CreateButtons(avatarTemplates.ToArray(), async (button, asset) =>
            {
                var downloadHandler = new DownloadHandlerTexture();
                var webRequestDispatcher = new WebRequestDispatcher();
                var url = $"{asset.ImageUrl}?w=64";
                var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler);
                button.SetIcon(response.Texture);
            });
        }

        /// <summary>
        /// Loads avatar template data without fetching the icon renders.
        /// </summary>
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
