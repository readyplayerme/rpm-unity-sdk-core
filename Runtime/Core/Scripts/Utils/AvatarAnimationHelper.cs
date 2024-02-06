using UnityEngine;

namespace ReadyPlayerMe.Core
{
    /// <summary>
    /// This static class contains useful helper functions used in the <see cref="ReadyPlayerMe" /> API.
    /// </summary>
    public static class AvatarAnimationHelper
    {
        private const string TAG = nameof(AvatarProcessor);

        private const string ANIMATOR_CONTROLLER_NAME = "Animation/Avatar Animator";
        private const string MASCULINE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Masculine";
        private const string FEMININE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Feminine";
        private const string XR_MASCULINE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Masculine_XR";
        private const string XR_FEMININE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Feminine_XR";

        private static RuntimeAnimatorController animatorController;

        /// <summary>
        /// Loads and sets the <see cref="RuntimeAnimatorController" /> property of the <c>GameObjects</c>'s
        /// <see cref="Animator" /> component.
        /// </summary>
        /// <param name="bodyType">
        /// The avatar body type. E.g. <see cref="BodyType.FullBody" /> or <see cref="BodyType.HalfBody" />
        /// </param>
        /// <param name="avatar"></param>
        public static void SetupAnimator(BodyType bodyType, GameObject avatar)
        {
            if (bodyType != BodyType.FullBody || avatar == null)
            {
                return;
            }

            if (animatorController == null)
            {
                animatorController = Resources.Load<RuntimeAnimatorController>(ANIMATOR_CONTROLLER_NAME);
            }

            var animator = avatar.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = animatorController;
            }
        }

        public static Avatar GetAnimationAvatar(OutfitGender outfitGender, BodyType bodyType)
        {
            var path = GetAvatarPath(outfitGender, bodyType);
            if (path == null)
            {
                SDKLogger.LogWarning(TAG, $"Avatar path for body type {bodyType} and gender {outfitGender} not found.");
                return null;
            }

            var model = Resources.Load<GameObject>(path);
            if (model == null)
            {
                SDKLogger.LogWarning(TAG, $"Failed to load avatar model from path: {path}");
                return null;
            }

            if (model.TryGetComponent<Animator>(out Animator animator))
            {
                return animator.avatar;
            }
            SDKLogger.LogWarning(TAG, $"Animator component not found on model loaded from path: {path}");
            return null;
        }

        private static string GetAvatarPath(OutfitGender outfitGender, BodyType bodyType)
        {
            return bodyType switch
            {
                BodyType.FullBody => GetFullbodyAvatarPath(outfitGender),
                BodyType.XRFullbody => GetFullbodyXrAvatarPath(outfitGender),
                _ => null
            };
        }

        private static string GetFullbodyAvatarPath(OutfitGender outfitGender)
        {
            return outfitGender == OutfitGender.Masculine
                ? MASCULINE_ANIMATION_AVATAR_NAME
                : FEMININE_ANIMATION_AVATAR_NAME;
        }

        private static string GetFullbodyXrAvatarPath(OutfitGender outfitGender)
        {
            return outfitGender == OutfitGender.Masculine
                ? XR_MASCULINE_ANIMATION_AVATAR_NAME
                : XR_FEMININE_ANIMATION_AVATAR_NAME;
        }
    }
}
