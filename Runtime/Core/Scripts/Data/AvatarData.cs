using UnityEngine;

namespace ReadyPlayerMe.Core
{
    public class AvatarData : MonoBehaviour
    {
        public string AvatarId;
        public AvatarMetadata AvatarMetadata;

        private SkinnedMeshRenderer[] meshes;

        private void Awake()
        {
            meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void OnDestroy()
        {
            foreach (var mesh in meshes)
            {
                var materials = mesh.sharedMaterials;

                foreach (var material in materials)
                {
                    if (material == null) continue;

                    foreach (var property in material.GetTexturePropertyNames())
                    {
                        Texture texture = material.GetTexture(property);

                        if (texture == null) continue;

                        Destroy(texture);
                    }
                    Destroy(material);
                }

                Destroy(mesh.sharedMesh);
            }
        }
    }
}
