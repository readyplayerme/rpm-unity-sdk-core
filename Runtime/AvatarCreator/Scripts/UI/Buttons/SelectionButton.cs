using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// A basic button element with an icon and functionality to bind an action to the
    /// buttons onClick event.
    /// </summary>
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private RawImage rawImage;

        private RectTransform rawImageRectTransform;

        /// <summary>
        /// Adds a listener to the button's onClick event.
        /// </summary>
        /// <param name="action">A function to run when the button is clicked</param>
        public void AddListener(Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }

        /// <summary>
        /// Sets the icon on the rawImage component
        /// </summary>
        /// <param name="texture">The texture to be assigned to the RawImage component</param>
        /// <param name="keepAspectRatio">If true the icon will resize to match the texture's aspect ratio</param>
        public void SetIcon(Texture texture, bool keepAspectRatio = false)
        {
            if (rawImageRectTransform == null)
            {
                rawImageRectTransform = rawImage.GetComponent<RectTransform>();
            }

            var previousSize = rawImageRectTransform.sizeDelta;
            rawImage.texture = texture;

            if (keepAspectRatio)
            {
                var currentAspectRatio = previousSize.x / previousSize.y;
                var targetAspectRatio = texture.width / (float) texture.height;
                if (Mathf.Abs(currentAspectRatio - targetAspectRatio) > 0.01f)
                {
                    AdjustRectTransformToAspectRatio(texture);
                    return;
                }
            }
            rawImageRectTransform.sizeDelta = previousSize;
        }

        private void AdjustRectTransformToAspectRatio(Texture texture)
        {
            if (rawImage == null)
            {
                Debug.LogWarning("RawImage reference is missing.");
                return;
            }

            var sizeDelta = rawImage.rectTransform.sizeDelta;
            var targetAspectRatio = texture.width / (float) texture.height;

            var currentAspectRatio = sizeDelta.x / sizeDelta.y;
            if (currentAspectRatio > targetAspectRatio)
            {
                sizeDelta.y = sizeDelta.x / targetAspectRatio;
            }
            else
            {
                sizeDelta.x = sizeDelta.y * targetAspectRatio;
            }

            rawImage.rectTransform.sizeDelta = sizeDelta;
        }

        public void SetColor(string hexColor)
        {
            rawImage.color = ColorUtility.TryParseHtmlString(hexColor, out var color) ? color : Color.white;
        }
    }
}
