using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples.LegacyAvatarCreator
{
    public class AssetButton : MonoBehaviour
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private GameObject selected;
        [SerializeField] private Button button;
        [SerializeField] private GameObject mask;
        [SerializeField] private GameObject loading;

        public void AddListener(Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }

        public void SetColor(string colorHex)
        {
            ColorUtility.TryParseHtmlString(colorHex, out var color);
            icon.color = color;
        }

        public void SetIcon(Texture texture)
        {
            icon.texture = texture;
            icon.enabled = true;
            loading.SetActive(false);
        }

        public void SetSelect(bool isSelected)
        {
            selected.SetActive(isSelected);
        }

        private void EnableMask()
        {
            mask.GetComponent<Mask>().enabled = true;
            mask.GetComponent<Image>().color = Color.white;
        }
    }
}
