using System.Collections.Generic;
using System.Linq;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        [SerializeField] private UserAvatarElement userAvatarElement;
        [SerializeField] private AvatarListFilter avatarListFilter;

        public UnityEvent<string> onAvatarSelect;
        public UnityEvent<string> onAvatarModify;
        public UnityEvent<string> onAvatarDeletionStarted;
        public UnityEvent<IEnumerable<string>> onAvatarsLoaded;

        private AvatarAPIRequests avatarAPIRequests;

        private readonly Dictionary<string, UserAvatarElement> partnerByAvatarId = new Dictionary<string, UserAvatarElement>();

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
            if (!partnerByAvatarId.ContainsKey(avatarId))
            {
                return;
            }

            Destroy(partnerByAvatarId[avatarId].gameObject);
            partnerByAvatarId.Remove(avatarId);
        }

        private void CreateButtons(IEnumerable<string> avatars)
        {
            ClearButtons();
            foreach (var avatarId in avatars)
            {
                CreateButton(avatarId);
            }
            onAvatarsLoaded?.Invoke(avatars);
        }

        private void ClearButtons()
        {
            if (partnerByAvatarId.Count > 0)
            {
                foreach (var avatarListItem in partnerByAvatarId)
                {
                    Destroy(avatarListItem.Value.gameObject);
                }
            }
            partnerByAvatarId.Clear();
        }

        private void CreateButton(string avatarId)
        {
            var avatarListElement = Instantiate(userAvatarElement, avatarsContainer).GetComponent<UserAvatarElement>();
            avatarListElement.SetupButton(avatarId,
                new UserAvatarElement.AvatarListItemAction { actionToPerform = () => OnAvatarModified(avatarId), actionType = UserAvatarElement.ButtonAction.Customize },
                new UserAvatarElement.AvatarListItemAction { actionToPerform = () => OnAvatarSelected(avatarId), actionType = UserAvatarElement.ButtonAction.Select },
                new UserAvatarElement.AvatarListItemAction { actionToPerform = () => onAvatarDeletionStarted?.Invoke(avatarId), actionType = UserAvatarElement.ButtonAction.Delete });
            partnerByAvatarId.Add(avatarId, avatarListElement);
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
