using UnityEditor;
using UnityEngine;
using ReadyPlayerMe.Core.Analytics;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     Footer for RPM editor windows. Contains buttons for external links.
    /// </summary>
    public class Footer : IEditorWindowComponent
    {
        private const string DOCS_URL = "https://bit.ly/UnitySDKDocs";
        private const string FAQ_URL = "https://docs.readyplayer.me/overview/frequently-asked-questions/game-engine-faq";
        private const string DISCORD_URL = "https://bit.ly/UnitySDKDiscord";

        private const float BUTTON_HEIGHT = 30f;

        private readonly GUIStyle webButtonStyle;

        private string editorWindowName;
        
        public Footer(string editorWindowName)
        {
            this.editorWindowName = editorWindowName;
            webButtonStyle = new GUIStyle(GUI.skin.button);
            webButtonStyle.fontSize = 12;
            webButtonStyle.fixedWidth = 149;
            webButtonStyle.fixedHeight = BUTTON_HEIGHT;
            webButtonStyle.padding = new RectOffset(5, 5, 5, 5);
        }

        public void Draw(Rect position = new Rect())
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Documentation", webButtonStyle))
            {
                AnalyticsEditorLogger.EventLogger.LogOpenDocumentation(editorWindowName);
                Application.OpenURL(DOCS_URL);
            }

            if (GUILayout.Button("FAQ", webButtonStyle))
            {
                AnalyticsEditorLogger.EventLogger.LogOpenFaq(editorWindowName);
                Application.OpenURL(FAQ_URL);
            }

            if (GUILayout.Button("Discord", webButtonStyle))
            {
                AnalyticsEditorLogger.EventLogger.LogOpenDiscord(editorWindowName);
                Application.OpenURL(DISCORD_URL);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}
