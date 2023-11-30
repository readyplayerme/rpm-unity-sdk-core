using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    public class SelectionElement : MonoBehaviour
    {
        [Header("UI Elements")]
        [Space(5)]
        [SerializeField] private ButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;

        public ButtonElement CreateButton()
        {
            var button = Instantiate(buttonElementPrefab, buttonContainer);
            button.AddListener(() => SetButtonSelected(button.transform));
            return button;
        }

        public void ClearButtons()
        {
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Sets the position and parent of the SelectedIcon to indicate which button was last selected.
        /// </summary>
        /// <param name="button"></param>
        private void SetButtonSelected(Transform button)
        {
            selectedIcon.transform.SetParent(button);
            selectedIcon.transform.localPosition = Vector3.zero;
            selectedIcon.SetActive(true);
        }
    }
}
