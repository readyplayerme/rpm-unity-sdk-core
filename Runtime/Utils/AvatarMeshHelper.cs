using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public static class AvatarMeshHelper
    {
        public static void TransferMesh(GameObject source, SkinnedMeshRenderer[] targetMeshes, Animator targetAnimator)
        {
            var sourceAnimator = source.GetComponentInChildren<Animator>();
            SkinnedMeshRenderer[] sourceMeshes = source.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (var i = 0; i < targetMeshes.Length; i++)
            {
                if (i >= sourceMeshes.Length)
                {
                    targetMeshes[i].enabled = false;
                    break;
                }
                Mesh mesh = sourceMeshes[i].sharedMesh;
                targetMeshes[i].sharedMesh = mesh;
                targetMeshes[i].enabled = true;
                Material[] materials = sourceMeshes[i].sharedMaterials;
                targetMeshes[i].sharedMaterials = materials;
            }

            Avatar avatar = sourceAnimator.avatar;
            targetAnimator.avatar = avatar;
        }
    }
}
