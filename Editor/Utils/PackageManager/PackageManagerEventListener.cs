using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Editor.Models;
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
            Debug.Log("here");
            packageRegistrationEventArgs.added
                .Select(package =>
                {

                    Debug.Log(package.repository.url);

                    return new PackageCoreInfo
                    {
                        Id = package.packageId,
                        Name = package.displayName,
                        Url = package.repository.url
                    };
                })
                .ToList()
                .ForEach(AnalyticsEditorLogger.EventLogger.LogPackageInstalled);
        }
    }
}
