using System;
using UnityEngine;

namespace ReadyPlayerMe.Core
{

    [Serializable]
    public enum Target
    {
        Development,
        Production
    }

    [CreateAssetMenu(fileName = "Analytics Target", menuName = "Scriptable Objects/Ready Player Me/Analytics Target", order = 1)]
    public class AnalyticsTarget : ScriptableObject
    {
        public static readonly string LocalAssetPath = "Analytics Target";
        
        public Target Target;
        
        public static AnalyticsTarget GetAsset()
        {
#if DISABLE_AUTO_INSTALLER
            return Resources.Load<AnalyticsTarget>(LocalAssetPath);
#endif
            return CreateInstance<AnalyticsTarget>();
        }
    }
}
