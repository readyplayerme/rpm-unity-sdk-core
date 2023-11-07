﻿using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    /// <summary>
    /// This class is a simple <see cref="Monobehaviour"/> to serve as an example on how to load a request a 2D render of a Ready Player Me avatar at runtime.
    /// </summary>
    public class AvatarRenderExample : MonoBehaviour
    {
        private const string TAG = nameof(AvatarRenderExample);

        [SerializeField] [Tooltip("Set this to the URL or shortcode of the Ready Player Me Avatar you want to render.")]
        private string url = "https://models.readyplayer.me/638df70ed72bffc6fa179596.glb";
        [SerializeField] [Tooltip("The scene to use for the avatar render.")]
        private AvatarRenderScene scene = AvatarRenderScene.FullBodyPostureTransparent;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private GameObject loadingPanel;

        private readonly string[] blendShapeMeshes = { "Wolf3D_Head", "Wolf3D_Teeth" };

        /// A collection of blendshape names and values to pose the face mesh into a smile using blendshapes
        private readonly List<BlendShape> blendShapes = new List<BlendShape>()
        {
            new BlendShape("mouthSmile", 0.7f),
            new BlendShape("viseme_aa", 0.5f),
            new BlendShape("jawOpen", 0.3f),
        };

        private void Start()
        {
            var avatarRenderer = new AvatarRenderLoader();
            avatarRenderer.OnCompleted = UpdateSprite;
            avatarRenderer.OnFailed = Fail;
            avatarRenderer.LoadRender(url, new AvatarRenderSettings()
            {
                BlendShapes = blendShapes
            });
            loadingPanel.SetActive(true);
        }

        /// Updates the sprite renderer with the provided render
        private void UpdateSprite(Texture2D render)
        {
            var sprite = Sprite.Create(render, new Rect(0, 0, render.width, render.height), new Vector2(.5f, .5f));
            spriteRenderer.sprite = sprite;
            loadingPanel.SetActive(false);
            SDKLogger.Log(TAG, "Sprite Updated ");
        }

        private void Fail(FailureType type, string message)
        {
            SDKLogger.Log(TAG, $"Failed with error type: {type} and message: {message}");
        }
    }
}
