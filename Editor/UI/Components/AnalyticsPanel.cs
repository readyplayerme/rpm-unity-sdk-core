using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace ReadyPlayerMe.Core.Editor
{
    public class AnalyticsPanel : IEditorWindowComponent
    {
        private const string HEADING = "Help us improve Ready Player Me SDK";
        private const string DESCRIPTION =
            "We are constantly adding new features and improvements to our SDK. Enable analytics and help us in building even better free tools for more developers. This data is used for internal purposes only and is not shared with third parties.";
        private const string ANALYTICS_PRIVACY_TEXT = "Read our Privacy Policy and learn how we use the data <b>here</b>.";
        private const string ANALYTICS_PRIVACY_URL =
            "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/help-us-improve-the-unity-sdk";
        
        private const string NEVER_ASK_AGAIN = "Never Ask Again";
        private const string DONT_ENABLE_ANALYTICS = "Don't Enable Analytics";
        private const string ENABLE_ANALYTICS = "Enable Analytics";
        private const int BUTTON_FONT_SIZE = 12;

        private static bool neverAskAgain;

        private readonly GUILayoutOption toggleWidth = GUILayout.Width(20);
        private GUIStyle buttonStyle;

        private bool variablesLoaded;

        private GUIStyle descriptionStyle;
        private GUIStyle HeadingStyle;
        public string EditorWindowName { get; set; }
        
        public UnityEvent OnButtonClick = new UnityEvent();
        
        public AnalyticsPanel(string editorWindowName)
        {
            EditorWindowName = editorWindowName;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadCachedVariables()
        {
            neverAskAgain = ProjectPrefs.GetBool(WelcomeWindow.NeverAskAgainPref);
            variablesLoaded = true;
        }

        /// <summary>
        ///     Sets the <c>EditorWindow</c> styles.
        /// </summary>
        private void LoadStyles()
        {
            HeadingStyle ??= new GUIStyle
            {
                fontSize = 14,
                richText = true,
                fontStyle = FontStyle.Bold,
                margin = new RectOffset(5, 0, 0, 8),
                normal =
                {
                    textColor = Color.white
                }
            };

            descriptionStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                richText = true,
                wordWrap = true,
                fixedWidth = 450,
                normal =
                {
                    textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f)
                }
            };

            buttonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = BUTTON_FONT_SIZE,
                padding = new RectOffset(5, 5, 5, 5),
                fixedHeight = 30,
                fixedWidth = 225
            };
        }


        public void Draw(Rect position = new Rect())
        {
            if (!variablesLoaded) LoadCachedVariables();
            LoadStyles();
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label(HEADING, HeadingStyle);

            GUILayout.Space(10);
            GUILayout.Label(DESCRIPTION, descriptionStyle);

            GUILayout.Space(10);
            if (GUILayout.Button(ANALYTICS_PRIVACY_TEXT, descriptionStyle))
            {
                Application.OpenURL(ANALYTICS_PRIVACY_URL);
            }

            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(4);
            neverAskAgain = EditorGUILayout.Toggle(neverAskAgain, toggleWidth);
            GUILayout.Label(NEVER_ASK_AGAIN);
            GUILayout.FlexibleSpace();

            ProjectPrefs.SetBool(WelcomeWindow.NeverAskAgainPref, neverAskAgain);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(3);
            if (GUILayout.Button(DONT_ENABLE_ANALYTICS, buttonStyle))
            {
                ProjectPrefs.SetBool(WelcomeWindow.NeverAskAgainPref, neverAskAgain);
                AnalyticsEditorLogger.Disable();
                OnButtonClick?.Invoke();
            }
            if (GUILayout.Button(ENABLE_ANALYTICS, buttonStyle))
            {
                ProjectPrefs.SetBool(WelcomeWindow.NeverAskAgainPref, true);
                AnalyticsEditorLogger.Enable();
                OnButtonClick?.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
