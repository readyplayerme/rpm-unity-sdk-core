using System;
using UnityEditor;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class EntryPoint
    {
        private const string SESSION_STARTED_KEY = "SessionStarted";

        ///     Event for when package is imported or when project with package is opened.
        public static Action Startup;

        static EntryPoint()
        {
            EditorApplication.update += Update;
        }

        /// <summary>
        ///     This function is called on every <c>EditorApplication.update</c>.
        ///     It is used to trigger moduleUpdater to check for updates when the Unity Project launches.
        /// </summary>
        private static void Update()
        {
            if (SessionState.GetBool(SESSION_STARTED_KEY, false)) return;
            SessionState.SetBool(SESSION_STARTED_KEY, true);

            Startup?.Invoke();
            ModuleUpdater.CheckForUpdates();
            EditorApplication.update -= Update;
        }
    }
}
