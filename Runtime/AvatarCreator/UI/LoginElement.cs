using System;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class LoginElement : MonoBehaviour
    {
        private const string TAG = nameof(LoginElement);
        [Header("Input Fields")]
        [SerializeField, Tooltip("Input field for entering email address")] private InputField emailField;
        [SerializeField, Tooltip("Input field for entering verification code")] private InputField codeField;

        [Header("Events")]
        [SerializeField] private UnityEvent OnLoginSuccess;
        [SerializeField] private UnityEvent<string> OnLoginFail;

        private void OnEnable()
        {
            AuthManager.OnSignInError += OnSignInError;
        }

        private void OnDisable()
        {
            AuthManager.OnSignInError -= OnSignInError;
        }

        public void SendVerificationCode()
        {
            AuthManager.SendEmailCode(emailField.text);
        }

        public async void LoginWithCode()
        {
            try
            {
                if (await AuthManager.LoginWithCode(codeField.text))
                {
                    OnLoginSuccess?.Invoke();
                    SDKLogger.Log(TAG, "Login with code successful");
                }
            }
            catch (Exception e)
            {
                OnSignInError(e.Message);
            }
        }

        private void OnSignInError(string error)
        {
            OnLoginFail?.Invoke(error);
            SDKLogger.Log(TAG, $"Login failed with error: {error}");
        }
    }
}
