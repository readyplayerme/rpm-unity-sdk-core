using UnityEngine;
using System.Collections.Generic;

namespace ReadyPlayerMe.Core
{
    public static class AvatarMeshHelper
    {
        /// <summary>
        ///     Transfers the mesh and material data from the source to the target avatar.
        /// </summary>
        /// <param name="source">Imported Ready Player Me avatar.</param>
        /// <param name="target">Photon Network Player Character.</param>
        public static void TransferMesh(GameObject source, GameObject target)
        {
            // store the relevant data of the source (downloaded) avatar
            var dataDict = new Dictionary<string, (Material, Mesh, Transform[])>();
            
            var sourceRenderers = source.GetComponentsInChildren<SkinnedMeshRenderer>();
            var targetRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
            
            foreach (var renderer in sourceRenderers)
            {
                dataDict.Add(renderer.name, (renderer.materials[0], renderer.sharedMesh, renderer.bones));
            }
            
            // transfer the data to the target skinning mesh renderers
            foreach (var renderer in targetRenderers)
            {
                if (dataDict.TryGetValue(renderer.name, out var data))
                {
                    var (sourceMaterial, sourceMesh, sourceBones) = data;
                    renderer.material = sourceMaterial;
                    renderer.sharedMesh = sourceMesh;
                    
                    // transfer the bone data
                    foreach (var targetBone in renderer.bones)
                    {
                        foreach (var sourceBone in sourceBones)
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
