using System;
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

        private void OnEnable()
        {
            maleButton.onClick.AddListener(MaleButtonClicked);
            femaleButton.onClick.AddListener(FemaleButtonClicked);
        }

        private void OnDisable()
        {
            maleButton.onClick.RemoveListener(MaleButtonClicked);
            femaleButton.onClick.RemoveListener(FemaleButtonClicked);
        }

        private void MaleButtonClicked()
        {
            OnGenderSelected?.Invoke(OutfitGender.Masculine);
        }

        private void FemaleButtonClicked()
        {
            OnGenderSelected?.Invoke(OutfitGender.Feminine);
        }
    }
}
