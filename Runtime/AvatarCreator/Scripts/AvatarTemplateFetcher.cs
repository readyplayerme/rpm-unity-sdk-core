using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public enum TemplateUsageType
    {
        All,
        Onboarding,
        Randomize
    }

    /// <summary>
    /// This class can be used to fetch avatar template data including icon renders from the avatarAPI.
    /// </summary>
    public class AvatarTemplateFetcher
    {
        private readonly CancellationToken ctx;
        private readonly AvatarAPIRequests avatarAPIRequests;
        private const string TEMPLATE_ONBOARDING_USAGE_TYPE = "onboarding";
        private const string TEMPLATE_RANDOMIZE_USAGE_TYPE = "randomize";
        private readonly TemplateUsageType templateUsageType;

        public AvatarTemplateFetcher(CancellationToken ctx = default, TemplateUsageType templateUsageType = TemplateUsageType.Onboarding)
        {
            this.ctx = ctx;
            this.templateUsageType = templateUsageType;
            avatarAPIRequests = new AvatarAPIRequests(ctx);
        }

        /// <summary>
        /// Fetches all avatar templates without the icon renders via the avatarAPI.
        /// </summary>
        /// <returns></returns>
        public async Task<List<AvatarTemplateData>> GetTemplates()
        {
            var templates = await avatarAPIRequests.GetAvatarTemplates();
            switch (templateUsageType)
            {
                case TemplateUsageType.Onboarding:
                    return templates.Where(template => template.UsageType.Contains(TEMPLATE_ONBOARDING_USAGE_TYPE)).ToList();
                case TemplateUsageType.Randomize:
                    var filteredTemplates = templates.Where(template => template.UsageType.Contains(TEMPLATE_ONBOARDING_USAGE_TYPE)).ToList();
                    return filteredTemplates;
                case TemplateUsageType.All:
                default:
                    return templates;
            }
        }

        /// <summary>
        /// Fetches all avatar template data with the icon renders via the avatarAPI.
        /// This will wait for all the icons to be downloaded. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AvatarTemplateData>> GetTemplatesWithRenders(Action<AvatarTemplateData> onIconDownloaded = null)
        {
            return await FetchTemplateRenders(await avatarAPIRequests.GetAvatarTemplates(), onIconDownloaded);
        }

        /// <summary>
        /// Fetches the renders for all the templates provided.
        /// </summary>
        public async Task<List<AvatarTemplateData>> FetchTemplateRenders(List<AvatarTemplateData> templates, Action<AvatarTemplateData> onIconDownloaded = null)
        {
            var tasks = templates.Select(async templateData =>
            {
                var requestDispatcher = new WebRequestDispatcher();
                templateData.Texture = await requestDispatcher.DownloadTexture(templateData.ImageUrl, ctx);
                onIconDownloaded?.Invoke(templateData);
            }).ToList();

            while (!tasks.All(x => x.IsCompleted) &&
                   !ctx.IsCancellationRequested)
            {
                await Task.Yield();
            }

            return templates;
        }
    }
}
