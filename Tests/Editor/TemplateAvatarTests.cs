using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Tests
{
    public class TemplateAvatarTests
    {
        private const string PATH_TEMPLATE_AVATAR_XR = "Assets/Ready Player Me/Core/Runtime/Core/Prefabs/RPM_Template_Avatar_XR.prefab";
        private const string PATH_TEMPLATE_AVATAR = "Assets/Ready Player Me/Core/Runtime/Core/Prefabs/RPM_Template_Avatar.prefab";
        
        [Test]
        public Task Check_Template_Avatar_XR()
        {
            var avatar = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_TEMPLATE_AVATAR_XR);
            Assert.IsNotNull(avatar, $"Failed to load '{PATH_TEMPLATE_AVATAR_XR}' from Resources.");

            var renderers = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            Assert.IsNotEmpty(renderers, "No SkinnedMeshRenderer components found on the avatar.");

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Assert.IsNotNull(renderer.bones, $"Bones array in SkinnedMeshRenderer on {renderer.gameObject.name} is null.");
                Assert.IsNotEmpty(renderer.bones, $"Bones array in SkinnedMeshRenderer on {renderer.gameObject.name} is empty.");
                foreach (var bone in renderer.bones)
                {
                    Assert.IsNotNull(bone, $"A bone in SkinnedMeshRenderer on {renderer.gameObject.name} is null.");
                }
            }
            return Task.CompletedTask;
        }

        [Test]
        public Task Check_Template_Avatar()
        {
            var avatar = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_TEMPLATE_AVATAR);
            Assert.IsNotNull(avatar, $"Failed to load '{PATH_TEMPLATE_AVATAR}' from Resources.");

            var renderers = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            Assert.IsNotEmpty(renderers, "No SkinnedMeshRenderer components found on the avatar.");

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Assert.IsNotNull(renderer.bones, $"Bones array in SkinnedMeshRenderer on {renderer.gameObject.name} is null.");
                Assert.IsNotEmpty(renderer.bones, $"Bones array in SkinnedMeshRenderer on {renderer.gameObject.name} is empty.");
                foreach (var bone in renderer.bones)
                {
                    Assert.IsNotNull(bone, $"A bone in SkinnedMeshRenderer on {renderer.gameObject.name} is null.");
                }
            }
            return Task.CompletedTask;
        }
    }
}
