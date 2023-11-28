using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadyPlayerMe.AvatarCreator.Avatars;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarApi
    {
        private const string FULL_BODY = "fullbody";
        private const string HALF_BODY = "halfbody";
        
        private readonly AuthorizedRequest authorizedRequest;
        private readonly CancellationToken ctx;

        public AvatarApi(CancellationToken ctx = default)
        {
            this.ctx = ctx;
            authorizedRequest = new AuthorizedRequest();
        }

        public async Task<Dictionary<string, string>> GetUserAvatars(string userId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Endpoints.API_V1_BASE_URL}avatars?select=id,partner&userId={userId}",
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );
            response.ThrowIfError();

            var userAvatarResponse = JsonConvert.DeserializeObject<UserAvatarResponse>(response.Text);
            
            return userAvatarResponse.Data.ToDictionary(element =>
                    element.Id!.ToString(),
                element => element.Partner!.ToString()
            );
        }

        public async Task<List<TemplateData>> GetTemplates()
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = AvatarEndpoints.GetAllAvatarTemplatesEndpoint(),
                    Method = HttpMethod.GET
                },
                ctx: ctx
            );
            response.ThrowIfError();

            var avatarTemplateResponse = JsonConvert.DeserializeObject<AvatarTemplateResponse>(response.Text);
            
            return JsonConvert.DeserializeObject<List<TemplateData>>(avatarTemplateResponse.Data.ToString());
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
                    Url = AvatarEndpoints.GetAvatarTemplatesEndpoint(templateId),
                    Method = HttpMethod.POST,
                    Payload = payload
                },
                ctx: ctx
            );

            response.ThrowIfError();

            var createDraftAvatarResponse = JsonConvert.DeserializeObject<CreateDraftAvatarResponse>(response.Text);
            var data = createDraftAvatarResponse.Data!.ToString();
            
            return JsonConvert.DeserializeObject<AvatarProperties>(data);
        }

        public async Task<ColorPalette[]> GetAllAvatarColors(string avatarId)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = $"{Endpoints.API_V2_BASE_URL}avatars/{avatarId}/colors?type=skin,beard,hair,eyebrow",
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
                    Url = AvatarEndpoints.GetAvatarMetadataEndpoint(avatarId),
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
                    Url = AvatarEndpoints.GetCreateEndpoint(),
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
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = AvatarEndpoints.GetAvatarModelEndpoint(avatarId, isPreview, parameters),
                    Method = HttpMethod.GET
                },
                ctx: ctx);

            response.ThrowIfError();
            return response.Data;
        }

        public async Task<byte[]> UpdateAvatar(string avatarId, AvatarProperties avatarProperties, string parameters = null)
        {
            var response = await authorizedRequest.SendRequest<Response>(
                new RequestData
                {
                    Url = AvatarEndpoints.GetUpdateAvatarEndpoint(avatarId, parameters),
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
                    Url = AvatarEndpoints.GetPrecompileEndpoint(avatarId, parameters),
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
                    Url = $"{Endpoints.API_V2_BASE_URL}avatars/{avatarId}",
                    Method = HttpMethod.PUT
                },
                ctx: ctx);

            response.ThrowIfError();
            return response.Text;
        }

        public async Task DeleteAvatar(string avatarId, bool isDraft = false)
        {
            var url = $"{Endpoints.API_V2_BASE_URL}avatars/{avatarId}/";

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
