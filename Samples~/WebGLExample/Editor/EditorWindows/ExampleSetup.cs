using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Samples;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    public static class ExampleSetup
    {
        private const string TAG = nameof(ExampleSetup);
        private static string WINDOW_TITLE = "RPM WebGL Example";
        private static string DESCRIPTION =
            "This example includes a WebGL template that can be used for WebGL builds. To use the template it needs to be moved inside WebGLTemplates folder and set in the player settings. Would you like to move it automatically?";
        private const string CONFIRM_BUTTON_TEXT = "Ok";
        private const string CANCEL_BUTTON_TEXT = "Cancel";

        private static readonly string RPM_WEBGL_SCREEN_SHOWN_KEY = "rpm-webgl-screen-shown";

        private static readonly string TEMPLATE_PATH = "/WebGLTemplates/RPMTemplate";
        private static readonly string FILE_NAME = "ExampleSetup.cs";
        private static readonly string ROOT_PATH = "/Assets";

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
            var templatePaths = GetTemplatePaths();

            if (templatePaths == null)
            {
                Debug.LogWarning("Failed to set source and destination paths. No changes were done to project");
                return;
            }
            Copy(templatePaths[0], templatePaths[1]);
            SetWebGLTemplate();
        }

        private static List<string> GetTemplatePaths()
        {
            string[] res = Directory.GetFiles(Application.dataPath, FILE_NAME, SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                return null;
            }
            string path = res[0].Replace(FILE_NAME, "").Replace("\\", "/");
            var sourcePath = path.Substring(0, path.IndexOf("/Editor/")) + TEMPLATE_PATH;
            var destinationPath = path.Substring(0, path.IndexOf(ROOT_PATH)) + ROOT_PATH;
            return new List<string>() { sourcePath, destinationPath };
        }

        private static void Copy(string sourcePath, string destinationPath)
        {
            foreach (string sourceFile in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (sourceFile.EndsWith(".meta"))
                {
                    continue;
                }

                var sourceFilePath = sourceFile.Replace("\\", "/");

                if (File.Exists(sourceFilePath))
                {
                    string destination = destinationPath + sourceFilePath.Substring(sourceFilePath.IndexOf(TEMPLATE_PATH)).Replace("\\", "/");

                    if (!Directory.Exists(destination.Substring(0, destination.LastIndexOf("/"))))
                    {
                        Directory.CreateDirectory(destination.Substring(0, destination.LastIndexOf("/")));
                    }

                    File.Copy(sourceFilePath, destination, true);
                }
                else
                {
                    Debug.LogError("Source file does not exist: " + sourceFilePath);
                }
            }
            Debug.Log("Copied RPMTemplate to the WebGLTemplate folder in the root path of Assets");
            AssetDatabase.Refresh();
        }


        private static void SetWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:RPMTemplate";
            Debug.Log("Updated player settings to use RPMTemplate");
        }
    }
}
