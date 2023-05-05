using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     This <c>EditorWindow</c> class is used as a base class for Ready Player Me Editor windows and contains
    ///     functionality that is shared between each child class.
    /// </summary>
    public class EditorWindowBase : EditorWindow
    {
        private const string SUPPORT_HEADING = "Support";
        private const string ERROR_ICON_SEARCH_FILTER = "t:Texture rpm_error_icon";
        private const float WIDTH = 460;
        private readonly GUILayoutOption windowWidth = GUILayout.Width(WIDTH);
        private readonly Color descriptionColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);

        protected readonly float ButtonHeight = 30f;
        
        protected GUIStyle HeadingStyle;
        protected GUIStyle DescriptionStyle;

        protected Texture ErrorIcon;

        private GUIStyle webButtonStyle;

        private Banner banner;
        private Footer footer;

        private string editorWindowName;
        private bool windowResized;

        private void LoadAssets()
        {
            banner ??= new Banner();

            footer ??= new Footer(editorWindowName);

            if (ErrorIcon == null)
            {
                var assetGuid = AssetDatabase.FindAssets(ERROR_ICON_SEARCH_FILTER).FirstOrDefault();
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

                if (assetPath != null)
                {
                    ErrorIcon = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)) as Texture;
                }
            }

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

            DescriptionStyle ??= new GUIStyle
            {
                fontSize = 12,
                richText = true,
                wordWrap = true,
                margin = new RectOffset(5, 0, 0, 0),
                normal =
                {
                    textColor = descriptionColor
                }
            };

            webButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fixedWidth = 149,
                fixedHeight = ButtonHeight,
                padding = new RectOffset(5, 5, 5, 5)
            };
        }

        protected void SetEditorWindowName(string editorName)
        {
            editorWindowName = editorName;
        }

        protected void DrawContent(Action content, bool useFooter = true)
        {
            LoadAssets();

            Layout.Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                Layout.Vertical(() =>
                {
                    banner.Draw(position);
                    content?.Invoke();
                    if (useFooter)
                    {
                        Layout.Vertical(() =>
                        {
                            GUILayout.Label(SUPPORT_HEADING, HeadingStyle);
                            footer.Draw();
                        }, true);
                    }
                }, false, windowWidth);
                GUILayout.FlexibleSpace();
            });

            SetWindowSize();
        }

        private void SetWindowSize()
        {
            var height = GUILayoutUtility.GetLastRect().height;
            if (!windowResized && height > 1)
            {
                minSize = maxSize = new Vector2(WIDTH, height);
                windowResized = true;
            }
        }
    }
}
