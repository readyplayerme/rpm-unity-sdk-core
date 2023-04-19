using UnityEditor;
using UnityEngine;

public static class ProjectPrefs
{
    public static bool GetBool(string key)
    {
        return EditorPrefs.GetBool($"{Application.dataPath}{key}");
    }

    public static void SetBool(string key, bool value)
    {
        EditorPrefs.SetBool($"{Application.dataPath}{key}", value);
    }
}
