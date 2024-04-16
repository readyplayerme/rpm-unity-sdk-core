using System;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// Provides methods for managing the user's authentication and session.
    /// </summary>
    public static class AuthManager
    {
        private const string TAG = nameof(AuthManager);
        private static readonly AuthAPIRequests AuthAPIRequests;
        private static UserSession userSession;
        public static UserSession UserSession => userSession;

        public static bool IsSignedIn;
        public static bool IsSignedInAnonymously;

        public static Action<UserSession> OnSignedIn;
        public static Action<UserSession> OnSessionRefreshed;
        public static Action OnSignedOut;
        public static Action<string> OnSignInError;

        static AuthManager()
        {
            AuthAPIRequests = new AuthAPIRequests(CoreSettingsHandler.CoreSettings.Subdomain);
        }

        public static async Task LoginAsAnonymous()
        {
            userSession = await AuthAPIRequests.LoginAsAnonymous();
            IsSignedInAnonymously = true;
        }

        public static void SetUser(UserSession session)
        {
            userSession = session;
            IsSignedIn = true;
            OnSignedIn?.Invoke(userSession);
        }

        public static async void SendEmailCode(string email)
        {
            await AuthAPIRequests.SendCodeToEmail(email, userSession.Id);
        }

        public static async Task<bool> LoginWithCode(string otp, string userIdToMerge = null)
        {
            try
            {
                userSession = await AuthAPIRequests.LoginWithCode(otp, userIdToMerge);
                IsSignedIn = true;
                OnSignedIn?.Invoke(userSession);
                return true;
            }
            catch (Exception e)
            {
                OnSignInError?.Invoke(e.Message);
                return false;
            }
        }

        public static async void Signup(string email)
        {
            await AuthAPIRequests.Signup(email, userSession.Id);
        }

        public static async Task RefreshToken()
        {
            RefreshTokenResponse newTokens;
            try
            {
                newTokens = await AuthAPIRequests.GetRefreshToken(userSession.Token, userSession.RefreshToken);
            }
            catch (Exception e)
            {
                SDKLogger.Log(TAG, "Refreshing token failed with error: " + e.Message);
                throw;
            }

            userSession.Token = newTokens.Token;
            userSession.RefreshToken = newTokens.RefreshToken;
            OnSessionRefreshed?.Invoke(userSession);
        }

        public static void Logout()
        {
            IsSignedIn = false;
            IsSignedInAnonymously = false;
            userSession = new UserSession();
            OnSignedOut?.Invoke();
        }

        public static void StoreLastModifiedAvatar(string avatarId)
        {
            userSession.LastModifiedAvatarId = avatarId;
        }
    }
}
