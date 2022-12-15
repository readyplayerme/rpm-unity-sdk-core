using System.Collections.Generic;
using UnityEngine;

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
                branch = "fbb449b8f5e1c6b1626fc153505da89cca0ddb72",
                version = "4.0.0"
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
            branch = "",
            version = "4.0.2"
        };

        public static Dictionary<string, string> GetInstalledModuleVersionDictionary()
        {
            var installedModules = new Dictionary<string, string>();
            installedModules.Add(Core.name, Core.version);

            foreach (var module in Modules)
            {
                Debug.Log("Module: " + module.name + ", " + module.isInstalled);
                if (module.isInstalled)
                {
                    installedModules.Add(module.name, module.version);
                }
            }

            if (DracoCompression.isInstalled)
            {
                installedModules.Add(DracoCompression.name, DracoCompression.version);
            }

            return installedModules;
        }
    }
}
