using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public enum TemplateVersions
    {
        All,
        V1,
        V2
    }

    /// <summary>
    /// This class can be used to fetch avatar template data including icon renders from the avatarAPI.
    /// </summary>
    public class AvatarTemplateFetcher
    {
        private readonly CancellationToken ctx;
        private readonly AvatarAPIRequests avatarAPIRequests;
        private const string TEMPLATE_V2_USAGE_TYPE = "onboarding";
        private const string TEMPLATE_V1_USAGE_TYPE = "randomize";
        private readonly TemplateVersions templateVersions;

        public AvatarTemplateFetcher(CancellationToken ctx = default, TemplateVersions templateVersions = TemplateVersions.V2)
        {
            this.ctx = ctx;
            this.templateVersions = templateVersions;
            avatarAPIRequests = new AvatarAPIRequests(ctx);
        }

        /// <summary>
        /// Fetches all avatar templates without the icon renders via the avatarAPI.
        /// </summary>
        /// <returns></returns>
        public async Task<List<AvatarTemplateData>> GetTemplates(OutfitGender gender = OutfitGender.None)
        {
            var templates = await avatarAPIRequests.GetAvatarTemplates();
            return templates.Where(template => HasCorrectTemplateVersion(template) && HasCorrectGender(template)).ToList();

            bool HasCorrectGender(AvatarTemplateData template)
            {
                if (gender == OutfitGender.None || template.Gender == OutfitGender.None)
                {
                    return true;
                }
                return gender == template.Gender;
            }

            bool HasCorrectTemplateVersion(AvatarTemplateData template)
            {
                switch (templateVersions)
                {
                    case TemplateVersions.V2:
                        return template.UsageType.Contains(TEMPLATE_V2_USAGE_TYPE);
                    case TemplateVersions.V1:
                        return template.UsageType.Contains(TEMPLATE_V1_USAGE_TYPE);
                    case TemplateVersions.All:
                    default:
                        return true;
                }
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
