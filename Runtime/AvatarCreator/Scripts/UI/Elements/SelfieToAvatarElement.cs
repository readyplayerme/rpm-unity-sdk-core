using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// A Unity MonoBehaviour class for converting selfies images into avatars.
    /// Allows capturing a photo and creating an avatar based on provided settings.
    /// </summary>
    public class SelfieToAvatarElement : MonoBehaviour
    {
        private const string TAG = nameof(SelfieToAvatarElement);

        [SerializeField] private BodyType bodyType;
        [SerializeField] private OutfitGender gender;
        [SerializeField] private AvatarConfig avatarConfig;

        [Space(5)]
        [Header("Events")]
        public UnityEvent<GameObject, AvatarProperties> OnAvatarCreated;

        /// <summary>
        /// Called when a photo is captured. Converts the photo to an avatar based on provided settings.
        /// </summary>
        /// <param name="texture">The captured selfie photo as a Texture2D.</param>
        public async void OnPhotoCaptured(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            var byteAsString = Convert.ToBase64String(bytes);

            var avatarProperties = new AvatarProperties();
            avatarProperties.Partner = CoreSettingsHandler.CoreSettings.Subdomain;
            avatarProperties.Base64Image = byteAsString;
            avatarProperties.BodyType = bodyType;
            avatarProperties.Gender = gender;
            avatarProperties.Assets = new Dictionary<AssetType, object>();

            if (!AuthManager.IsSignedIn && !AuthManager.IsSignedInAnonymously)
            {
                SDKLogger.Log(TAG, "Not signed in");
                return;
            }

            var avatarManager = new AvatarManager(avatarConfig);
            var avatar = await avatarManager.CreateAvatar(avatarProperties);

            OnAvatarCreated?.Invoke(avatar.avatarGameObject, avatar.avatarProperties);
        }
    }
}
