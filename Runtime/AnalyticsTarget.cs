using System;
using UnityEditor;
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
        public static readonly string LocalAssetPath = "Assets/Ready Player Me/Core/Resources/Analytics Target.asset";
        
        public Target Target;
        
        public static AnalyticsTarget GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath<AnalyticsTarget>(LocalAssetPath);
        }
    }
}
