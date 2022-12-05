
namespace ReadyPlayerMe.Core.Editor
{
    public static class ModuleList
    {
        public static readonly ModuleInfo[] Modules = 
        {
            new ModuleInfo 
            {
                name = "com.atteneder.gltfast",
                gitUrl = "https://github.com/atteneder/glTFast.git",
                branch = "fbb449b8f5e1c6b1626fc153505da89cca0ddb72"
            },
            new ModuleInfo 
            {
                name = "com.readyplayer.avatarloader",
                gitUrl = "https://github.com/readyplayerme/Unity-Avatar-Loader.git",
                branch = ""
            },
            new ModuleInfo 
            {
                name = "com.readyplayerme.webview",
                gitUrl = "https://github.com/readyplayerme/Unity-WebView.git",
                branch = ""
            }

        };
    }
}
