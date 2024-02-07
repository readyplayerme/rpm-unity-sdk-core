using System;
using System.Collections.Generic;
using System.Linq;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AvatarListElement : MonoBehaviour
    {
        private enum AvatarListFilter
        {
            Application,
            None
        }

        private const string TAG = nameof(AvatarListElement);

        [SerializeField] private Transform avatarsContainer;
        [FormerlySerializedAs("avatarListButton")]
        [SerializeField] private AvatarListItem avatarListItem;
        [SerializeField] private AvatarListFilter avatarListFilter;
        public UnityEvent<string> onAvatarSelect;
        public UnityEvent<string> onAvatarModify;
        public UnityEvent<string> onAvatarDeletionStarted;

        private AvatarAPIRequests avatarAPIRequests;

        private readonly Dictionary<string, AvatarListItem> avatarListItems = new Dictionary<string, AvatarListItem>();

        public async void LoadAndCreateUserAvatars()
        {
            if (!AuthManager.IsSignedIn && !AuthManager.IsSignedInAnonymously)
            {
                SDKLogger.Log(TAG, "Not signed in");
                return;
            }

            avatarAPIRequests ??= new AvatarAPIRequests();

            var avatarPartnerArr = await avatarAPIRequests.GetUserAvatars(AuthManager.UserSession.Id);

            if (avatarListFilter == AvatarListFilter.Application)
            {
                var avatars = avatarPartnerArr.Where((pair) => pair.Value == CoreSettingsHandler.CoreSettings.Subdomain).Select(pair => pair.Key);
                CreateButtons(avatars);
            }
            else
            {
                CreateButtons(avatarPartnerArr.Keys);
            }

        }

        public void RemoveItem(string avatarId)
        {
            if (!avatarListItems.ContainsKey(avatarId))
            {
                return;
            }

            Destroy(avatarListItems[avatarId].gameObject);
            avatarListItems.Remove(avatarId);
        }

        private void CreateButtons(IEnumerable<string> avatars)
        {
            ClearButtons();
            foreach (var avatarId in avatars)
            {
                CreateButton(avatarId);
            }

        }

        private void ClearButtons()
        {
            if (avatarListItems.Count > 0)
            {
                foreach (var avatarListItem in avatarListItems)
                {
                    Destroy(avatarListItem.Value.gameObject);
                }
            }
            avatarListItems.Clear();
        }

        private void CreateButton(string avatarId)
        {
            var avatarListElement = Instantiate(avatarListItem, avatarsContainer).GetComponent<AvatarListItem>();
            avatarListElement.AddListener(() => OnAvatarModified(avatarId), () => OnAvatarSelected(avatarId), () => onAvatarDeletionStarted.Invoke(avatarId));
            avatarListElement.SetIcon(avatarId);
            avatarListItems.Add(avatarId, avatarListElement);
        }

        private void OnAvatarModified(string avatarId)
        {
            SDKLogger.Log(TAG, $"Started modifying avatar with id {avatarId}");
            onAvatarModify?.Invoke(avatarId);
        }

        private void OnAvatarSelected(string avatarId)
        {
            SDKLogger.Log(TAG, $"Selected avatar with id {avatarId}");
            onAvatarSelect?.Invoke(avatarId);
        }
    }
}
