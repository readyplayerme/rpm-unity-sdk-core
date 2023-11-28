using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class can be used to fetch avatar template data including icon renders from the avatarAPI.
    /// </summary>
    public class TemplateFetcher
    {
        private readonly AvatarAPIRequests avatarAPIRequests;

        public TemplateFetcher(CancellationToken ctx = default)
        {
            avatarAPIRequests = new AvatarAPIRequests(ctx);
        }

        /// <summary>
        /// Fetches all avatar templates without the icon renders via the avatarAPI.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TemplateData>> GetTemplates()
        {
            return await avatarAPIRequests.GetTemplates();
        }

        /// <summary>
        /// Fetches all avatar template data with the icon renders via the avatarAPI.
        /// This will wait for all the icons to be downloaded. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TemplateData>> GetTemplatesWithRenders()
        {
            var templates = await avatarAPIRequests.GetTemplates();
            await FetchTemplateRenders(templates);
            return templates;
        }

        /// <summary>
        /// Fetches the renders for all the templates provided.
        /// </summary>
        public async Task FetchTemplateRenders(List<TemplateData> templates)
        {
            var tasks = templates.Select(async templateData =>
            {
                templateData.Texture = await avatarAPIRequests.GetTemplateAvatarImage(templateData.ImageUrl);
            });

            await Task.WhenAll(tasks);
        }
    }
}
