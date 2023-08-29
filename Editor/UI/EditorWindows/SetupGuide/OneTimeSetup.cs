using ReadyPlayerMe.Core.Analytics;
using UnityEditor;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class OneTimeSetup
    {
        static OneTimeSetup()
        {
            EntryPoint.Startup += OnStartup;
        }

        private static void OnStartup()
        {
            EntryPoint.Startup -= OnStartup;

            if (CanShowWindow())
            {
                AnalyticsEditorLogger.Enable();
                SetupGuide.ShowWindow();
                ProjectPrefs.SetBool(ProjectPrefs.FIRST_TIME_SETUP_DONE, true);
            }

            if (AnalyticsEditorLogger.IsEnabled)
            {
                AnalyticsEditorLogger.EventLogger.LogOpenProject();
                AnalyticsEditorLogger.EventLogger.IdentifyUser();
                EditorApplication.quitting += OnQuit;
            }
        }

        private static void OnQuit()
        {
            AnalyticsEditorLogger.EventLogger.LogCloseProject();
        }

        private static bool CanShowWindow()
        {
            return !ProjectPrefs.GetBool(ProjectPrefs.FIRST_TIME_SETUP_DONE);
        }
    }
}
