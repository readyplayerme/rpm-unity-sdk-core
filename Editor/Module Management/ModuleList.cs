using System.Collections.Generic;

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
                branch = "fbb449b8f5e1c6b1626fc153505da89cca0ddb72",
                Version = "5.0.0"
            },
            new ModuleInfo
            {
                name = "com.readyplayerme.avatarloader",
                gitUrl = "https://github.com/readyplayerme/Unity-Avatar-Loader.git",
                branch = "develop",
                Version = "0.1.0"
            },
            new ModuleInfo
            {
                name = "com.readyplayerme.webview",
                gitUrl = "https://github.com/readyplayerme/Unity-WebView.git",
                branch = "develop",
                Version = "0.1.0"
            }
        };

        public static ModuleInfo DracoCompression = new ModuleInfo
        {
            name = "com.atteneder.draco",
            gitUrl = "https://github.com/atteneder/DracoUnity.git",
            branch = "",
            Version = "4.0.2"
        };

        public static ModuleInfo Core = new ModuleInfo
        {
            name = "com.readyplayerme.core",
            gitUrl = "https://github.com/readyplayerme/Unity-Core.git",
            branch = "",
            Version = "0.1.0"
        };

        public static Dictionary<string, string> GetInstalledModuleVersionDictionary()
        {
            var installedModules = new Dictionary<string, string>();
            installedModules.Add(Core.name, Core.Version);

            foreach (var module in Modules)
            {
                if (module.IsInstalled)
                {
                    installedModules.Add(module.name, module.Version);
                }
            }

            if (DracoCompression.IsInstalled)
            {
                installedModules.Add(DracoCompression.name, DracoCompression.Version);
            }

            return installedModules;
        }
    }
}
