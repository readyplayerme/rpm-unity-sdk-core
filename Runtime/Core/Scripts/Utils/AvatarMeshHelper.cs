using UnityEngine;
using System.Collections.Generic;

namespace ReadyPlayerMe.Core
{
    public static class AvatarMeshHelper
    {
        [Obsolete("Use TransferMesh(GameObject source, GameObject target) instead.")]
        public static void TransferMesh(GameObject source, SkinnedMeshRenderer[] targetMeshes, Animator targetAnimator)
        {
            TransferMesh(source, targetMeshes[0].transform.parent.gameObject);
        }
        
        /// <summary>
        ///     Transfers the mesh and material data from the source to the target avatar.
        /// </summary>
        /// <param name="source">Imported Ready Player Me avatar.</param>
        /// <param name="target">Photon Network Player Character.</param>
        public static void TransferMesh(GameObject source, GameObject target)
        {
            // store the relevant data of the source (downloaded) avatar
            var rendererDict = new Dictionary<string, SkinnedMeshRenderer>();
            
            var sourceRenderers = source.GetComponentsInChildren<SkinnedMeshRenderer>();
            var targetRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
            
            foreach (var renderer in sourceRenderers)
            {
                rendererDict.Add(renderer.name, renderer);
            }
            
            // transfer the data to the target skinning mesh renderers
            foreach (var renderer in targetRenderers)
            {
                if (rendererDict.TryGetValue(renderer.name, out var sourceRenderer))
                {
                    renderer.material = sourceRenderer.materials[0];
                    renderer.sharedMesh = sourceRenderer.sharedMesh;
                    
                    // transfer the bone data
                    foreach (var targetBone in renderer.bones)
                    {
                        foreach (var sourceBone in sourceRenderer.bones)
                        {
                            if (sourceBone.name == targetBone.name)
                            {
                                targetBone.position = sourceBone.position;
                                targetBone.rotation = sourceBone.rotation;
                                targetBone.localScale = sourceBone.localScale;
                                break;                            
                            }
                        }
                    }
                }
                else
                {
                    // remove the renderer if it's not found in the source avatar
                    Object.Destroy(renderer.gameObject);
                }
            }
            
            // transfer the animation avatar
            var sourceAnimator = source.GetComponentInChildren<Animator>();
            var targetAnimator = target.GetComponentInChildren<Animator>();
            targetAnimator.avatar = sourceAnimator.avatar;
        }
    }
}
