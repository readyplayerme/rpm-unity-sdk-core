using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class GenderSelectElement : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField, Tooltip("Button for selecting a male avatar")]
        private Button maleButton;

        [SerializeField, Tooltip("Button for selecting a female avatar")]
        private Button femaleButton;

        [Space(5)]
        [Header("Events")]
        public UnityEvent<OutfitGender> OnGenderSelected;

        private void Awake()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            if (maleButton != null)
            {
                maleButton.onClick.AddListener(() => OnGenderSelected?.Invoke(OutfitGender.Masculine));
            }

            if (femaleButton != null)
            {
                femaleButton.onClick.AddListener(() => OnGenderSelected?.Invoke(OutfitGender.Feminine));
            }
        }

        private void OnDestroy()
        {
            maleButton?.onClick.RemoveListener(() => OnGenderSelected?.Invoke(OutfitGender.Masculine));
            femaleButton?.onClick.RemoveListener(() => OnGenderSelected?.Invoke(OutfitGender.Feminine));
        }
    }
}
