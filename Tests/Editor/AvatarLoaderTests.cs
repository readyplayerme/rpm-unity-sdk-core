using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace ReadyPlayerMe.Core.Tests
{
    public class AvatarLoaderTests
    {
        private GameObject avatar;

        [TearDown]
        public void Cleanup()
        {
            TestUtils.DeleteAvatarDirectoryIfExists(TestAvatarData.DefaultAvatarUri.Guid, true);
            TestUtils.DeleteAvatarDirectoryIfExists(TestUtils.TEST_WRONG_GUID, true);

            if (avatar != null)
            {
                Object.DestroyImmediate(avatar);
            }
        }

        [UnityTest]
        public IEnumerator AvatarLoader_Complete_Load()
        {
            var avatarUrl = string.Empty;
            var failureType = FailureType.None;

            var loader = new AvatarObjectLoader();
            loader.OnCompleted += (sender, args) =>
            {
                avatar = args.Avatar;
                avatarUrl = args.Url;
            };
            loader.OnFailed += (sender, args) => { failureType = args.Type; };
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);

            yield return new WaitUntil(() => avatar != null || failureType != FailureType.None);

            Assert.AreEqual(TestAvatarData.DefaultAvatarUri.ModelUrl, avatarUrl);
            Assert.AreEqual(FailureType.None, failureType);
            Assert.IsNotNull(avatar);
            Assert.IsNotNull(avatar.GetComponent<AvatarData>());
        }

        [Test]
        public async Task AvatarLoader_Complete_Load_Async()
        {
            var loader = new AvatarObjectLoader();
            EventArgs args = await loader.LoadAvatarAsync(TestAvatarData.DefaultAvatarUri.ModelUrl);

            Assert.True(args != null);

            if (args.GetType() == typeof(FailureEventArgs))
            {
                Assert.Fail(((FailureEventArgs) args).Type.ToString());
            }
            else if (args.GetType() == typeof(CompletionEventArgs))
            {
                var completedEventArgs = (CompletionEventArgs) args;
                Assert.AreEqual(TestAvatarData.DefaultAvatarUri.ModelUrl, completedEventArgs.Url);
                Assert.IsNotNull(completedEventArgs.Avatar);
                avatar = completedEventArgs.Avatar;
            }
            else
            {
                Assert.Fail("Unknown event args type");
            }
        }

        [UnityTest]
        public IEnumerator AvatarLoader_Fail_Load()
        {
            var failureType = FailureType.None;
            var avatarUrl = string.Empty;

            var loader = new AvatarObjectLoader();
            loader.OnFailed += (sender, args) =>
            {
                failureType = args.Type;
                avatarUrl = args.Url;
            };
            loader.LoadAvatar(TestAvatarData.WrongUri.ModelUrl);

            yield return new WaitUntil(() => failureType != FailureType.None);

            Assert.AreEqual(TestAvatarData.WrongUri.ModelUrl, avatarUrl);
            Assert.AreNotEqual(FailureType.None, failureType);
        }

        [UnityTest]
        public IEnumerator AvatarLoader_Clears_Persistent_Cache()
        {
            AvatarLoaderSettings settings = AvatarLoaderSettings.LoadSettings();
            settings.AvatarCachingEnabled = true;

            var failureType = FailureType.None;

            var loader = new AvatarObjectLoader();
            loader.OnCompleted += (_, args) => avatar = args.Avatar;
            loader.OnFailed += (_, args) => failureType = args.Type;
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);

            yield return new WaitUntil(() => avatar != null || failureType != FailureType.None);

            Assert.AreEqual(FailureType.None, failureType);
            Assert.AreEqual(false, AvatarCache.IsCacheEmpty());

            AvatarCache.Clear();
            Assert.AreEqual(true, AvatarCache.IsCacheEmpty());

            settings.AvatarCachingEnabled = false;
        }

        [UnityTest]
        public IEnumerator AvatarLoader_Cancel_Loading()
        {
            var failureType = FailureType.None;
            var loader = new AvatarObjectLoader();

            loader.OnCompleted += (sender, args) =>
            {
                avatar = args.Avatar;
            };
            loader.OnFailed += (sender, args) => { failureType = args.Type; };
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);

            var frameCount = 0;
            const int cancelAfterFramesCount = 10;

            while (failureType == FailureType.None && avatar == null)
            {
                if (frameCount > cancelAfterFramesCount)
                {
                    loader.Cancel();
                }

                frameCount++;
                yield return null;
            }

            Assert.AreNotEqual(FailureType.None, failureType);
            Assert.AreEqual(null, avatar);
        }

        [UnityTest]
        public IEnumerator AvatarLoader_Low_LOD_Smaller_than_High_LOD()
        {
            var failureType = FailureType.None;

            var avatarConfig = ScriptableObject.CreateInstance<AvatarConfig>();
            avatarConfig.Lod = Lod.Low;
            avatarConfig.TextureAtlas = TextureAtlas.Low;
            avatarConfig.TextureChannel = Array.Empty<TextureChannel>();

            var loader = new AvatarObjectLoader();
            loader.OnCompleted += (sender, args) =>
            {
                avatar = args.Avatar;
            };
            loader.AvatarConfig = avatarConfig;
            loader.OnFailed += (sender, args) => { failureType = args.Type; };
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);
            yield return new WaitUntil(() => avatar != null || failureType != FailureType.None);
            var thisRenderer = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            var lowLODVertices = thisRenderer.Aggregate(0, (totalVertices, renderer) => totalVertices + renderer.sharedMesh.vertexCount);

            Object.DestroyImmediate(avatar);
            loader = new AvatarObjectLoader();
            avatarConfig.Lod = Lod.High;
            loader.AvatarConfig = avatarConfig;

            loader.OnCompleted += (sender, args) =>
            {
                avatar = args.Avatar;
                Object.DestroyImmediate(avatarConfig);
            };
            loader.OnFailed += (sender, args) => { failureType = args.Type; };
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);
            yield return new WaitUntil(() => avatar != null || failureType != FailureType.None);
            thisRenderer = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            var highLODVertices = thisRenderer.Aggregate(0, (totalVertices, renderer) => totalVertices + renderer.sharedMesh.vertexCount);

            Assert.IsTrue(lowLODVertices < highLODVertices);
        }
    }
}
