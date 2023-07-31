using UnityEngine;

namespace ReadyPlayerMe.Core.Data
{
    public class CoreSettings : ScriptableObject
    {
        public string Subdomain = DEFAULT_SUBDOMAIN;
        public bool EnableLogging;
        public const string DEFAULT_SUBDOMAIN = "demo";
    }
}
