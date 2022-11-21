using System;
using UnityEditor;

namespace ReadyPlayerMe.Core
{
    public enum Target
    {
        Production,
        Development
    }

    public static class AnalyticsTarget
    {
        public static Target GetTarget()
        {
            var path = AssetDatabase.FindAssets ( $"t:Script {nameof(AnalyticsTarget)}" );
            var directoryPath = AssetDatabase.GUIDToAssetPath(path[0]);
            return directoryPath.Contains("com.readyplayerme.core") ? Target.Production : Target.Development;
        }
    }
}
