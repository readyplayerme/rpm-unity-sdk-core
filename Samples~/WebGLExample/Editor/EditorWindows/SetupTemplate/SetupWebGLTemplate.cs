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
        [SerializeField] private GameObject folder;

        public static void ShowWindow()
        {
            var window = GetWindow<SetupWebGLTemplate>();
        }

        private void CreateGUI()
        {
            this.titleContent = new GUIContent(TITLE);
            this.minSize = new Vector2(350, 120);
            this.maxSize = new Vector2(350, 120);
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
