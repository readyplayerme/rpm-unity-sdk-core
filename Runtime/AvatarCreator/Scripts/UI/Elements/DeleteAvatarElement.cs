using System;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class DeleteAvatarElement : MonoBehaviour
    {
        private const string TAG = nameof(AvatarListElement);

        [SerializeField]
        private Button confirmButton;
        [SerializeField]
        private Button cancelButton;

        public UnityEvent<string> onCancel;
        public UnityEvent<string> onDeletion;

        private string avatarId;
        private AvatarAPIRequests avatarAPIRequests;

        private void Awake()
        {
            confirmButton.onClick.AddListener(DeleteAvatar);
            cancelButton.onClick.AddListener(CancelDeletion);
            avatarAPIRequests = new AvatarAPIRequests();
        }

        public void SetupDeletion(string avatarId)
        {
            this.avatarId = avatarId;
        }

        private void CancelDeletion()
        {
            avatarId = null;
            onCancel?.Invoke(this.avatarId);
        }

        private async void DeleteAvatar()
        {
            if (avatarId == null)
            {
                SDKLogger.LogWarning(TAG, "AvatarId is not set");
                return;
            }
            await avatarAPIRequests.DeleteAvatar(avatarId);
            onDeletion?.Invoke(avatarId);
            avatarId = null;
        }
    }
}
