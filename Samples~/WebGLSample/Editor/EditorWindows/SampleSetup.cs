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
        private const string PACKAGE_PATH = "Packages/com.readyplayerme.core/Samples~/WebGLSample/WebGLSample.unitypackage";
        private const string ASSETS_PACKAGE_PATH = "Assets/Ready Player Me/Core/Samples/WebGLSample/WebGLSample.unitypackage";
        
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (ProjectPrefs.GetBool(RPM_WEBGL_SCREEN_SHOWN_KEY))
            {
                return;
            }
            
            EditorApplication.update += ShowWebGLScreen;
        }

        private static void ShowWebGLScreen()
        {
            ProjectPrefs.SetBool(RPM_WEBGL_SCREEN_SHOWN_KEY, true);
            var shouldUpdate = EditorUtility.DisplayDialogComplex(WINDOW_TITLE,
                DESCRIPTION,
                CONFIRM_BUTTON_TEXT,
                CANCEL_BUTTON_TEXT,
                "");

            switch (shouldUpdate)
            {
                case 0:
                    OnConfirm();
                    break;
            }
            EditorApplication.update -= ShowWebGLScreen;
        }

        private static void OnConfirm()
        {
            ImportPackage();
            SetWebGLTemplate();
        }

        private static void ImportPackage()
        {
            var path = PACKAGE_PATH;
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if(asset == null)
            {
                path = ASSETS_PACKAGE_PATH;
                asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            }
            if (asset == null)
            {
                Debug.LogWarning("Failed to find WebGLSample. No changes were done to project");
                return;
            }
            AssetDatabase.ImportPackage(path, true);
        }
        
        private static void SetWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:RPMTemplate";
            SDKLogger.Log(TAG, "Updated player settings to use RPMTemplate");
        }
    }
}
