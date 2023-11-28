using System;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    public static class ImageApi
    {
        public static async Task<Texture> DownloadImageAsync(string url, Action<Texture> completed = null, CancellationToken ctx = default)
        {
            var downloadHandler = new DownloadHandlerTexture();
            var webRequestDispatcher = new WebRequestDispatcher();
            var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler, ctx: ctx);

            response.ThrowIfError();
            completed?.Invoke(response.Texture);
            
            return response.Texture;
        }
    }
}
