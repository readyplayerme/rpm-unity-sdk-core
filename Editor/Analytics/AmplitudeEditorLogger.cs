using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Analytics
{
    public class AmplitudeEditorLogger : IAnalyticsEditorLogger
    {
        private const string SDK_TARGET = "Unity";

        private readonly Dictionary<HelpSubject, string> helpDataMap = new Dictionary<HelpSubject, string>
        {
            { HelpSubject.AvatarCaching, "avatar caching" },
            { HelpSubject.Subdomain, "subdomain" },
            { HelpSubject.AvatarConfig, "avatar config" },
            { HelpSubject.GltfDeferAgent, "gltf defer agent" },
            { HelpSubject.LoadingAvatars, "download avatar into scene" }
        };

        private bool isEnabled;
        private readonly AppData appData;

        public AmplitudeEditorLogger(bool isEnabled)
        {
            this.isEnabled = isEnabled;
            appData = ApplicationData.GetData();
        }

        public void Enable()
        {
            isEnabled = true;
            if (!AmplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            ToggleAnalytics(true);
        }

        public void Disable()
        {
            ToggleAnalytics(false);
            isEnabled = false;
            AmplitudeEventLogger.SetSessionId(0);
        }

        public void IdentifyUser()
        {
            if (!isEnabled) return;
            if (!AmplitudeEventLogger.IsSessionIdSet())
            {
                GenerateSessionId();
            }
            SetUserProperties();
        }

        public void LogOpenProject()
        {
            if (!isEnabled) return;
            GenerateSessionId();
            AmplitudeEventLogger.LogEvent(Constants.EventName.OPEN_PROJECT);
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

        public void LogCheckForUpdates()
        {
            LogEvent(Constants.EventName.CHECK_FOR_UPDATES);
        }

        public void LogSetLoggingEnabled(bool isLoggingEnabled)
        {
            LogEvent(Constants.EventName.SET_LOGGING_ENABLED, new Dictionary<string, object>
            {
                { Constants.Properties.LOGGING_ENABLED, isLoggingEnabled }
            });
        }

        public void LogSetCachingEnabled(bool isCachingEnabled)
        {
            LogEvent(Constants.EventName.SET_CACHING_ENABLED, new Dictionary<string, object>
            {
                { Constants.Properties.CACHING_ENABLED, isCachingEnabled }
            });
        }

        public void LogClearLocalCache()
        {
            LogEvent(Constants.EventName.CLEAR_LOCAL_CACHE);
        }

        public void LogViewPrivacyPolicy()
        {
            LogEvent(Constants.EventName.PRIVACY_POLICY);
        }

        public void LogShowInExplorer()
        {
            LogEvent(Constants.EventName.SHOW_IN_EXPLORER);
        }

        public void LogFindOutMore(HelpSubject subject)
        {
            LogEvent(Constants.EventName.FIND_OUT_MORE, new Dictionary<string, object>
            {
                { Constants.Properties.CONTEXT, helpDataMap[subject] }
            });
        }

        public void LogOpenSetupGuide()
        {
            LogEvent(Constants.EventName.OPEN_SETUP_GUIDE);
        }

        public void LogOpenIntegrationGuide()
        {
            LogEvent(Constants.EventName.OPEN_INTEGRATION_GUIDE);
        }

        public void LogLoadQuickStartScene()
        {
            LogEvent(Constants.EventName.LOAD_QUICK_START_SCENE);
        }

        public void LogOpenAvatarDocumentation()
        {
            LogEvent(Constants.EventName.OPEN_AVATAR_DOCUMENTATION);
        }

        public void LogOpenAnimationDocumentation()
        {
            LogEvent(Constants.EventName.OPEN_ANIMATION_DOCUMENTATION);
        }

        public void LogOpenAvatarCreatorDocumentation()
        {
            LogEvent(Constants.EventName.OPEN_AVATAR_CREATOR_DOCUMENTATION);
        }

        public void LogOpenOptimizationDocumentation()
        {
            LogEvent(Constants.EventName.OPEN_OPTIMIZATION_DOCUMENTATION);
        }

        private void SetUserProperties()
        {
            var userProperties = new Dictionary<string, object>
            {
                { Constants.Properties.ENGINE_VERSION, appData.UnityVersion },
                { Constants.Properties.RENDER_PIPELINE, appData.RenderPipeline },
                { Constants.Properties.SUBDOMAIN, appData.PartnerName },
                { Constants.Properties.APP_NAME, PlayerSettings.productName },
                { Constants.Properties.SDK_TARGET, SDK_TARGET },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier },
                { Constants.Properties.ALLOW_ANALYTICS, true }
            };

            Dictionary<string, string> modules = ModuleList.GetInstalledModuleVersionDictionary();

            foreach (KeyValuePair<string, string> module in modules)
            {
                userProperties.Add(module.Key, module.Value);
            }

            LogEvent(Constants.EventName.SET_USER_PROPERTIES, null, userProperties);
        }

        private void GenerateSessionId()
        {
            AmplitudeEventLogger.SetSessionId(DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        private void ToggleAnalytics(bool allow)
        {
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
        
        private void LogEvent(string eventName, Dictionary<string, object> eventProperties = null, Dictionary<string, object> userProperties = null)
        {
            if (!isEnabled) return;
            AmplitudeEventLogger.LogEvent(eventName, eventProperties, userProperties);
        }
    }
}
