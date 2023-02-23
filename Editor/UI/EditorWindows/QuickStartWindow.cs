using ReadyPlayerMe.Core.Editor;
using UnityEditor;
using UnityEngine;

public class QuickStartWindow : EditorWindow
{
    private const string DESCRIPTION = "New to Ready Player Me? ";
    private const string DESCRIPTION2 = "Get started with the QuickStart sample.";
    private Banner banner;
    private GUIStyle buttonStyle;
    private GUIStyle descriptionStyle;
    private GUIStyle headingStyle;
    private const int BUTTON_FONT_SIZE = 12;

    [MenuItem("Ready Player Me/Quick Start")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuickStartWindow), false, "Quick Start");
    }

    private void LoadAssets()
    {
        banner ??= new Banner("Banner");
        
        buttonStyle ??= new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fixedWidth = 149,
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
    
    private void OnGUI()
    {
        LoadAssets();
        
        EditorGUILayout.BeginVertical();
        banner.Draw(position);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(DESCRIPTION, headingStyle);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(DESCRIPTION2, descriptionStyle);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close", buttonStyle))
        {
            Debug.Log("Close");
        }
        GUILayout.Space(50);
        if (GUILayout.Button("QuickStart", buttonStyle))
        {
            Debug.Log("QuickStart");
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }
}
