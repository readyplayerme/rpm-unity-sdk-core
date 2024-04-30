using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace ReadyPlayerMe.Core.Tests
{
    public class TemplateAvatarTests
    {
        //RPM_Template_Avatar_XR

        [OneTimeTearDown]
        public void Cleanup()
        {
        }

        [Test]
        public Task Check_Template_Avatar_XR()
        {
            var avatar = Resources.Load<GameObject>("RPM_Template_Avatar_XR");
            Assert.IsNotNull(avatar, "Failed to load 'RPM_Template_Avatar_XR' from Resources.");

            // Get all SkinnedMeshRenderer components from the loaded GameObject
            var renderers = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            Assert.IsNotEmpty(renderers, "No SkinnedMeshRenderer components found on the avatar.");

            // Check each SkinnedMeshRenderer for non-null and non-empty bones array
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
            var avatar = Resources.Load<GameObject>("RPM_Template_Avatar");
            Assert.IsNotNull(avatar, "Failed to load 'RPM_Template_Avatar' from Resources.");

            // Get all SkinnedMeshRenderer components from the loaded GameObject
            var renderers = avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            Assert.IsNotEmpty(renderers, "No SkinnedMeshRenderer components found on the avatar.");

            // Check each SkinnedMeshRenderer for non-null and non-empty bones array
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
