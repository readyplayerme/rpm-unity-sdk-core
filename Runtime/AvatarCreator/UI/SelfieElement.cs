using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    public class SelfieElement : MonoBehaviour
    {
        private const string TAG = nameof(SelfieElement);

        [SerializeField] private ImageConfirmationElement imageConfirmationElement;
        [SerializeField] private BodyType bodyType;
        [SerializeField] private OutfitGender gender;

        public UnityEvent<GameObject,AvatarProperties> OnAvatarCreated;

        private void OnEnable()
        {
            imageConfirmationElement.onImageConfirmed.AddListener(OnPhotoCaptured);
        }

        private void OnDisable()
        {
            imageConfirmationElement.onImageConfirmed.RemoveListener(OnPhotoCaptured);
        }

        private async void OnPhotoCaptured(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            var byteAsString = Convert.ToBase64String(bytes);

            var avatarProperties = new AvatarProperties();
            avatarProperties.Partner = CoreSettingsHandler.CoreSettings.Subdomain;
            avatarProperties.Base64Image = byteAsString;
            avatarProperties.BodyType = bodyType;
            avatarProperties.Gender = gender;
            avatarProperties.Assets = new Dictionary<Category, object>();

            if (!AuthManager.IsSignedIn && !AuthManager.IsSignedInAnonymously)
            {
                SDKLogger.Log(TAG, "Not signed in");
                await AuthManager.LoginAsAnonymous();
                // return;
            }
            
            var avatarManager = new AvatarManager(bodyType);
            var avatar = await avatarManager.CreateAvatar(avatarProperties);

            OnAvatarCreated?.Invoke(avatar.Item1, avatar.Item2);
        }
    }
}
