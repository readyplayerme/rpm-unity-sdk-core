using System.Collections.Generic;

namespace ReadyPlayerMe
{
    public static class Core
    {
        public static readonly Dictionary<string, string> PackageMap =
            new Dictionary<string, string>
            {
                { "me.readyplayer.unityavatarloader", "https://github.com/readyplayerme/Unity-Avatar-Loader.git#feature/add-avatar-loader" },
                { "com.readyplayerme.webview", "https://github.com/readyplayerme/Unity-WebView.git#feature/sdk-web-view" },
                { "com.unity.nuget.newtonsoft-json", "2.0.2" },
                { "com.atteneder.gltfast", "https://github.com/atteneder/glTFast.git" }
            };

        public const string AVATAR_LOADER_PACKAGE = "me.readyplayer.unityavatarloader";
        public const string WEB_VIEW_PACKAGE = "com.readyplayerme.webview";
        public const string NEWTONSOFT_PACKAGE = "com.unity.nuget.newtonsoft-json";
        public const string GLTFAST_PACKAGE = "com.atteneder.gltfast";
    }
}