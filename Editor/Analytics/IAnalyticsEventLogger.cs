using ReadyPlayerMe.Core.Editor;

namespace ReadyPlayerMe.Core.Analytics
{
    public interface IAnalyticsEventLogger
    {
        void Enable();
        void Disable();
        void IdentifyUser();
        void LogOpenProject();
        void LogCloseProject();
        void LogOpenDocumentation(string target);
        void LogOpenFaq(string target);
        void LogOpenDiscord(string target);
        void LogLoadAvatarFromDialog(string avatarUrl, bool eyeAnimation, bool voiceHandler);
        void LogUpdatePartnerURL(string previousSubdomain, string newSubdomain);
        void LogOpenDialog(string dialog);
        void LogBuildApplication(string target, string appName, bool productionBuild);
        void LogMetadataDownloaded(double duration);
        void LogAvatarLoaded(double duration);
        public void LogCheckForUpdates();
        public void LogSetLoggingEnabled(bool isLoggingEnabled);
        public void LogSetCachingEnabled(bool isCachingEnabled);
        public void LogClearLocalCache();
        public void LogViewPrivacyPolicy();
        public void LogShowInExplorer();
        public void LogFindOutMore(DocumentationContext context);
    }
}
