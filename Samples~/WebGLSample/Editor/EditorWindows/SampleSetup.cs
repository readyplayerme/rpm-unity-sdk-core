using System;
using System.Collections.Generic;
using System.IO;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    public static class SampleSetup
    {
        private const string TAG = nameof(SampleSetup);
        private const string WINDOW_TITLE = "RPM WebGL Sample";
        private const string DESCRIPTION =
            "This sample includes a WebGL template and a WebGLHelper library that is used for WebGL builds. For these to work they need to be moved into specific directories. Would you like to move them automatically now?";
        private const string CONFIRM_BUTTON_TEXT = "Ok";
        private const string CANCEL_BUTTON_TEXT = "Cancel";

        private const string RPM_WEBGL_SCREEN_SHOWN_KEY = "rpm-webgl-screen-shown";
        private const string TEMPLATE_PATH = "WebGLTemplates";
        private const string FILE_NAME = "ReadyPlayerMe.Core.WebGLSample.asmdef";
        private const string PLUGINS_FOLDER = "Plugins";
        private const string WEBGL_HELPER_PATH = "WebGlHelper";

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (ProjectPrefs.GetBool(RPM_WEBGL_SCREEN_SHOWN_KEY))
            {
                return;
            }

            ProjectPrefs.SetBool(RPM_WEBGL_SCREEN_SHOWN_KEY, true);
            EditorApplication.update += ShowWebGLScreen;
        }

        private static void ShowWebGLScreen()
        {
            var shouldUpdate = EditorUtility.DisplayDialogComplex(WINDOW_TITLE,
                DESCRIPTION,
                CONFIRM_BUTTON_TEXT,
                CANCEL_BUTTON_TEXT,
                "");

            switch (shouldUpdate)
            {
                // Update
                case 0:
                    OnConfirm();
                    break;
            }
            EditorApplication.update -= ShowWebGLScreen;
        }

        private static void OnConfirm()
        {
            var samplesRootFolder = GetSampleRootFolder();
            if (string.IsNullOrEmpty(samplesRootFolder))
            {
                Debug.LogWarning("Failed to find WebGLSample. No changes were done to project");
                return;
            }
            MoveFolder($"{samplesRootFolder}/{TEMPLATE_PATH}", $"{Application.dataPath}");
            MoveFolder($"{samplesRootFolder}/{WEBGL_HELPER_PATH}", $"{Application.dataPath}/{PLUGINS_FOLDER}");
            SetWebGLTemplate();
        }

        private static string GetSampleRootFolder()
        {
            var results = Directory.GetFiles(Application.dataPath, FILE_NAME, SearchOption.AllDirectories);
            if (results.Length == 0)
            {
                return String.Empty;
            }
            var rootSamplePath = results[0].Replace(FILE_NAME, "").Replace("\\", "/");
            return rootSamplePath.TrimEnd('/');
        }

        private static void MoveFolder(string sourcePath, string destinationPath)
        {
            // Extract the last part of the source path (e.g., "Plugin")
            var sourceDirectoryName = new DirectoryInfo(sourcePath).Name;

            // Append the source directory name to the destination path
            var newDestinationPath = Path.Combine(destinationPath, sourceDirectoryName);

            // Check if the source directory exists
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourcePath);
            }

            // If the destination directory doesn't exist, create it
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Move the entire source directory to the new destination
            Directory.Move(sourcePath, newDestinationPath);
            SDKLogger.Log(TAG, $"Moved folder and contents from {sourcePath} to {newDestinationPath}");
            AssetDatabase.Refresh();
        }

        private static void SetWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:RPMTemplate";
            SDKLogger.Log(TAG, "Updated player settings to use RPMTemplate");
        }
    }
}
