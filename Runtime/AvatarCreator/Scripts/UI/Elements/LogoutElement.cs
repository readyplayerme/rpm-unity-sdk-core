using System;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// This class provides all the functionality required to create a basic Logout UI element.
    /// This is used to log out users from RPM accounts, once they have logged in.
    /// </summary>
    public class LogoutElement : MonoBehaviour
    {
        private const string TAG = nameof(LoginElement);
        [SerializeField] private UnityEvent OnLogout;
        [SerializeField] private UnityEvent<string> OnLogoutFail;
        public async void Logout()
        {
            AuthManager.Logout();
            try
            {
                await AuthManager.LoginAsAnonymous();
                LogOutSucceeded();
            }
            catch (Exception e)
            {
                LogOutFailed(e.Message);
            }
        }

        private void LogOutFailed(string error)
        {
            SDKLogger.Log(TAG, $"Login failed with error: {error}");
            OnLogoutFail?.Invoke(error);
        }

        private void LogOutSucceeded()
        {
            SDKLogger.Log(TAG, "Log out successful");
            OnLogout?.Invoke();
        }
    }
}
