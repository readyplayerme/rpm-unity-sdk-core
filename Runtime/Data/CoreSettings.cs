using UnityEngine;

namespace ReadyPlayerMe.Core.Data
{
    public class CoreSettings : ScriptableObject
    {
        private const string RESOURCE_PATH = "Settings/CoreSettings";
        public const string DEFAULT_SUBDOMAIN = "demo";

        public string Subdomain = DEFAULT_SUBDOMAIN;
        public string AppId = string.Empty;
        public bool EnableLogging;
        public bool EnableAnalytics;

        public static CoreSettings Load()
        {
            return Resources.Load<CoreSettings>(RESOURCE_PATH);
        }
    }
}
