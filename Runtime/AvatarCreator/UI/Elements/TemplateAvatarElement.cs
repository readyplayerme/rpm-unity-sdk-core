using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ReadyPlayerMe.AvatarCreator
{
    public class TemplateAvatarElement : MonoBehaviour
    {
        private const string TAG = nameof(TemplateAvatarElement);

        [Header("UI Elements")]
        [Space(5)]
        [SerializeField] private TemplateButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;

        [Header("Events")]
        [Space(5)]
        public UnityEvent<TemplateData> OnTemplateSelected;

        private List<TemplateData> avatarTemplateDataList;
        private TemplateButtonElement[] templateAvatarButtons;
        private TemplateFetcher templateFetcher;

        private void Awake()
        {
            templateFetcher = new TemplateFetcher();
        }

        public async void LoadAndCreateButtons()
        {
            await LoadTemplateRenders();
            CreateButtons();
        }

        public async void FetchTemplateData()
        {
            avatarTemplateDataList = await templateFetcher.GetTemplates();
            if (avatarTemplateDataList == null || avatarTemplateDataList.Count == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found");
            }
        }

        public async Task LoadTemplateRenders()
        {
            avatarTemplateDataList = await templateFetcher.GetTemplatesWithRenders();
        }

        public void CreateButtons()
        {
            if (avatarTemplateDataList.Count == 0)
            {
                SDKLogger.LogWarning(TAG, "No templates found. You need to load fetch the template data first.");
                return;
            }
            templateAvatarButtons = new TemplateButtonElement[avatarTemplateDataList.Count];
            for (var i = 0; i < avatarTemplateDataList.Count; i++)
            {
                var button = Instantiate(buttonElementPrefab, buttonContainer);
                button.SetTemplateData(avatarTemplateDataList[i]);
                button.AddListener(() => TemplateSelected(button));
                templateAvatarButtons[i] = button;
            }
        }

        private void ClearButtons()
        {
            foreach (var button in templateAvatarButtons)
            {
                Destroy(button.gameObject);
            }
            templateAvatarButtons = null;
        }

        private void TemplateSelected(TemplateButtonElement buttonElement)
        {
            OnTemplateSelected?.Invoke(buttonElement.TemplateData);
            SetButtonSelected(buttonElement);
        }

        public void SetButtonSelected(TemplateButtonElement buttonElement)
        {
            selectedIcon.transform.SetParent(buttonElement.transform);
            selectedIcon.transform.localPosition = Vector3.zero;
            selectedIcon.SetActive(true);
        }
    }
}
