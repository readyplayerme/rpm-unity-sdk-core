using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    public interface IAssetData
    {
        public string Id { get; set; }
        public AssetType AssetType { get; set; }
    }

    public abstract class SelectionElement : MonoBehaviour
    {
        [Header("UI Elements")]
        [Space(5)]
        [SerializeField] private ButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;
        public UnityEvent<IAssetData> onAssetSelected;
        private Dictionary<string, ButtonElement> buttonMap = new Dictionary<string, ButtonElement>();

        public ButtonElement CreateButton(string id)
        {
            var button = Instantiate(buttonElementPrefab, buttonContainer);
            button.name = id;
            buttonMap.Add(id, button);
            button.AddListener(() => SetButtonSelected(button.transform));
            return button;
        }

        public void ClearButtons()
        {
            foreach (var button in buttonMap)
            {
                Destroy(button.Value.gameObject);
            }
            buttonMap.Clear();
        }

        public void UpdateButtonIcon(string id, Texture texture)
        {
            if (buttonMap.ContainsKey(id))
            {
                buttonMap[id].SetIcon(texture);
            }
            else
            {
                Debug.LogWarning($"No button found with id {id}");
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

        protected virtual void AssetSelected(IAssetData assetData)
        {
            onAssetSelected?.Invoke(assetData);
        }
    }
}
