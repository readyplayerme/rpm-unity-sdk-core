using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    public abstract class PackageManagerEventListener
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            Events.registeringPackages += OnPackagesInstalled;
        }

        static void OnPackagesInstalled(PackageRegistrationEventArgs packageRegistrationEventArgs)
        {
            packageRegistrationEventArgs.added
                .ToList()
                .ForEach(x =>
                {
                    AnalyticsEditorLogger.EventLogger.LogPackageInstalled(x.name, x.packageId);
                });
        }
    }
}
