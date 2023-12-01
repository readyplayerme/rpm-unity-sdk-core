﻿using System;
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
        private readonly CancellationToken ctx;
        private readonly AvatarAPIRequests avatarAPIRequests;
        private readonly IconFetcher iconFetcher;

        public AvatarTemplateFetcher(CancellationToken ctx = default)
        {
            this.ctx = ctx;
            avatarAPIRequests = new AvatarAPIRequests(ctx);
            iconFetcher = new IconFetcher(ctx: ctx);
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
                templateData.Texture = await iconFetcher.GetIcon(templateData.ImageUrl);
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
