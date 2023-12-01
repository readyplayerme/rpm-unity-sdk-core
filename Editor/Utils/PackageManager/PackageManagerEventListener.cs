using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe.Core.Editor
{
    public abstract class PackageManagerEventListener
    {
        private const string REQUIRED_KEYWORD = "readyplayerme";
        
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            Events.registeringPackages += OnPackagesInstalled;
        }
        
        ~PackageManagerEventListener()
        {
            Events.registeringPackages -= OnPackagesInstalled;
        }

        static void OnPackagesInstalled(PackageRegistrationEventArgs packageRegistrationEventArgs)
        {
            packageRegistrationEventArgs.added
                .Where(packageInfo => packageInfo.packageId.Contains(REQUIRED_KEYWORD))
                .ToList()
                .ForEach(packageInfo => 
                    AnalyticsEditorLogger.EventLogger.LogPackageInstalled(packageInfo.name, packageInfo.packageId));
        }
    }
}
