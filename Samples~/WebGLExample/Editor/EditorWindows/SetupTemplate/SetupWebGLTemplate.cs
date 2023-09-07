using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Samples
{
    public class SetupWebGLTemplate : EditorWindow
    {
        private string destinationPath;
        private string sourcePath;

        private const string TITLE = "RPM WebGL";

        private const string CONFIRM_BUTTON = "ConfirmButton";
        private const string CANCEL_BUTTON = "CancelButton";
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        private VisualTreeAsset GetVisualTreeAsset()
        {
            MonoScript script = MonoScript.FromScriptableObject(this);

            if (script != null)
            {
                string scriptPath = AssetDatabase.GetAssetPath(script);

                if (!string.IsNullOrEmpty(scriptPath))
                {
                    string folderPath = System.IO.Path.GetDirectoryName(scriptPath);
                    string visualTreeAssetPath = folderPath + "/" + script.name + ".uxml";

                    VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreeAssetPath);

                    return visualTreeAsset;
                }
                else
                {
                    Debug.LogError("Script path is empty.");
                }
            }
            else
            {
                Debug.LogError("MonoScript not found for this script.");
            }
            return null;
        }
        
        public static void ShowWindow()
        {
            var window = GetWindow<SetupWebGLTemplate>();
            window.titleContent = new GUIContent(TITLE);
            window.minSize = new Vector2(350, 120);
            window.maxSize = new Vector2(350, 120);
        }

        private void CreateGUI()
        {
            visualTreeAsset = GetVisualTreeAsset();
            if (!visualTreeAsset)
            {
                this.Close();
                return;
            }
            visualTreeAsset.CloneTree(rootVisualElement);
            var confirm = rootVisualElement.Q<Button>(CONFIRM_BUTTON);
            confirm.clicked += OnConfirm;
            var cancel = rootVisualElement.Q<Button>(CANCEL_BUTTON);
            cancel.clicked += OnCancel;
        }

        private bool SetTemplatePaths()
        {
            string[] res = Directory.GetFiles(Application.dataPath, "SetupWebGLTemplate.cs", SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                Debug.LogError("error message ..");
                return false;
            }
            string path = res[0].Replace("SetupWebGLTemplate.cs", "").Replace("\\", "/");
            sourcePath = path.Substring(0, path.IndexOf("/Editor/")) + "/WebGLTemplates/RPMTemplate";
            destinationPath = path.Substring(0, path.IndexOf("/Assets")) + "/Assets";
            return true;
        }

        private void Copy()
        {
            foreach (string sourceFile in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (sourceFile.EndsWith(".meta"))
                {
                    continue;
                }

                var sourceFilePath = sourceFile.Replace("\\", "/");

                if (File.Exists(sourceFilePath))
                {
                    string destination = this.destinationPath + sourceFilePath.Substring(sourceFilePath.IndexOf("/WebGLTemplates/RPMTemplate")).Replace("\\", "/");

                    if (!Directory.Exists(destination.Substring(0, destination.LastIndexOf("/"))))
                    {
                        Directory.CreateDirectory(destination.Substring(0, destination.LastIndexOf("/")));
                    }

                    File.Copy(sourceFilePath, destination, true);
                }
                else
                {
                    Debug.LogError("Source file does not exist: " + sourceFilePath);
                }
            }
            Debug.Log("Copied RPMTemplate to the WebGLTemplate folder in the root path of Assets");
            AssetDatabase.Refresh();
        }

        private void OnConfirm()
        {
            if (!SetTemplatePaths())
            {
                Debug.LogError("Failed to set source and destination paths. No changes were done to project");
                return;
            }
            Copy();
            SetWebGLTemplate();
            this.Close();
        }

        private void SetWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:RPMTemplate";
            Debug.Log("Updated player settings to use RPMTemplate");
        }

        private void OnCancel()
        {
            this.Close();
        }
    }
}
