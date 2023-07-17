﻿using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Loader;
using UnityEngine;

namespace ReadyPlayerMe.AvatarLoader
{
    /// <summary>
    /// This class is responsible for for processing, requesting and loading a 2D render of an avatar.
    /// </summary>
    public class AvatarRenderLoader
    {
        public int Timeout { get; set; }

        /// Called upon failure
        public Action<FailureType, string> OnFailed { get; set; }

        /// Called upon success.
        public Action<Texture2D> OnCompleted { get; set; }

        /// Called when the progress of the operation changes.
        public Action<float, string> ProgressChanged { get; set; }

        private OperationExecutor<AvatarContext> executor;

        /// <summary>
        /// This method runs through the complete avatar render loading process and returns a <see cref="Texture2D" /> with via
        /// the <see cref="AvatarRenderLoader.OnCompleted" /> event.
        /// </summary>
        /// <param name="url">The url to the avatars .glb file.</param>
        /// <param name="renderScene">The <see cref="AvatarRenderScene" /> to use for the avatar render.</param>
        /// <param name="blendShapeMeshes">
        /// The name of the <see cref="SkinnedMeshRenderer" /> that contains the blendshapes you
        /// want to set.
        /// </param>
        /// <param name="renderBlendShapes">A map of blendshape names and values that you want to set.</param>
        public async void LoadRender(
            string url,
            AvatarRenderScene renderScene,
            string[] blendShapeMeshes = null,
            Dictionary<string, float> renderBlendShapes = null
        )
        {
            var renderSettings = new AvatarRenderSettings
            {
                Model = url,
                Scene = renderScene,
                BlendShapeMeshes = blendShapeMeshes,
                BlendShapes = renderBlendShapes
            };

            var context = new AvatarContext();
            context.Url = renderSettings.Model;
            context.RenderSettings = renderSettings;

            executor = new OperationExecutor<AvatarContext>(new IOperation<AvatarContext>[]
            {
                new UrlProcessor(),
                new MetadataDownloader(),
                new AvatarRenderDownloader()
            });
            executor.ProgressChanged += ProgressChanged;
            executor.Timeout = Timeout;

            try
            {
                context = await executor.Execute(context);
            }
            catch (CustomException exception)
            {
                OnFailed?.Invoke(exception.FailureType, exception.Message);
                return;
            }

            OnCompleted?.Invoke((Texture2D) context.Data);
        }

        /// <summary>
        /// Cancel the execution
        /// </summary>
        public void Cancel()
        {
            executor?.Cancel();
        }
    }
}
