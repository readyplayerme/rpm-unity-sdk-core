using System.Collections.Generic;
using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public enum DocumentationContext
    {
        AvatarCaching,
        Subdomain,
        AvatarConfig,
        GltfDeferAgent,
        DownloadAvatarIntoScene
    }
    
    public static class DocumentationButton
    {
        private struct DocumentationData
        {
            public string Url;
            public string ContextName;
        }
        
        private static readonly GUIStyle Style = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            fixedHeight = 18,
            fixedWidth = 18,
            margin = new RectOffset(2, 2, 0, 8),
            padding = new RectOffset(2, 0, 0, 1),
            normal =
            {
                textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f)
            },
            alignment = TextAnchor.MiddleCenter
            
        };

        public static void Draw(DocumentationContext context)
        {
            var documentationData = DocumentationContextMap[context];
            AnalyticsEditorLogger.EventLogger.LogFindOutMore(documentationData.ContextName);
            if (GUILayout.Button("?", Style))
            {
                Application.OpenURL(documentationData.Url);
            }
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
        }
        
        private static readonly Dictionary<DocumentationContext, DocumentationData> DocumentationContextMap = new Dictionary<DocumentationContext, DocumentationData>
        {
            { DocumentationContext.AvatarCaching, new DocumentationData { Url = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/optimize/avatar-caching", ContextName = "avatar caching" } },
            { DocumentationContext.Subdomain, new DocumentationData { Url = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/quickstart#before-you-begin", ContextName = "subdomain" } },
            { DocumentationContext.AvatarConfig, new DocumentationData { Url = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/optimize/avatar-configuration", ContextName = "avatar config" } },
            { DocumentationContext.GltfDeferAgent, new DocumentationData { Url = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/optimize/defer-agents", ContextName = "gltf defer agent" } },
            { DocumentationContext.DownloadAvatarIntoScene, new DocumentationData { Url = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/load-avatars#save-avatars-as-npcs-in-your-project", ContextName = "download avatar into scene" } }
        };
    }
}
