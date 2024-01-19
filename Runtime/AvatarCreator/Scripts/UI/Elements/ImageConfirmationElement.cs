using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// A Unity MonoBehaviour class for displaying and confirming images.
    /// Allows users to set an image to be displayed and confirms it upon user interaction.
    /// </summary>
    public class ImageConfirmationElement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RawImage imageTextureTarget;

        [Header("Events")]
        public UnityEvent<Texture2D> onImageConfirmed;

        /// <summary>
        /// Sets the texture to be displayed in the element.
        /// </summary>
        /// <param name="texture">The Texture2D to be displayed.</param>
        public void SetTexture(Texture2D texture)
        {
            imageTextureTarget.texture = texture;
        }

        /// <summary>
        /// Confirms the currently displayed image and invokes the "onImageConfirmed" event with the confirmed texture.
        /// </summary>
        public void ConfirmPhoto()
        {
            onImageConfirmed?.Invoke((Texture2D) imageTextureTarget.texture);
        }
    }
}
