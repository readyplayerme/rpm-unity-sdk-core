using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarAPIRequests
    {
        private const string FULL_BODY = "fullbody";
        private const string HALF_BODY = "halfbody";
        private const string PARTNER = "partner";
        private const string DATA = "data";
        private const string ID = "id";

        private readonly AuthorizedRequest authorizedRequest;
        private readonly CancellationToken ctx;

        public AvatarAPIRequests(CancellationToken ctx = default)
        {
            this.ctx = ctx;
            authorizedRequest = new AuthorizedRequest();
        }

        public async Task<Dictionary<string, string>> GetUserAvatars(string userId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V1_BASE_URL}/avatars/?select=id,partner&userId={userId}",
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );
            response.ThrowIfError();

            var json = JObject.Parse(response.Text);
            var data = json[DATA]!;
            return data.ToDictionary(element => element[ID]!.ToString(), element => element[PARTNER]!.ToString());
        }

        public async Task<List<AvatarTemplateData>> GetAvatarTemplates()
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}avatars/templates",
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );
            response.ThrowIfError();

            var json = JObject.Parse(response.Text);
            var data = json[DATA]!;
            return JsonConvert.DeserializeObject<List<AvatarTemplateData>>(data.ToString());
        }

        public async Task<Texture> GetAvatarTemplateImage(string url)
        {
            var downloadHandler = new DownloadHandlerTexture();
            var webRequestDispatcher = new WebRequestDispatcher();
            var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler, ctx: ctx);

            response.ThrowIfError();
            return response.Texture;
        }

        public async Task<AvatarProperties> CreateFromTemplateAvatar(string templateId, string partner, BodyType bodyType)
        {
            var payloadData = new Dictionary<string, string>
            {
                { nameof(partner), partner },
                { nameof(bodyType), bodyType == BodyType.FullBody ? FULL_BODY : HALF_BODY }
            };

            var payload = AuthDataConverter.CreatePayload(payloadData);

            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}avatars/templates/{templateId}",
                    Method = HttpMethod.POST,
                    Payload = payload
                },
                ctx: ctx
            );

            response.ThrowIfError();

            var json = JObject.Parse(response.Text);
            var data = json[DATA]!.ToString();
            return JsonConvert.DeserializeObject<AvatarProperties>(data);
        }

        public async Task<ColorPalette[]> GetAllAvatarColors(string avatarId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}avatars/{avatarId}/colors?type=skin,beard,hair,eyebrow",
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );

            response.ThrowIfError();
            return ColorResponseHandler.GetColorsFromResponse(response.Text);
        }

        public async Task<AvatarProperties> GetAvatarMetadata(string avatarId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}avatars/{avatarId}.json",
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );

            response.ThrowIfError();

            var json = JObject.Parse(response.Text);
            var data = json[DATA]!.ToString();
            return JsonConvert.DeserializeObject<AvatarProperties>(data);
        }

        public async Task<AvatarProperties> CreateNewAvatar(AvatarProperties avatarProperties)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}/avatars",
                    Method = HttpMethod.POST,
                    Payload = avatarProperties.ToJson(true)
                },
                ctx: ctx
            );
            response.ThrowIfError();

            var metadata = JObject.Parse(response.Text);
            var data = metadata[DATA]!.ToString();
            return JsonConvert.DeserializeObject<AvatarProperties>(data);
        }

        public async Task<byte[]> GetAvatar(string avatarId, bool isPreview = false, string parameters = null)
        {
            var url = $"{Env.RPM_API_V2_BASE_URL}avatars/{avatarId}.glb?";
            
            if (!string.IsNullOrEmpty(parameters))
                url += parameters?.Substring(1) + "&";

            if (isPreview)
                url += "preview=true";
            
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = url,
                    Method = HttpMethod.GET
                },
                ctx: ctx);

            response.ThrowIfError();
            return response.Data;
        }

        public async Task<byte[]> UpdateAvatar(string avatarId, AvatarProperties avatarProperties, string parameters = null)
        {
            var url = $"{Env.RPM_API_V2_BASE_URL}/{avatarId}?responseType=glb&{parameters}";
            
            if (!string.IsNullOrEmpty(parameters))
                url += parameters?.Substring(1);

            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = url,
                    Method = HttpMethod.PATCH,
                    Payload = avatarProperties.ToJson(true)
                },
                ctx: ctx);

            response.ThrowIfError();
            return response.Data;
        }

        public async Task PrecompileAvatar(string avatarId, PrecompileData precompileData, string parameters = null)
        {
            var json = JsonConvert.SerializeObject(precompileData);
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}/{avatarId}/precompile{parameters ?? string.Empty}",
                    Method = HttpMethod.POST,
                    Payload = json
                },
                ctx: ctx);

            response.ThrowIfError();
        }

        public async Task<string> SaveAvatar(string avatarId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Env.RPM_API_V2_BASE_URL}/{avatarId}",
                    Method = HttpMethod.PUT
                },
                ctx: ctx);

            response.ThrowIfError();
            return response.Text;
        }

        public async Task DeleteAvatar(string avatarId, bool isDraft = false)
        {
            var url = $"{Env.RPM_API_V2_BASE_URL}/{avatarId}/";

            if (isDraft)
                url += "draft";
            
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = url,
                    Method = HttpMethod.DELETE
                },
                ctx: ctx);

            response.ThrowIfError();
        }
    }
}
