using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Editor.Models;
using UnityEditor;
using UnityEngine;
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
            
            Debug.Log("here");
            
            if (readyPlayerMeCorePackage == null)
                return;

            Debug.Log("here 2");
            
            var packageInfo = PackageInfo.FindForAssetPath(READY_PLAYER_ME_PACKAGE_PATH);
            
            Debug.Log(packageInfo.resolvedPath);

            AnalyticsEditorLogger.EventLogger.LogPackageInstalled(new PackageCoreInfo
                {
                    Id = packageInfo.packageId,
                    Name = packageInfo.name,
                    Url = packageInfo.resolvedPath
                },
                force: true
            );
        }
    }
}
