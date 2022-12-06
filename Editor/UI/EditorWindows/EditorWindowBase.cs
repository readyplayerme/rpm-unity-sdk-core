using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public class EditorWindowBase : EditorWindow
    {
        private const string SUPPORT_HEADING = "Support";
        private const string ERROR_ICON_SEARCH_FILTER = "t:Texture rpm_error_icon";

        protected Texture errorIcon;
        protected GUIStyle HeadingStyle;
        protected GUIStyle descriptionStyle;

        private readonly GUILayoutOption windowWidth = GUILayout.Width(460);
        private Banner banner;
        private Footer footer;
        
        protected readonly float ButtonHeight = 30f;
        
        private bool windowResized;
        private string editorWindowName;
        
        private GUIStyle webButtonStyle;
        
        private void LoadAssets()
        {
            if (banner == null)
            {
                banner = new Banner(editorWindowName);
            }
            
            if (footer == null)
            {
                footer = new Footer(editorWindowName);
            }

            if (errorIcon == null)
            {
                var assetGuid = AssetDatabase.FindAssets(ERROR_ICON_SEARCH_FILTER).FirstOrDefault();
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

                if (assetPath != null)
                {
                    errorIcon = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)) as Texture;
                }
            }

            if (HeadingStyle == null)
            {
                HeadingStyle = new GUIStyle()
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
            }

            if (descriptionStyle == null)
            {
                descriptionStyle = new GUIStyle()
                {
                    fontSize = 12,
                    richText = true,
                    wordWrap = true,
                    margin = new RectOffset(5, 0, 0, 0)
                };
                descriptionStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            }

            if (webButtonStyle == null)
            {
                webButtonStyle = new GUIStyle(GUI.skin.button);
                webButtonStyle.fontSize = 12;
                webButtonStyle.fixedWidth = 149;
                webButtonStyle.fixedHeight = ButtonHeight;
                webButtonStyle.padding = new RectOffset(5, 5, 5, 5);
            }
        }

        protected void SetEditorWindowName(string editorName)
        {
            editorWindowName = editorName;
        }

        protected void DrawContent(Action getContent, bool useFooter = true)
        {
            LoadAssets();

            Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                Vertical(() =>
                {
                    banner.Draw(position);
                    
                    getContent?.Invoke();

                    if (useFooter)
                    {
                        Vertical(() =>
                        {
                            GUILayout.Label(SUPPORT_HEADING, HeadingStyle);
                            footer.Draw(position);
                        }, true);
                    }
                }, windowWidth);
                GUILayout.FlexibleSpace();
            });

            SetWindowSize();
        }

        private void SetWindowSize()
        {
            float height = GUILayoutUtility.GetLastRect().height;
            if (!windowResized && height > 1)
            {
                minSize = maxSize = new Vector2(460, height);
                windowResized = true;
            }
        }

        #region Horizontal and Vertical Layouts
        protected void Vertical(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginVertical(isBox ? "Box" : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        protected void Vertical(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        protected void Horizontal(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginHorizontal(isBox ? "Box" : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        protected void Horizontal(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
