using ReadyPlayerMe.Samples;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    public static class ExampleSetup
    {
        private static readonly string RPM_WEBGL_SCREEN_SHOWN_KEY = "rpm-webgl-screen-shown";
        private static bool HasShownScreen = false;
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
            SetupWebGLTemplate.ShowWindow();
            EditorApplication.update -= ShowWebGLScreen;
        }
    }
}
