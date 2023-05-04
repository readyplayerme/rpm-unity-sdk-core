using System;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public static class Layout
    {
        public static void Vertical(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginVertical(isBox ? GUI.skin.box : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void Vertical(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void Horizontal(Action content, bool isBox = false, float width = 0f, float height = 0f)
        {
            EditorGUILayout.BeginHorizontal(isBox ? GUI.skin.box : GUIStyle.none, GUILayout.Height(height), GUILayout.Width(width));
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void Horizontal(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }
    }
}
