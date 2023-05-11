﻿using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public static class DocumentationButton
    {
        private static readonly GUIStyle Style = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            fixedHeight = 18,
            fixedWidth = 18,
            margin = new RectOffset(2, 0, 0, 8),
            normal =
            {
                textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f)
            }
        };

        public static void Draw(string url)
        {
            if (GUILayout.Button("?", Style))
            {
                Application.OpenURL(url);
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
        }
    }
}
