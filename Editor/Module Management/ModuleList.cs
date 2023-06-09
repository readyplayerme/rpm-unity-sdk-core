using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     Class <c>ModuleList</c> is a static class that can be referenced to get the latest module version.
    /// </summary>
    public static class ModuleList
    {
        private const string WEB_VIEW_PACKAGE = "com.readyplayerme.webview";
        private const string AVATAR_CREATOR_PACKAGE = "com.readyplayerme.avatarcreator";

        public static ModuleInfo Core = new ModuleInfo
        {
            name = "com.readyplayerme.core",
            description = "Ready Player Me Core is responsible for module management, SDK setup and providing shared functionality to our other modules.",
            gitUrl = "https://github.com/readyplayerme/rpm-unity-sdk-core.git",
            branch = "",
            version = "1.3.0"
        };

        /// <summary>
        ///     A static list of all the required modules represented in an array of <c>ModuleInfo</c>.
        /// </summary>
        public static readonly ModuleInfo[] RequiredModules =
        {
            new ModuleInfo
            {
                name = "com.atteneder.gltfast",
                description = "glTFast is a fast, multithreaded C# parser for the glTF 2.0 format.",
                gitUrl = "https://github.com/atteneder/glTFast.git",
                branch = "v5.0.0",
                version = "5.0.0"
            },
            new ModuleInfo
            {
                name = "com.readyplayerme.avatarloader",
                description = "Ready Player Me Avatar Loader is responsible for loading and displaying Ready Player Me avatars in Unity.",
                gitUrl = "https://github.com/readyplayerme/rpm-unity-sdk-avatar-loader.git",
                branch = "",
                version = "1.3.0"
            }
        };

        /// <summary>
        ///     A static list of all the optional modules represented in an array of <c>ModuleInfo</c>.
        /// </summary>
        public static readonly Dictionary<string, ModuleInfo> OptionalModules = new Dictionary<string, ModuleInfo>()
        {
            {
                WEB_VIEW_PACKAGE, new ModuleInfo
                {
                    name = WEB_VIEW_PACKAGE,
                    description = "Ready Player Me Web View is responsible for displaying the Ready Player Me web Avatar Creator in Unity applications.",
                    gitUrl = "https://github.com/readyplayerme/rpm-unity-sdk-webview.git",
                    branch = "",
                    version = "1.2.0"
                }
            },
            {
                AVATAR_CREATOR_PACKAGE, new ModuleInfo
                {
                    name = AVATAR_CREATOR_PACKAGE,
                    description = "For creating RPM avatars in Unity applications without the need for web browser.",
                    gitUrl = "https://github.com/readyplayerme/rpm-unity-sdk-avatar-creator.git",
                    branch = "",
                    version = "0.3.1"
                }
            }
        };


        /// <summary>
        ///     Unity Module that adds support for gltf files that use DracoCompression.
        /// </summary>
        public static ModuleInfo DracoCompression = new ModuleInfo
        {
            name = "com.atteneder.draco",
            gitUrl = "https://github.com/atteneder/DracoUnity.git",
            branch = "v4.0.2",
            version = "4.0.2"
        };

        /// <summary>
        ///     Get installed modules from Modules list.
        /// </summary>
        /// <returns>A <see cref="Dictionary"/> of installed Unity Module information in the format of <c>Dictionary&lt;string: name, string: version&gt;</c>. </returns>
        public static Dictionary<string, string> GetInstalledModuleVersionDictionary()
        {
            PackageInfo[] packageList = ModuleInstaller.GetPackageList();

            var installedModules = new Dictionary<string, string>();
            installedModules.Add(Core.name, Core.version);

            foreach (ModuleInfo module in RequiredModules)
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
