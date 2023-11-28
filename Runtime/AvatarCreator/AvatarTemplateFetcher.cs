﻿using System.Collections.Generic;
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
        private readonly CancellationToken ctx;
        private readonly AvatarAPIRequests avatarAPIRequests;

        public AvatarTemplateFetcher(CancellationToken ctx = default)
        {
            this.ctx = ctx;
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
            return await FetchTemplateRenders(templates);
        }

        /// <summary>
        /// Fetches the renders for all the templates provided.
        /// </summary>
        public async Task<List<AvatarTemplateData>> FetchTemplateRenders(List<AvatarTemplateData> templates)
        {
            var tasks = templates.Select(async templateData =>
            {
                templateData.Texture = await avatarAPIRequests.GetAvatarTemplateImage(templateData.ImageUrl);
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
