﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Analytics
{
    public class AnalyticsEventLogger : IAnalyticsEventLogger
    {
        private readonly AmplitudeEventLogger amplitudeEventLogger;

        private bool isEnabled;

        public AnalyticsEventLogger(bool isEnabled)
        {
            amplitudeEventLogger = new AmplitudeEventLogger();
            this.isEnabled = isEnabled;
        }

        public void Enable()
        {
            isEnabled = true;
            if (!amplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            ToggleAnalytics(true);
        }

        public void Disable()
        {
            ToggleAnalytics(false);
            isEnabled = false;
            amplitudeEventLogger.SetSessionId(0);
        }

        public void IdentifyUser()
        {
            if (!isEnabled) return;
            if (!amplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            amplitudeEventLogger.SetUserProperties();
        }

        public void LogOpenProject()
        {
            if (!isEnabled) return;
            GenerateSessionId();
            amplitudeEventLogger.LogEvent(Constants.EventName.OPEN_PROJECT);
        }

        public void LogCloseProject()
        {
            LogEvent(Constants.EventName.CLOSE_PROJECT);
        }

        public void LogOpenDocumentation(string target)
        {
            LogEvent(Constants.EventName.OPEN_DOCUMENTATION, new Dictionary<string, object>
            {
                { Constants.Properties.TARGET, target }
            });
        }

        public void LogOpenFaq(string target)
        {
            LogEvent(Constants.EventName.OPEN_FAQ, new Dictionary<string, object>
            {
                { Constants.Properties.TARGET, target }
            });
        }

        public void LogCheckForUpdates()
        {
            LogEvent(Constants.EventName.CHECK_FOR_UPDATES);
        }

        public void LogToggleLogging(bool isLoggingEnabled)
        {
            LogEvent(Constants.EventName.TOGGLE_LOGGING, new Dictionary<string, object>
            {
                { Constants.Properties.LOGGING_ENABLED, isLoggingEnabled }
            });
        }

        public void LogToggleCaching(bool isCachingEnabled)
        {
            LogEvent(Constants.EventName.TOGGLE_CACHING, new Dictionary<string, object>
            {
                { Constants.Properties.CACHING_ENABLED, isCachingEnabled }
            });
        }

        public void LogClearLocalCache()
        {
            LogEvent(Constants.EventName.CLEAR_LOCAL_CACHE);
        }

        public void LogPrivacyPolicy()
        {
            LogEvent(Constants.EventName.PRIVACY_POLICY);
        }

        public void LogShowInExplorer()
        {
            LogEvent(Constants.EventName.SHOW_IN_EXPLORER);
        }

        public void LogHelpButton(string context)
        {
            LogEvent(Constants.EventName.HELP_BUTTON, new Dictionary<string, object>
            {
                { Constants.Properties.CONTEXT, context }
            });
        }

        public void LogOpenDiscord(string target)
        {
            LogEvent(Constants.EventName.OPEN_DISCORD, new Dictionary<string, object>
            {
                { Constants.Properties.TARGET, target }
            });
        }

        public void LogLoadAvatarFromDialog(string avatarUrl, bool eyeAnimation, bool voiceHandler)
        {
            LogEvent(Constants.EventName.LOAD_AVATAR_FROM_DIALOG, new Dictionary<string, object>
            {
                { Constants.Properties.AVATAR_URL, avatarUrl },
                { Constants.Properties.EYE_ANIMATION, eyeAnimation },
                { Constants.Properties.VOICE_HANDLER, voiceHandler }
            });
        }

        public void LogUpdatePartnerURL(string previousSubdomain, string newSubdomain)
        {
            LogEvent(Constants.EventName.UPDATED_PARTNER_URL, new Dictionary<string, object>
            {
                { Constants.Properties.PREVIOUS_SUBDOMAIN, previousSubdomain },
                { Constants.Properties.NEW_SUBDOMAIN, newSubdomain }
            }, new Dictionary<string, object>
            {
                { Constants.Properties.SUBDOMAIN, newSubdomain }
            });
        }

        public void LogOpenDialog(string dialog)
        {
            LogEvent(Constants.EventName.OPEN_DIALOG, new Dictionary<string, object>
            {
                { Constants.Properties.DIALOG, dialog }
            });
        }

        private void LogEvent(string eventName, Dictionary<string, object> eventProperties = null, Dictionary<string, object> userProperties = null)
        {
            if (!isEnabled) return;
            amplitudeEventLogger.LogEvent(eventName, eventProperties, userProperties);
        }

        public void LogBuildApplication(string target, string appName, bool productionBuild)
        {
            LogEvent(Constants.EventName.BUILD_APPLICATION, new Dictionary<string, object>
            {
                { Constants.Properties.TARGET, target },
                { Constants.Properties.APP_NAME, appName },
                { Constants.Properties.PRODUCTION_BUILD, productionBuild },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier }
            });
        }

        public void LogMetadataDownloaded(double duration)
        {
            LogEvent(Constants.EventName.METADATA_DOWNLOADED, new Dictionary<string, object>
            {
                { Constants.Properties.DURATION, duration }
            });
        }

        public void LogAvatarLoaded(double duration)
        {
            LogEvent(Constants.EventName.AVATAR_LOADED, new Dictionary<string, object>
            {
                { Constants.Properties.DURATION, duration }
            });
        }

        private void GenerateSessionId()
        {
            amplitudeEventLogger.SetSessionId(DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        private void ToggleAnalytics(bool allow)
        {
            AppData appData = ApplicationData.GetData();
            LogEvent(Constants.EventName.ALLOW_ANALYTICS, new Dictionary<string, object>
            {
                { Constants.Properties.ALLOW, allow }
            }, new Dictionary<string, object>
            {
                { Constants.Properties.ENGINE_VERSION, appData.UnityVersion },
                { Constants.Properties.RENDER_PIPELINE, appData.RenderPipeline },
                { Constants.Properties.SUBDOMAIN, appData.PartnerName },
                { Constants.Properties.APP_NAME, PlayerSettings.productName },
                { Constants.Properties.SDK_TARGET, "Unity" },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier },
                { Constants.Properties.ALLOW_ANALYTICS, allow }
            });
        }
    }
}
