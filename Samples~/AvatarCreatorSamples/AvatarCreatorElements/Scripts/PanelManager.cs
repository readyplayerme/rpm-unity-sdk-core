using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples.AvatarCreatorElements
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private List<ButtonElementLink> buttonElementLinks;

        private void Awake()
        {
            foreach (var elementButtonLink in buttonElementLinks)
            {
                elementButtonLink.button.onClick.AddListener(() =>
                {
                    ShowElement(elementButtonLink.element);
                });
            }
        }

        private void ShowElement(GameObject element)
        {
            foreach (var elementSection in buttonElementLinks)
            {
                elementSection.element.SetActive(elementSection.element == element);
            }
        }
    }

    [Serializable]
    public struct ButtonElementLink
    {
        public Button button;
        public GameObject element;
    }
}
