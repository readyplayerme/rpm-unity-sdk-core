using UnityEngine;

namespace ReadyPlayerMe
{
    public static class ModuleList
    {
        public static readonly ModuleInfo[] Modules = 
        {
            new ModuleInfo 
            {
                name = "com.unity.nuget.newtonsoft-json",
                gitUrl = "",
                branch = ""
            },
            new ModuleInfo 
            {
            name = "com.readyplayer.avatarloader",
            gitUrl = "https://github.com/readyplayerme/Unity-Avatar-Loader.git",
            branch = "feature/add-avatar-loader"
                
            },
            new ModuleInfo 
            {
                name = "com.readyplayerme.webview",
                gitUrl = "https://github.com/readyplayerme/Unity-WebView.git",
                branch = "feature/sdk-web-view"
            },
            new ModuleInfo 
            {
                name = "com.atteneder.gltfast",
                gitUrl = "https://github.com/atteneder/glTFast.git",
                branch = ""
            }
        };
    }
}
