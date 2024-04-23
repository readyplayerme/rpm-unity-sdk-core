using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarCreationResponse
    {
        public GameObject AvatarObject { get; private set; }
        public AvatarProperties Properties { get; private set; }

        public AvatarCreationResponse(GameObject avatarObject, AvatarProperties properties)
        {
            AvatarObject = avatarObject;
            Properties = properties;
        }
    }

    /// <summary>
    /// It is responsible for creating a new avatar, updating and deleting an avatar.
    /// </summary>
    public class AvatarManager : IDisposable
    {
        private const string TAG = nameof(AvatarManager);
        private readonly AvatarAPIRequests avatarAPIRequests;
        private readonly string avatarConfigParameters;
        private readonly InCreatorAvatarLoader inCreatorAvatarLoader;
        private readonly CancellationTokenSource ctxSource;
        private OutfitGender gender;
        public Action<string> OnError { get; set; }

        public string AvatarId => avatarId;
        private string avatarId;

        /// <param name="avatarConfig">Config for downloading preview avatar</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="gender"></param>
        public AvatarManager(AvatarConfig avatarConfig = null, CancellationToken token = default, OutfitGender gender = OutfitGender.None)
        {
            this.gender = gender;
            if (avatarConfig != null)
            {
                avatarConfigParameters = AvatarConfigProcessor.ProcessAvatarConfiguration(avatarConfig);
            }

            ctxSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            inCreatorAvatarLoader = new InCreatorAvatarLoader();
            avatarAPIRequests = new AvatarAPIRequests(ctxSource.Token);
        }

        /// <summary>
        /// Create a new avatar.
        /// </summary>
        /// <param name="avatarProperties">Properties which describes avatar</param>
        /// <returns>AvatarCreationResponse: Result of avatar creation including the GameObject and its properties</returns>
        public async Task<AvatarCreationResponse> CreateAvatarAsync(AvatarProperties avatarProperties)
        {
            GameObject avatar = null;
            try
            {
                avatarProperties = await avatarAPIRequests.CreateNewAvatar(avatarProperties);
                gender = avatarProperties.Gender;
                if (ctxSource.IsCancellationRequested)
                {
                    return new AvatarCreationResponse(null, avatarProperties);
                }

                avatarId = avatarProperties.Id;
                avatar = await GetAvatar(avatarId, avatarProperties.BodyType, true);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return new AvatarCreationResponse(avatar, avatarProperties);
            }

            return new AvatarCreationResponse(avatar, avatarProperties);
        }

        /// <summary>
        /// Create a new avatar.
        /// </summary>
        /// <param name="avatarProperties">Properties which describes avatar</param>
        /// <returns>A tuple in the form of (GameObject, AvatarProperties)</returns>
        [Obsolete("This method is deprecated. Use CreateAvatarAsync instead.")]
        public async Task<(GameObject avatarGameObject, AvatarProperties avatarProperties)> CreateAvatar(AvatarProperties avatarProperties)
        {
            GameObject avatar = null;
            try
            {
                avatarProperties = await avatarAPIRequests.CreateNewAvatar(avatarProperties);
                gender = avatarProperties.Gender;
                if (ctxSource.IsCancellationRequested)
                {
                    return (null, avatarProperties);
                }

                avatarId = avatarProperties.Id;
                avatar = await GetAvatar(avatarId, avatarProperties.BodyType, true);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return (avatar, avatarProperties);
            }

            return (avatar, avatarProperties);
        }

        /// <summary>
        /// Create a new avatar from a provided template.
        /// </summary>
        /// <param name="id">Template id</param>
        /// <param name="bodyType">Avatar body type</param>
        /// <returns>A tuple in the form of (GameObject, AvatarProperties)</returns>
        [Obsolete("This method is deprecated. Use CreateAvatarFromTemplateAsync instead.")]
        public async Task<(GameObject, AvatarProperties)> CreateAvatarFromTemplate(string id, BodyType bodyType)
        {
            GameObject avatar = null;
            var avatarProperties = new AvatarProperties();
            try
            {
                avatarProperties = await avatarAPIRequests.CreateFromTemplateAvatar(
                    id,
                    CoreSettingsHandler.CoreSettings.Subdomain,
                    bodyType
                );
                gender = avatarProperties.Gender;
                if (ctxSource.IsCancellationRequested)
                {
                    return (null, avatarProperties);
                }
                avatarProperties.isDraft = true;
                avatarId = avatarProperties.Id;
                avatar = await GetAvatar(avatarId, bodyType, true);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return (avatar, avatarProperties);
            }

            return (avatar, avatarProperties);
        }

        /// <summary>
        /// Create a new avatar from a provided template.
        /// </summary>
        /// <param name="id">Template id</param>
        /// <param name="bodyType">Avatar body type</param>
        /// <returns>AvatarCreationResponse: Result of avatar creation including the GameObject and its properties</returns>
        public async Task<AvatarCreationResponse> CreateAvatarFromTemplateAsync(string id, BodyType bodyType)
        {
            GameObject avatar = null;
            var avatarProperties = new AvatarProperties();
            try
            {
                avatarProperties = await avatarAPIRequests.CreateFromTemplateAvatar(
                    id,
                    CoreSettingsHandler.CoreSettings.Subdomain,
                    bodyType
                );
                gender = avatarProperties.Gender;
                if (ctxSource.IsCancellationRequested)
                {
                    return new AvatarCreationResponse(null, avatarProperties);
                }
                avatarProperties.isDraft = true;
                avatarId = avatarProperties.Id;
                avatar = await GetAvatar(avatarProperties.Id, bodyType, true);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return new AvatarCreationResponse(avatar, avatarProperties);
            }

            return new AvatarCreationResponse(avatar, avatarProperties);
        }

        /// <summary>
        /// Precompile an avatar on server to increase the fetching speed.
        /// </summary>
        /// <param name="id">Avatar id</param>
        /// <param name="precompileData">Precompiled data for assets</param>
        public async void PrecompileAvatar(string id, PrecompileData precompileData)
        {
            if (string.IsNullOrEmpty(id))
            {
                SDKLogger.LogWarning(TAG, "ID not set. Precompiled avatar request cancelled.");
                return;
            }
            try
            {
                await avatarAPIRequests.PrecompileAvatar(id, precompileData, avatarConfigParameters);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                SDKLogger.LogWarning(TAG, "Precompiled avatar request failed.");
            }

            if (ctxSource.IsCancellationRequested)
            {
                SDKLogger.LogWarning(TAG, "Precompiled avatar request cancelled.");
            }
        }

        /// <summary>
        /// Download and import avatar.
        /// </summary>
        /// <param name="id">Avatar id</param>
        /// <param name="isPreview">Whether its a preview avatar</param>
        /// <returns>Avatar gameObject</returns>
        public async Task<GameObject> GetAvatar(string id, BodyType bodyType, bool isPreview = false)
        {
            avatarId = id;
            byte[] data;
            try
            {
                data = await avatarAPIRequests.GetAvatar(avatarId, isPreview, avatarConfigParameters);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return null;
            }

            if (ctxSource.IsCancellationRequested)
            {
                return null;
            }

            return await inCreatorAvatarLoader.Load(avatarId, bodyType, gender, data);
        }

        public async Task<AvatarProperties> GetAvatarProperties(string id)
        {
            avatarId = id;
            var avatarProperties = new AvatarProperties();
            try
            {
                avatarProperties = await avatarAPIRequests.GetAvatarProperties(avatarId);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }

            return avatarProperties;
        }

        /// <summary>
        /// Update an asset of the avatar.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="assetType"></param>
        /// <returns>Avatar gameObject</returns>
        public async Task<GameObject> UpdateAsset(AssetType assetType, BodyType bodyType, object assetId)
        {
            var payload = new AvatarProperties
            {
                Assets = new Dictionary<AssetType, object>()
            };

            if (assetType == AssetType.Top || assetType == AssetType.Bottom || assetType == AssetType.Footwear)
            {
                payload.Assets.Add(AssetType.Outfit, string.Empty);
            }

            payload.Assets.Add(assetType, assetId);
            byte[] data;
            try
            {
                data = await avatarAPIRequests.UpdateAvatar(avatarId, payload, avatarConfigParameters);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return null;
            }

            if (ctxSource.IsCancellationRequested)
            {
                return null;
            }

            return await inCreatorAvatarLoader.Load(avatarId, bodyType, gender, data);
        }

        public async Task<Dictionary<AssetType, AssetColor[]>> LoadAvatarColors()
        {
            var assetColorsByAssetType = new Dictionary<AssetType, AssetColor[]>();
            try
            {
                var assetColors = await avatarAPIRequests.GetAvatarColors(avatarId);
                assetColorsByAssetType = assetColors
                    .GroupBy(color => color.AssetType)
                    .ToDictionary(group => group.Key, group => group.ToArray());
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }

            return assetColorsByAssetType;
        }

        /// <summary>
        /// Saves the avatar from temp to permanent storage. 
        /// </summary>
        public async Task<string> Save()
        {
            try
            {
                await avatarAPIRequests.SaveAvatar(avatarId);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
                return null;
            }

            return avatarId;
        }

        /// <summary>
        /// This will delete the avatar draft which have not been saved. 
        /// </summary>
        public async Task Delete(bool isDraft)
        {
            try
            {
                await avatarAPIRequests.DeleteAvatar(avatarId, isDraft);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        public void Dispose()
        {
            ctxSource?.Cancel();
        }
    }
}
