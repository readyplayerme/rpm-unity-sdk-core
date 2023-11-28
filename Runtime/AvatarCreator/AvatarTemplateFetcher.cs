using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class can be used to fetch avatar template data including icon renders from the avatarAPI.
    /// </summary>
    public class AvatarTemplateFetcher
    {
        private readonly AvatarAPIRequests avatarAPIRequests;

        public AvatarTemplateFetcher(CancellationToken ctx = default)
        {
            avatarAPIRequests = new AvatarAPIRequests(ctx);
        }

        /// <summary>
        /// Fetches all avatar templates without the icon renders via the avatarAPI.
        /// </summary>
        /// <returns></returns>
        public async Task<List<AvatarTemplateData>> GetTemplates()
        {
            return await avatarAPIRequests.GetAvatarTemplates();
        }

        /// <summary>
        /// Fetches all avatar template data with the icon renders via the avatarAPI.
        /// This will wait for all the icons to be downloaded. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AvatarTemplateData>> GetTemplatesWithRenders()
        {
            var templates = await avatarAPIRequests.GetAvatarTemplates();
            await FetchTemplateRenders(templates);
            return templates;
        }

        /// <summary>
        /// Fetches the renders for all the templates provided.
        /// </summary>
        public async Task FetchTemplateRenders(List<AvatarTemplateData> templates)
        {
            var tasks = templates.Select(async templateData =>
            {
                templateData.Texture = await avatarAPIRequests.GetAvatarTemplateImage(templateData.ImageUrl);
            });

            await Task.WhenAll(tasks);
        }
    }
}
