using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.Core.Editor
{
    public class QuickStartPanel : IEditorWindowComponent
    {
        
        private const string HEADING = "New to Ready Player Me? ";
        private const string DESCRIPTION = "Get started with the QuickStart sample.";
        private static bool neverAskAgain;

        private bool variablesLoaded;

        private GUIStyle buttonStyle;
        private GUIStyle descriptionStyle;
        private GUIStyle headingStyle;
        
        public readonly UnityEvent OnQuickStartClick = new UnityEvent();
        public readonly UnityEvent OnCloseClick = new UnityEvent();

        private const string LOADER_PACKAGE = "com.readyplayerme.avatarloader";
        private const string QUICKSTART_SAMPLE_NAME = "QuickStart";

        private void LoadStyles()
        {
            buttonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fixedWidth = 225,
                fixedHeight = 30f,
                padding = new RectOffset(5, 5, 5, 5),
                fontStyle = FontStyle.Bold
            };

            descriptionStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                richText = true,
                fontStyle = FontStyle.Bold,
                wordWrap = true,
                fixedWidth = 450,
                normal =
                {

                    textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f)
                },
                alignment = TextAnchor.MiddleCenter
            };

            headingStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                richText = true,
                fontStyle = FontStyle.Bold,
                wordWrap = true,
                fixedWidth = 450,
                normal =
                {
                    textColor = Color.white
                },
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        public void Draw(Rect position = new Rect())
        {
            LoadStyles();
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.Label(HEADING, headingStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(8);
            
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.Label(DESCRIPTION, descriptionStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", buttonStyle))
            {
                OnCloseClick?.Invoke();
            }
            if (GUILayout.Button(QUICKSTART_SAMPLE_NAME, buttonStyle))
            {
                var quickStartSample = GetQuickStartSample();
                if (!quickStartSample.isImported)
                {
                    ImportAndOpenSample(quickStartSample);
                }
                OnQuickStartClick?.Invoke();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8);
        }

        private Sample GetQuickStartSample()
        {
            var samples = Sample.FindByPackage(LOADER_PACKAGE, null).ToArray();
            var quickStartSample = samples.First(x => x.displayName == QUICKSTART_SAMPLE_NAME);
            return quickStartSample;
        }
        
        private void ImportAndOpenSample(Sample quickStartSample)
        {
            quickStartSample.Import();
            while (!quickStartSample.isImported)
                Thread.Sleep(1);
            OpenQuickStartScene(quickStartSample.importPath);
        }

        private void OpenQuickStartScene(string importPath)
        {
            EditorSceneManager.OpenScene($"{importPath}/{QUICKSTART_SAMPLE_NAME}.unity");
        }
    }
}
