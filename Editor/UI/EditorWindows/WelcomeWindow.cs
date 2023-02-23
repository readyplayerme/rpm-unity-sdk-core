using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public class WelcomeWindow : EditorWindowBase
    {
        private Banner banner;
        private GUIStyle buttonStyle;
        private GUIStyle descriptionStyle;
        private GUIStyle headingStyle;
        private AnalyticsPanel analyticsPanel;
        private QuickStartPanel quickStartPanel;
        private bool displayQuickStart;

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
                    //TODO add event for opening quick start sample
                    Close();
                });                
                quickStartPanel.OnCloseClick.AddListener(() =>
                {
                    //TODO add event for NOT opening quick start sample
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
