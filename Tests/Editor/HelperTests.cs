using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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
    }
}
