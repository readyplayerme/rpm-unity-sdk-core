using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace ReadyPlayerMe.Core.Tests
{
    public class TemplateAvatarTests
    {
        private const string XR_TEMPLATE_AVATAR = "RPM_Template_Avatar_XR";
        private const string TEMPLATE_AVATAR = "RPM_Template_Avatar";

        [Test]
        public Task Check_Template_Avatar_XR()
        {
            var avatar = Resources.Load<GameObject>(XR_TEMPLATE_AVATAR);
            Assert.IsNotNull(avatar, $"Failed to load '{XR_TEMPLATE_AVATAR}' from Resources.");

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
            var avatar = Resources.Load<GameObject>(TEMPLATE_AVATAR);
            Assert.IsNotNull(avatar, $"Failed to load '{TEMPLATE_AVATAR}' from Resources.");

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
