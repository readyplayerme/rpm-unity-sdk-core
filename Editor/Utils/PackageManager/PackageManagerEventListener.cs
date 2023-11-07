using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    public class PackageManagerEventListener
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            Events.registeringPackages += OnPackagesInstalled;
        }

        static void OnPackagesInstalled(PackageRegistrationEventArgs packageRegistrationEventArgs)
        {
            foreach (var addedPackage in packageRegistrationEventArgs.added)
            {
                AnalyticsEditorLogger.EventLogger.LogPackageInstalled(
                    addedPackage.packageId,
                    addedPackage.displayName,
                    addedPackage.repository.url
                );
            }
        }
    }
}
