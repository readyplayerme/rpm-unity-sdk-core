using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class IntegrationGuide : EditorWindow
{
    [SerializeField] private VisualTreeAsset visualTreeAsset;
    
    [MenuItem("Ready Player Me/Integration Guide")]
    public static void ShowWindow()
    {
        var window = GetWindow<IntegrationGuide>();
        window.titleContent = new GUIContent("IntegrationGuide");
    }

    public void CreateGUI()
    {
        visualTreeAsset.CloneTree(rootVisualElement);
    }
}
