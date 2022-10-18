using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Threading;
using UnityEditor.PackageManager;

namespace ReadyPlayerMe
{
    [InitializeOnLoad]
    public class ModuleInstaller : AssetPostprocessor
    {
        private static string INSTALL_COMPLETED_KEY = "RPM_ModuleInstallCompleted";
        private static string PROGRESS_BAR_TITLE = "Ready Player Me";
        
        static ModuleInstaller()
        {
            EditorApplication.update += InstallModules;
        }

        [MenuItem("RPM/Install Modules")]
        private static void ForceInstallModules()
        {
            SessionState.SetBool(INSTALL_COMPLETED_KEY, false);
            InstallModules();
        }

        private static void InstallModules()
        {
            if (!SessionState.GetBool(INSTALL_COMPLETED_KEY, false))
            {
                EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Installing modules...", 0);

                int count = ModuleList.Modules.Length;
                for(var i = 0; i < count; i++)
                {
                    var packages = GetPackageList();
                    
                    var module = ModuleList.Modules[i];
                    
                    if (packages.All(info => info.name != module.name))
                    {
                        EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"Installing module {module.name}", i * count * 0.1f + 0.1f);
                        AddModule(module.Identifier);
                    }
                    else
                    {
                        EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"All modules are loaded.", 1);
                        SessionState.SetBool(INSTALL_COMPLETED_KEY, true);
                    }
                }

                Thread.Sleep(200);
                EditorUtility.ClearProgressBar();
                EditorApplication.update -= InstallModules;
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Any(path => path.StartsWith("Packages")))
            {
                if(importedAssets.Any(path => path.StartsWith("Packages/com.readyplayerme")))
                {
                    SessionState.SetBool(INSTALL_COMPLETED_KEY, true);
                }
                InstallModules();
            }
        }

        private static PackageCollection GetPackageList()
        {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(20);
 
            if (listRequest.Error != null)
            {
                Debug.Log("Error: " + listRequest.Error.message);
                return null;
            }
            
            return listRequest.Result;
        }

        private static void AddModule(string name)
        {
            var addRequest = Client.Add(name);
            while (!addRequest.IsCompleted)
                Thread.Sleep(20);
                    
            if (addRequest.Error != null)
            {
                Debug.Log("Error: " + addRequest.Error.message);
            }
        }
    }
}
