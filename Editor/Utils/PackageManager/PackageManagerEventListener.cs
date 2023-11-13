﻿using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Editor.PackageManager.Extensions;
using ReadyPlayerMe.Core.Editor.Models;
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
            packageRegistrationEventArgs.added
                .Select(package => new PackageCoreInfo
                {
                    Name = package.displayName,
                    Url = package.packageId.TryParsePackageUrl()
                })
                .ToList()
                .ForEach(info => AnalyticsEditorLogger.EventLogger.LogPackageInstalled(info));
        }
    }
}