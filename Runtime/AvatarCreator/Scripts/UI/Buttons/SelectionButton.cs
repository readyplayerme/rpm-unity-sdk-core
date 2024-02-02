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

            var previousSizeDelta = rawImageRectTransform.sizeDelta;
            rawImage.texture = texture;
            if (keepAspectRatio)
            {
                var rect = rawImageRectTransform.rect;
                var currentRatio = rect.size.x / rect.size.y;
                var targetAspectRatio = texture.width / (float) texture.height;
                if (Mathf.Abs(currentRatio - targetAspectRatio) > Mathf.Epsilon)
                {
                    AdjustRectTransformToAspectRatio(targetAspectRatio);
                    return;
                }
            }
            rawImageRectTransform.sizeDelta = previousSizeDelta;
        }

        private void AdjustRectTransformToAspectRatio(float targetAspectRatio)
        {
            if (rawImage == null)
            {
                Debug.LogWarning("RawImage reference is missing.");
                return;
            }
            var newSize = CalculateNewSize(rawImageRectTransform.rect.size, targetAspectRatio);
            rawImageRectTransform.pivot = new Vector2(0.5f, 0.5f);
            rawImageRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rawImageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rawImageRectTransform.sizeDelta = newSize;
        }

        private Vector2 CalculateNewSize(Vector2 currentSize, float targetAspectRatio)
        {
            var currentAspectRatio = currentSize.x / currentSize.y;

            if (currentAspectRatio > targetAspectRatio)
            {
                return new Vector2(currentSize.y * targetAspectRatio, currentSize.y);
            }
            return new Vector2(currentSize.x, currentSize.x / targetAspectRatio);
        }

        public void SetColor(string hexColor)
        {
            rawImage.color = ColorUtility.TryParseHtmlString(hexColor, out var color) ? color : Color.white;
        }
    }
}
