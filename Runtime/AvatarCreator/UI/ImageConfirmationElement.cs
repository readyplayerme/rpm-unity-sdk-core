using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class ImageConfirmationElement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RawImage imageTextureTarget;

        [Header("Events")]
        public UnityEvent<Texture2D> onImageConfirmed;

        public void SetTexture(Texture2D texture)
        {
            imageTextureTarget.texture = texture;
        }
        
        public void ConfirmPhoto()
        {
            onImageConfirmed?.Invoke((Texture2D)imageTextureTarget.texture);
        }
    }
}
