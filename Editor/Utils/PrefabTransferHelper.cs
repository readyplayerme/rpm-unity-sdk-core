using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public static class PrefabTransferHelper
    {
        public static void TransferPrefabByGuid(string guid, string newPath)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"Prefab with guid {guid} not found.");
                return;
            }
            AssetDatabase.CopyAsset(path, newPath);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(newPath, typeof(GameObject));
        }
    }
}
