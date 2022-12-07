using System;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace ReadyPlayerMe.Core.Editor
{
    public static class EditorUtilities
    {
        private const string TAG = nameof(EditorUtilities);
        private const string SHORT_CODE_REGEX = "^[A-Z0-9]{6}$";

        // TODO: move into avatar loader
        public static void CreatePrefab(GameObject source, string path)
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(source, path, InteractionMode.AutomatedAction, out var success);
            PrefabUtility.ApplyObjectOverride(source, path, InteractionMode.AutomatedAction);

            SDKLogger.Log(TAG, success ? $"Prefab created successfully at path: {path}" : "Prefab creation failed");

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(source);
        }

        public static string TextFieldWithPlaceholder(string text, string placeholder, GUILayoutOption layoutOptions)
        {
            var newText = EditorGUILayout.TextField(text, layoutOptions);
            if (string.IsNullOrEmpty(text))
            {
                var previousColor = GUI.color;
                GUI.color = Color.grey;
                EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), placeholder);
                GUI.color = previousColor;
            }
            return newText;
        }

        public static bool IsUrlShortcodeValid(string shortcodeUrl)
        {
            return string.IsNullOrEmpty(shortcodeUrl) || Regex.Match(shortcodeUrl, SHORT_CODE_REGEX).Length > 0 || shortcodeUrl.EndsWith(".glb");
        }

        /// <summary>
        ///     Invokes a method and makes sure it is called only once during Unity editor session.
        ///     Called in static method that uses <see cref="InitializeOnLoadMethodAttribute"/>, session lock is
        ///     used for making sure it runs only once and does not invoke on file reimport or editor mode switch.
        /// </summary>
        /// <param name="key">Unique key.</param>
        /// <param name="action">Action to invoke.</param>
        public static void InvokeOnLoad(string key, Action action)
        {
            if (!SessionState.GetBool(key, false))
            {
                SessionState.SetBool(key, true);
                action?.Invoke();
            }
        }
    }
}
