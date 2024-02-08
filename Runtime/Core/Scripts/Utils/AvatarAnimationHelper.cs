using System.Collections.Generic;
using UnityEngine;

namespace ReadyPlayerMe.Core
{
    /// <summary>
    /// This static class contains useful helper functions for setting up Animators on Ready Player Me Avatars.
    /// </summary>
    public static class AvatarAnimationHelper
    {
        private const string TAG = nameof(AvatarProcessor);

        private const string MASCULINE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Masculine";
        private const string FEMININE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Feminine";
        private const string XR_MASCULINE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Masculine_XR";
        private const string XR_FEMININE_ANIMATION_AVATAR_NAME = "AnimationAvatars/Feminine_XR";

        private static readonly Dictionary<string, Avatar> AnimationAvatarCache = new Dictionary<string, Avatar>();

        public static void SetupAnimator(AvatarMetadata avatarMetadata, GameObject avatar)
        {
            var animator = avatar.GetComponent<Animator>();
            if (animator == null)
            {
                animator = avatar.AddComponent<Animator>();
            }
            SetupAnimator(avatarMetadata, animator);
        }

        public static void SetupAnimator(AvatarMetadata avatarMetadata, Animator animator)
        {
            animator.avatar = GetAnimationAvatar(avatarMetadata.OutfitGender, avatarMetadata.BodyType);
        }

        public static Avatar GetAnimationAvatar(OutfitGender outfitGender, BodyType bodyType)
        {
            var path = GetAvatarPath(outfitGender, bodyType);
            if (path == null)
            {
                return null;
            }

            if (!AnimationAvatarCache.TryGetValue(path, out var avatar))
            {
                var model = Resources.Load<GameObject>(path);
                if (model == null)
                {
                    return null;
                }

                if (model.TryGetComponent(out Animator animator))
                {
                    avatar = animator.avatar;
                    AnimationAvatarCache[path] = avatar;
                }
            }

            return avatar;
        }

        private static string GetAvatarPath(OutfitGender outfitGender, BodyType bodyType)
        {
            return bodyType switch
            {
                BodyType.FullBody => GetFullbodyAvatarPath(outfitGender),
                BodyType.FullBodyXR => GetFullbodyXrAvatarPath(outfitGender),
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
