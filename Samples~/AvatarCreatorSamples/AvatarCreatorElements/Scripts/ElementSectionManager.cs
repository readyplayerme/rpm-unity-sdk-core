using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples.AvatarCreatorElements
{
    public class ElementSectionManager : MonoBehaviour
    {
        [SerializeField] private List<ElementSection> elementSections;

        private void Awake()
        {
            foreach (var elementSection in elementSections)
            {
                elementSection.button.onClick.AddListener(() =>
                {
                    ShowSection(elementSection.selectionElement);
                });
            }
        }

        private void ShowSection(GameObject section)
        {
            foreach (var elementSection in elementSections)
            {
                elementSection.selectionElement.SetActive(elementSection.selectionElement == section);
            }
        }
    }

    [Serializable]
    public struct ElementSection
    {
        public Button button;
        public GameObject selectionElement;
    }
}
