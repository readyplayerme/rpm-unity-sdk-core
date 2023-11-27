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
        [SerializeField] private ButtonElement buttonElementPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject selectedIcon;

        [Header("Events")]
        [Space(5)]
        public UnityEvent<TemplateData> onTemplateSelected;

        private List<TemplateData> avatarTemplateDataList;
        private ButtonElement[] templateAvatarButtons;
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
            templateAvatarButtons = new ButtonElement[avatarTemplateDataList.Count];
            for (var i = 0; i < avatarTemplateDataList.Count; i++)
            {
                var button = Instantiate(buttonElementPrefab, buttonContainer);
                var templateData = avatarTemplateDataList[i];
                button.SetIcon(templateData.Texture);
                button.AddListener(() => TemplateSelected(button.transform, templateData));
                templateAvatarButtons[i] = button;
            }
        }

        private void ClearButtons()
        {
            if (templateAvatarButtons == null) return;

            foreach (var button in templateAvatarButtons)
            {
                Destroy(button.gameObject);
            }
            templateAvatarButtons = null;
        }

        private void TemplateSelected(Transform buttonTransform, TemplateData templateData)
        {
            onTemplateSelected?.Invoke(templateData);
            SetButtonSelected(buttonTransform);
        }

        private void SetButtonSelected(Transform buttonTransform)
        {
            selectedIcon.transform.SetParent(buttonTransform);
            selectedIcon.transform.localPosition = Vector3.zero;
            selectedIcon.SetActive(true);
        }
    }
}
