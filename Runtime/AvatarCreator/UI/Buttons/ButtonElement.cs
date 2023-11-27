using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    [RequireComponent(typeof(Button))]
    public class ButtonElement : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private RawImage rawImage;

        public void AddListener(Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void SetIcon(Texture texture, bool sizeToParent = true)
        {
            rawImage.texture = texture;
            if (sizeToParent) rawImage.SizeToParent();
        }
    }
}
