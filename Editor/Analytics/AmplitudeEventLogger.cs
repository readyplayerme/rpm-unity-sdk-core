using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core.Analytics
{
    public class AmplitudeEventLogger
    {
        private const string ENDPOINT = "https://analytics-sdk.readyplayer.me/";

        private readonly AppData appData;
        private const string NO_INTERNET_CONNECTION = "No internet connection.";
        private bool HasInternetConnection => Application.internetReachability != NetworkReachability.NotReachable;

        private readonly Target target;

        private long sessionId;

        public AmplitudeEventLogger()
        {
            appData = ApplicationData.GetData();
            target = AnalyticsTarget.GetTarget();
        }

        public void SetSessionId(long id)
        {
            sessionId = id;
        }

        public bool IsSessionIdSet()
        {
            return sessionId != 0;
        }

        public void SetUserProperties()
        {
            var userProperties = new Dictionary<string, object>
            {
                { Constants.Properties.ENGINE_VERSION, appData.UnityVersion },
                { Constants.Properties.RENDER_PIPELINE, appData.RenderPipeline },
                { Constants.Properties.SUBDOMAIN, appData.PartnerName },
                { Constants.Properties.APP_NAME, PlayerSettings.productName },
                { Constants.Properties.SDK_TARGET, "Unity" },
                { Constants.Properties.APP_IDENTIFIER, Application.identifier },
                { Constants.Properties.ALLOW_ANALYTICS, true }
            };

            LogEvent(Constants.EventName.SET_USER_PROPERTIES, null, userProperties);
        }


        public async void LogEvent(string eventName, Dictionary<string, object> eventProperties = null, Dictionary<string, object> userProperties = null)
        {
            var eventData = new Dictionary<string, object>
            {
                { Constants.AmplitudeKeys.DEVICE_ID, SystemInfo.deviceUniqueIdentifier },
                { Constants.AmplitudeKeys.EVENT_TYPE, eventName },
                { Constants.AmplitudeKeys.PLATFORM, appData.UnityPlatform },
                { Constants.AmplitudeKeys.SESSION_ID, sessionId },
                { Constants.AmplitudeKeys.APP_VERSION, appData.SDKVersion },
                { Constants.AmplitudeKeys.OPERATING_SYSTEM, SystemInfo.operatingSystem }
            };

            if (userProperties != null)
            {
                eventData.Add(Constants.AmplitudeKeys.USER_PROPERTIES, userProperties);
            }

            if (eventProperties != null)
            {
                eventData.Add(Constants.AmplitudeKeys.EVENT_PROPERTIES, eventProperties);
            }

            var payload = new
            {
                target = GetAnalyticsTarget(),
                events = new[]
                {
                    eventData
                }
            };

            var json = JsonConvert.SerializeObject(payload);
            var bytes = Encoding.UTF8.GetBytes(json);

            try
            {
                await Dispatch(ENDPOINT, bytes);
            }
            catch (Exception exception)
            {
                SDKLogger.Log(nameof(AmplitudeEventLogger), exception);
            }
        }
        
        private async Task Dispatch(string url, byte[] bytes)
        {
            if (HasInternetConnection)
            {
                using (var request = UnityWebRequest.Put(url, bytes))
                {
                    request.method = "POST";
                    request.SetRequestHeader("Content-Type", "application/json");

                    var asyncOperation = request.SendWebRequest();
                    while (!asyncOperation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.isHttpError || request.isNetworkError)
                    {
                        throw new CustomException(FailureType.DownloadError, request.error);
                    }
                    return;
                }
            }

            throw new CustomException(FailureType.NoInternetConnection, NO_INTERNET_CONNECTION);
        }

        #region Analytics Target

        private const string PRODUCTION = "unity";
        private const string DEVELOPMENT = "unity-dev";

        private string GetAnalyticsTarget()
        {
            switch (target)
            {
                case Target.Development:
                    return DEVELOPMENT;
                case Target.Production:
                    return PRODUCTION;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
