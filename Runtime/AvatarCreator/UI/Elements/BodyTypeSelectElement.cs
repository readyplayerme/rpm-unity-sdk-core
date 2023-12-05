using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class BodyTypeSelectElement : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField, Tooltip("Button for selecting fullbody avatar body type.")]
        private Button fullbodyButton;

        [SerializeField, Tooltip("Button for selecting fullbody avatar body type.")]
        private Button halfbodyButton;

        [Space(5), Header("Events"), Tooltip("The event will be called when a body type is selected.")]
        public UnityEvent<BodyType> OnBodyTypeSelected;

        private void Awake()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            if (fullbodyButton != null)
            {
                fullbodyButton.onClick.AddListener(FullbodyButtonClicked);
            }
            if (halfbodyButton != null)
            {
                halfbodyButton.onClick.AddListener(HalfbodyButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (fullbodyButton != null)
            {
                fullbodyButton.onClick.RemoveListener(FullbodyButtonClicked);
            }
            if (halfbodyButton != null)
            {
                halfbodyButton.onClick.RemoveListener(HalfbodyButtonClicked);
            }
        }

        private void FullbodyButtonClicked()
        {
            OnBodyTypeSelected?.Invoke(BodyType.FullBody);
        }

        private void HalfbodyButtonClicked()
        {
            OnBodyTypeSelected?.Invoke(BodyType.HalfBody);
        }
    }
}
