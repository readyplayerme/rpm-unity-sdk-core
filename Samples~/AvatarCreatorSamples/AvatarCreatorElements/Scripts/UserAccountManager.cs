using System;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.Samples.AvatarCreatorElements
{
    public class UserAccountManager:MonoBehaviour
    {
        public UnityEvent OnStartLogin;
        public UnityEvent<UserSession> OnLogin;
        private async void Start()
        {
            OnStartLogin?.Invoke();
            if (PlayerPrefs.HasKey("StoredSession"))
            {
                AuthManager.SetUser(JsonUtility.FromJson<UserSession>(PlayerPrefs.GetString("StoredSession")));
            }
            else
            {
                await AuthManager.LoginAsAnonymous();
            }
            OnLogin?.Invoke(AuthManager.UserSession);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString("StoredSession", JsonUtility.ToJson(AuthManager.UserSession));
        }
        
    }
}
