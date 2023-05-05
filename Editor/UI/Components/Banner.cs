using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    /// <summary>
    ///     Banner for RPM editor windows. Contains a image and version of the SDK.
    /// </summary>
    public class Banner : IEditorWindowComponent
    {
        private const int BANNER_WIDTH = 460;
        private const int BANNER_HEIGHT = 123;

        private const string BANNER_SEARCH_FILTER = "t:Texture rpm_editor_window_banner";

        private readonly Texture2D banner;

        public Banner()
        {
            var assetGuid = AssetDatabase.FindAssets(BANNER_SEARCH_FILTER).FirstOrDefault();
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            if (assetPath != null)
            {
                banner = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            }
        }

        public void Draw(Rect position)
        {
            var rect = new Rect((position.size.x - BANNER_WIDTH) / 2, 0, BANNER_WIDTH, BANNER_HEIGHT);
            GUI.DrawTexture(rect, banner);
            GUILayout.Space(128);
        }
    }
}
