using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    /// <summary>
    ///     Used for logging in SDK. Currently logs to unity's debugger.
    ///     <remarks>
    ///         Logging is enabled by default, if required can be disabled.
    ///         <code> SDKLogging.logEnabled = false; </code>
    ///     </remarks>
    /// </summary>
    public static class SDKLogger
    {
        public static readonly Logger AvatarLoaderLogger = new Logger(new CustomLogHandler());

        private static readonly string LoggingEnabledPref = $"rpm-sdk-logging-enabled-{Application.dataPath.GetHashCode()}";
  
        private static bool loggingEnabled;

        static SDKLogger()
        {
            loggingEnabled = GetEnabledPref();
        }

        public static bool GetEnabledPref()
        {
            return EditorPrefs.GetBool(LoggingEnabledPref, false);
        }

        public static void SetEnabledPref(bool enabled)
        {
            loggingEnabled = enabled;
            EditorPrefs.SetBool(LoggingEnabledPref, loggingEnabled);
        }
        
        public static void Log(string tag, object message)
        {
            if (loggingEnabled)
            {
                AvatarLoaderLogger.Log(tag, message);
            }
        }

        public static void LogWarning(string tag, object message)
        {
            if (loggingEnabled)
            {
                AvatarLoaderLogger.LogWarning(tag, message);
            }
        }
    }
}
