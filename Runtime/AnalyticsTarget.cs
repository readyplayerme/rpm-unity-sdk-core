using System;
using UnityEngine;

namespace ReadyPlayerMe.Core
{

    [Serializable]
    public enum Target
    {
        Production,
        Development
    }

    [CreateAssetMenu(fileName = "Analytics Target", menuName = "Scriptable Objects/Ready Player Me/Analytics Target", order = 1)]
    public class AnalyticsTarget : ScriptableObject
    {
        public static readonly string LocalAssetPath = "Analytics Target";

        public Target Target;

        public static AnalyticsTarget GetAsset()
        {
            return Resources.Load<AnalyticsTarget>(LocalAssetPath);
        }
    }
}
