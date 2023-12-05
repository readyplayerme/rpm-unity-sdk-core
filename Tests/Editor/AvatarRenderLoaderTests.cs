using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ReadyPlayerMe.Core.Tests
{
    public class AvatarRenderLoaderTests
    {
        private const RenderCamera RENDER_SCENE = RenderCamera.FullBody;
        private const string RENDER_BLENDSHAPE = "mouthSmile";
        private const float BLENDSHAPE_VALUE = 0.5f;
        private const string RENDER_STRING = "?pose=relaxed&blendShapes[mouthSmile]=0.5&camera=portrait&size=800&background=255,255,255";

        [UnityTest]
        public IEnumerator RenderLoader_Load()
        {
            Texture2D renderTexture = null;
            var failureType = FailureType.None;

            var renderLoader = new AvatarRenderLoader();
            renderLoader.OnCompleted = data => renderTexture = data;
            renderLoader.OnFailed = (failType, message) => failureType = failType;

            renderLoader.LoadRender(TestAvatarData.DefaultAvatarUri.ModelUrl, new AvatarRenderSettings()
            {
                Camera = RENDER_SCENE
            });

            yield return new WaitUntil(() => renderTexture != null || failureType != FailureType.None);

            Assert.AreEqual(FailureType.None, failureType);
            Assert.IsNotNull(renderTexture);
        }

        [UnityTest]
        public IEnumerator Fail_RenderLoader_Load_Wrong_Url()
        {
            Texture2D renderTexture = null;
            var failureType = FailureType.None;

            var renderLoader = new AvatarRenderLoader();
            renderLoader.OnCompleted = data => renderTexture = data;
            renderLoader.OnFailed = (failType, message) => failureType = failType;

            renderLoader.LoadRender(TestAvatarData.WrongUri.ModelUrl, new AvatarRenderSettings()
            {
                Camera = RENDER_SCENE
            });

            yield return new WaitUntil(() => renderTexture != null || failureType != FailureType.None);

            Assert.AreEqual(FailureType.MetadataDownloadError, failureType);
            Assert.IsNull(renderTexture);
        }

        [UnityTest]
        public IEnumerator RenderLoader_Load_With_Correct_BlendShape_Parameters()
        {
            Texture2D renderTexture = null;
            var failureType = FailureType.None;

            var renderLoader = new AvatarRenderLoader();
            renderLoader.OnCompleted = data => renderTexture = data;
            renderLoader.OnFailed = (failType, message) => failureType = failType;

            renderLoader.LoadRender(
                TestAvatarData.DefaultAvatarUri.ModelUrl,
                new AvatarRenderSettings()
                {
                    Camera = RENDER_SCENE,
                    BlendShapes = new List<BlendShape>
                        { new BlendShape(RENDER_BLENDSHAPE, BLENDSHAPE_VALUE) }
                }
            );

            yield return new WaitUntil(() => renderTexture != null || failureType != FailureType.None);

            Assert.AreEqual(FailureType.None, failureType);
            Assert.IsNotNull(renderTexture);
        }

        [UnityTest]
        public IEnumerator RenderLoader_Load_Incorrect_BlendShape_Shape_Parameter()
        {
            Texture2D renderTexture = null;
            var failureType = FailureType.None;

            var renderLoader = new AvatarRenderLoader();
            renderLoader.OnCompleted = data => renderTexture = data;
            renderLoader.OnFailed = (failType, message) => failureType = failType;

            renderLoader.LoadRender(
                TestAvatarData.DefaultAvatarUri.ModelUrl,
                new AvatarRenderSettings()
                {
                    Camera = RENDER_SCENE,
                    BlendShapes = new List<BlendShape>
                        { new BlendShape(RENDER_BLENDSHAPE, BLENDSHAPE_VALUE) }
                }
            );

            yield return new WaitUntil(() => renderTexture != null || failureType != FailureType.None);

            Assert.AreEqual(FailureType.None, failureType);
            Assert.IsNotNull(renderTexture);
            
        }

        // create a render string test
        [Test]
        public void RenderLoader_CreateRenderString()
        {
            var renderSettings = new AvatarRenderSettings
            {
                 BlendShapes = new List<BlendShape>
                        { new BlendShape(RENDER_BLENDSHAPE, BLENDSHAPE_VALUE) }
            };

            var renderString = renderSettings.GetParametersAsString();

            Debug.Log(renderString);
            Assert.AreEqual(RENDER_STRING, renderString);
        }
    }
}
