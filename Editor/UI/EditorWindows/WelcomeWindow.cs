using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public class WelcomeWindow : EditorWindowBase
    {
        private Banner banner;
        private GUIStyle buttonStyle;
        private GUIStyle descriptionStyle;
        private GUIStyle headingStyle;
        private AnalyticsPanel analyticsPanel;
        private QuickStartPanel quickStartPanel;
        private bool displayQuickStart;
        
        private const string METRICS_NEVER_ASK_AGAIN = "rpm-sdk-metrics-never-ask-again";

        /// <summary>
        ///     Constructor method that subscribes to the StartUp event.
        /// </summary>
        static WelcomeWindow()
        {
            EntryPoint.Startup += OnStartup;
        }

        /// <summary>
        ///     This method is called when Unity Editor is closed or this package is removed.
        /// </summary>
        private void OnDestroy()
        {
            EntryPoint.Startup -= OnStartup;
        }
        
        /// <summary>
        ///     This method is called when a Unity project is opened or after this Unity package has finished importing and is
        ///     responsible for displaying the window. It also calls anayltics events if enabled.
        /// </summary>
        private static void OnStartup()
        {
            if (!EditorPrefs.GetBool(METRICS_NEVER_ASK_AGAIN) && !AnalyticsEditorLogger.IsEnabled)
            {
                ShowWindow();
            }

            if (AnalyticsEditorLogger.IsEnabled)
            {
                AnalyticsEditorLogger.EventLogger.LogOpenProject();
                AnalyticsEditorLogger.EventLogger.IdentifyUser();
                EditorApplication.quitting += OnQuit;
            }
        }

        /// <summary>
        ///     This method is called when the Unity Editor is closed and logs the close event.
        /// </summary>
        private static void OnQuit()
        {
            AnalyticsEditorLogger.EventLogger.LogCloseProject();
        }

        [MenuItem("Ready Player Me/Welcome")]
        public static void ShowWindow()
        {
            GetWindow(typeof(WelcomeWindow), false, "Welcome");
        }

        private void LoadAssets()
        {
            if (analyticsPanel == null)
            {
                analyticsPanel = new AnalyticsPanel("Welcome");
                analyticsPanel.OnButtonClick.AddListener(() =>
                {
                    displayQuickStart = true;
                    minSize = maxSize = new Vector2(460, 248);
                });
            }
            banner ??= new Banner("Banner");
            if (quickStartPanel == null)
            {
                quickStartPanel = new QuickStartPanel();
                quickStartPanel.OnQuickStartClick.AddListener(() =>
                {
                    Close();
                });                
                quickStartPanel.OnCloseClick.AddListener(() =>
                {
                    Close();
                });
            }
        }

        private void OnGUI()
        {
            LoadAssets();
            DrawContent(DrawContent, false);
        }

        private void DrawContent()
        {
            Vertical(() =>
            {
                if (displayQuickStart)
                {
                    quickStartPanel.Draw(position);
                }
                else
                {
                    analyticsPanel.Draw(position);
                }
            });
        }
    }
}
