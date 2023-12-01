using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    public abstract class SelectionElement : MonoBehaviour
    {
        private const string TAG = nameof(TemplateSelectionElement);
        [Header("UI Elements")]
        [Space(5)]
        [SerializeField] private ButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;

        public UnityEvent<IAssetData> onAssetSelected;
        private readonly Dictionary<string, ButtonElement> buttonMap = new Dictionary<string, ButtonElement>();

        public void CreateButtons<T>(T[] assets, Action<ButtonElement, T> onButtonCreated = default) where T : IAssetData
        {
            if (assets.Length == 0)
            {
                SDKLogger.LogWarning(TAG, "No assets provided.");
                return;
            }

            for (int i = 0; i < assets.Length; i++)
            {
                var button = CreateButton(assets[i].Id);
                var asset = assets[i];
                button.AddListener(() => AssetSelected(asset));
                onButtonCreated?.Invoke(button, asset);
            }
        }

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

        public ButtonElement GetButton(string id)
        {
            if (buttonMap.TryGetValue(id, out ButtonElement value))
            {
                return value;
            }
            Debug.LogWarning($"No button found with id {id}");
            return null;
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
