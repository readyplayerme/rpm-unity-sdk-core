using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public static class PrefabTransferHelper
    {
        public static void TransferPrefabByGuid(string guid, string newPath)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.CopyAsset(path, newPath);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(newPath, typeof(GameObject));
        }
    }
}
