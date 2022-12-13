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
                name = "com.readyplayerme.avatarloader",
                gitUrl = "https://github.com/readyplayerme/Unity-Avatar-Loader.git",
                branch = "bugfix/namespace-fix"
            },
            new ModuleInfo 
            {
                name = "com.readyplayerme.webview",
                gitUrl = "https://github.com/readyplayerme/Unity-WebView.git",
                branch = "develop"
            }
        };

        public static readonly ModuleInfo DracoCompression = new ModuleInfo
        {
            name = "com.atteneder.draco",
            gitUrl = "https://github.com/atteneder/DracoUnity.git",
            branch = ""
        };
    }
}
