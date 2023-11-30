using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    public struct RequestData
    {
        public string Url;
        public HttpMethod Method;
        public string Payload;
        public DownloadHandler DownloadHandler;
    }

    public class AuthorizedRequest
    {
        public async Task<T> SendRequest<T>(RequestData requestData, CancellationToken ctx = new CancellationToken()) where T : IResponse, new()
        {
            var response = await Send<T>(requestData, ctx);

            if (response is { IsSuccess: false, ResponseCode: 401 })
            {
                await AuthManager.RefreshToken();

                response = await Send<T>(requestData, ctx);
            }

            return response;
        }

        private async Task<T> Send<T>(RequestData requestData, CancellationToken ctx) where T : IResponse, new()
        {
            var headers = CommonHeaders.GetHeadersWithAppId();
            headers.Add("Authorization", $"Bearer {AuthManager.UserSession.Token}");

            var webRequestDispatcher = new WebRequestDispatcher();
            return await webRequestDispatcher.SendRequest<T>(
                requestData.Url,
                requestData.Method,
                headers,
                requestData.Payload,
                requestData.DownloadHandler,
                ctx: ctx
            );
        }
    }
}
