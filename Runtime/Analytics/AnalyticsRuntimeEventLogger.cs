using System.Collections.Generic;

namespace ReadyPlayerMe.Core
{
    internal class AnalyticsRuntimeEventLogger: IAnalyticsRuntimeLogger
    {
     
        public void LogRunQuickStartScene()
        {
            LogEvent("Personal Avatar Loaded");
        }
        
        public  void LogPersonalAvatarLoading()
        {
            LogEvent("Personal Avatar Loaded");
        }

        public void LogLoadPersonalAvatarButton()
        {
            LogEvent("Personal Avatar Loaded");
        }

        private void LogEvent(string eventName, Dictionary<string, object> eventProperties = null, Dictionary<string, object> userProperties = null)
        {
            if (!CoreSettingsHandler.CoreSettings.EnableAnalytics) return;
            AmplitudeEventLogger.LogEvent(eventName, eventProperties, userProperties);
        }
    }
}
