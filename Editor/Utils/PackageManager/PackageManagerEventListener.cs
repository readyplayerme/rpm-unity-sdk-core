using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

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
                //AnalyticsEditorLogger.EventLogger.LogPackageInstalled(addedPackage.displayName);
                Debug.Log($"Adding {addedPackage.displayName}");
                Debug.Log($"Adding {addedPackage.version}");
                Debug.Log($"Adding {addedPackage.repository.url}");
                Debug.Log($"Adding {addedPackage.repository.path}");
                Debug.Log($"Adding {addedPackage.packageId}");
                Debug.Log($"Adding {addedPackage.changelogUrl}");
            }
        }
    }
}
