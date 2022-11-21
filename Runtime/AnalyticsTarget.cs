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
    public static class AnalyticsTarget
    {
        public static readonly string LocalAssetPath = "Analytics Target";

        public static Target GetTarget()
        {
            string path = typeof(AnalyticsTarget).Assembly.Location;
            Debug.Log(typeof(AnalyticsTarget).Assembly.Location);
            if (path.Contains("com.readyplayerme.core"))
            {
                Debug.Log("Inside package");
                return Target.Production;
            }

            Debug.Log("In project");
            return Target.Development;
        }
    }
}
