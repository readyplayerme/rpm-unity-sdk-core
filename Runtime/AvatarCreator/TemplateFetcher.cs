using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadyPlayerMe.AvatarCreator
{
    public class TemplateFetcher
    {
        private readonly AvatarAPIRequests avatarAPIRequests;

        public TemplateFetcher(CancellationToken ctx = default)
        {
            avatarAPIRequests = new AvatarAPIRequests(ctx);
        }

        public async Task<List<TemplateData>> GetTemplates()
        {
            return await avatarAPIRequests.GetTemplates();
        }

        public async Task<List<TemplateData>> GetTemplatesWithRenders()
        {
            var templates = await avatarAPIRequests.GetTemplates();
            await FetchTemplateRendersParallel(templates);
            return templates;
        }

        public async Task FetchTemplateRendersParallel(List<TemplateData> templates)
        {
            var tasks = templates.Select(FetchTemplateRenderAsync);
            await Task.WhenAll(tasks);
        }

        public async Task FetchTemplateRenderAsync(TemplateData templateData)
        {
            templateData.Texture = await avatarAPIRequests.GetTemplateAvatarImage(templateData.ImageUrl);
        }
    }
}
