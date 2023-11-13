using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Editor.PackageManager.Extensions;
using ReadyPlayerMe.Core.Editor.Models;
using UnityEditor;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ReadyPlayerMe.Core.Editor
{
    public class PackageInstallerPostProcessor : AssetPostprocessor
    {
        private const string READY_PLAYER_ME_PACKAGE_PATH = "Packages/com.readyplayerme.core";

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var readyPlayerMeCorePackage = importedAssets.FirstOrDefault(package => package == READY_PLAYER_ME_PACKAGE_PATH);

            if (readyPlayerMeCorePackage == null)
                return;

            var packageInfo = PackageInfo.FindForAssetPath(READY_PLAYER_ME_PACKAGE_PATH);
            
            AnalyticsEditorLogger.EventLogger.LogPackageInstalled(new PackageCoreInfo
                {
                    Id = packageInfo.packageId,
                    Name = packageInfo.name,
                    Url = packageInfo.packageId.TryParsePackageUrl()
                },
                force: true
            );
        }
    }
}
