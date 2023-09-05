using ReadyPlayerMe.Samples;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    public static class ExampleSetup
    {
        private static readonly string RPM_WEBGL_SCREEN_SHOWN_KEY = "rpm-webgl-screen-shown";

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (!ProjectPrefs.GetBool(RPM_WEBGL_SCREEN_SHOWN_KEY))
            {
                ProjectPrefs.SetBool(RPM_WEBGL_SCREEN_SHOWN_KEY, true);
                SetupWebGLTemplate.ShowWindow();
            }
        }
    }
}
