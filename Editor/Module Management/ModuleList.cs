using System.Collections.Generic;
using System.Linq;

namespace ReadyPlayerMe.Core.Editor
{
    public static class ModuleList
    {
        public static ModuleInfo Core = new ModuleInfo
        {
            name = "com.readyplayerme.core",
            gitUrl = "https://github.com/readyplayerme/Unity-Core.git",
            branch = "",
            version = "0.1.0"
        };

        public static readonly ModuleInfo[] Modules =
        {
            new ModuleInfo
            {
                name = "com.atteneder.gltfast",
                gitUrl = "https://github.com/atteneder/glTFast.git",
                branch = "v5.0.0",
                version = "5.0.0"
            },
            new ModuleInfo
            {
                name = "com.readyplayerme.avatarloader",
                gitUrl = "https://github.com/readyplayerme/Unity-Avatar-Loader.git",
                branch = "develop",
                version = "0.1.0"
            },
            new ModuleInfo
            {
                name = "com.readyplayerme.webview",
                gitUrl = "https://github.com/readyplayerme/Unity-WebView.git",
                branch = "develop",
                version = "0.1.0"
            }
        };

        public static ModuleInfo DracoCompression = new ModuleInfo
        {
            name = "com.atteneder.draco",
            gitUrl = "https://github.com/atteneder/DracoUnity.git",
            branch = "v4.0.2",
            version = "4.0.2"
        };

        public static Dictionary<string, string> GetInstalledModuleVersionDictionary()
        {
            var packageList = ModuleInstaller.GetPackageList();

            var installedModules = new Dictionary<string, string>();
            installedModules.Add(Core.name, Core.version);

            foreach (var module in Modules)
            {
                if (packageList.Any(x => x.name == module.name))
                {
                    installedModules.Add(module.name, module.version);
                }
            }

            if (packageList.Any(x => x.name == DracoCompression.name))
            {
                installedModules.Add(DracoCompression.name, DracoCompression.version);
            }

            return installedModules;
        }
    }
}
