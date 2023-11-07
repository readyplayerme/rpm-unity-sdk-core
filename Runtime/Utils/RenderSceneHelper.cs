using System.Collections.Generic;

namespace ReadyPlayerMe.Core
{
    public static class RenderSceneHelper
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var letters = str.ToCharArray();
            letters[0] = char.ToLower(letters[0]);
            return new string(letters);
        }

        public static Dictionary<RenderPose, string> RenderPoseMap = new Dictionary<RenderPose, string>()
        {
            { RenderPose.PowerStance, "power-stance" },
            { RenderPose.Relaxed, "relaxed" },
            { RenderPose.Standing, "standing" },
            { RenderPose.ThumbsUp, "thumbs-up" }
        };

        public static string GetSceneNameAsString(this AvatarRenderScene avatarRenderScene)
        {
            return RenderSceneMap[avatarRenderScene];
        }

        public static readonly Dictionary<AvatarRenderScene, string> RenderSceneMap = new Dictionary<AvatarRenderScene, string>
        {
            { AvatarRenderScene.FullbodyPortrait, "fullbody-portrait-v1" },
            { AvatarRenderScene.HalfbodyPortrait, "halfbody-portrait-v1" },
            { AvatarRenderScene.FullbodyPortraitTransparent, "fullbody-portrait-v1-transparent" },
            { AvatarRenderScene.HalfbodyPortraitTransparent, "halfbody-portrait-v1-transparent" },
            { AvatarRenderScene.FullBodyPostureTransparent, "fullbody-posture-v1-transparent" }
        };
    }
}
