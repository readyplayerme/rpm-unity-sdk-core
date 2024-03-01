using UnityEngine;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReadyPlayerMe.Core.Tests
{
    public class HelperTests
    {
        private readonly List<GameObject> gameObjects = new List<GameObject>();

        [OneTimeTearDown]
        public void Cleanup()
        {
            foreach (var avatar in gameObjects)
            {
                Object.DestroyImmediate(avatar);
            }
            gameObjects.Clear();
        }

        [Test]
        public void Setup_Animator_Fullbody()
        {
            var gameObject = new GameObject();
            gameObjects.Add(gameObject);
            var avatarMetadata = new AvatarMetadata
            {
                OutfitGender = OutfitGender.Masculine,
                BodyType = BodyType.FullBody
            };
            AvatarAnimationHelper.SetupAnimator(avatarMetadata, gameObject);
            var animator = gameObject.GetComponent<Animator>();
            Assert.True(animator != null);
            Assert.True(animator.runtimeAnimatorController == null);
        }

        [Test]
        public void Setup_Animator_Halfbody()
        {
            var gameObject = new GameObject();
            gameObjects.Add(gameObject);
            var avatarMetadata = new AvatarMetadata
            {
                OutfitGender = OutfitGender.Masculine,
                BodyType = BodyType.HalfBody
            };
            AvatarAnimationHelper.SetupAnimator(avatarMetadata, gameObject);
            var animator = gameObject.GetComponent<Animator>();
            Assert.True(animator != null);
        }
        
        [Test]
        public void AvatarMeshHelper_Check_Prefab_Meshes()
        {
            GameObject prefab = Resources.Load<GameObject>("RPM_Template_Avatar");
            var meshes = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();

            Assert.True(meshes.Length == 14);
        }

        [Test]
        public async Task AvatarMeshHelper_Transfer_Mesh()
        {
            GameObject source = null;
            var loader = new AvatarObjectLoader();
            loader.OnCompleted += (sender, args) => { source = args.Avatar; };
            loader.LoadAvatar(TestAvatarData.DefaultAvatarUri.ModelUrl);
        
            while (source == null) await Task.Yield();
            
            GameObject target = Resources.Load<GameObject>("RPM_Template_Avatar");
            
            AvatarMeshHelper.TransferMesh(source, target);
            
            var meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
            
            Assert.IsNotNull(source, "Source avatar is null");
            Assert.IsNotNull(target, "Target avatar is null");
            
            // Left eye
            Assert.True(meshes[0].sharedMesh != null, "Left eye mesh is null");
            Assert.True(meshes[0].sharedMaterial != null, "Left eye material is null");
            
            // Hair
            Assert.True(meshes[4].sharedMesh != null, "Hair mesh is null");
            Assert.True(meshes[4].sharedMaterial != null, "Hair material is null");
        }
    }
}
