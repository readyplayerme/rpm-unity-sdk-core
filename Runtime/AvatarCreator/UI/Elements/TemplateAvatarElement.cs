using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;

public class TemplateAvatarElement : MonoBehaviour
{
    private const string TAG = nameof(TemplateAvatarElement);

    [Header("UI Elements")]
    [Space(5)]
    [SerializeField] private TemplateAvatarButton buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject selectedIcon;

    [Header("Events")]
    [Space(5)]
    public UnityEvent<TemplateData> OnTemplateSelected;

    private List<TemplateData> avatarTemplateDataList;
    private TemplateAvatarButton[] templateAvatarButtons;
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
        templateAvatarButtons = new TemplateAvatarButton[avatarTemplateDataList.Count];
        for (var i = 0; i < avatarTemplateDataList.Count; i++)
        {
            var button = Instantiate(buttonPrefab, buttonContainer);
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

    private void TemplateSelected(TemplateAvatarButton button)
    {
        OnTemplateSelected?.Invoke(button.TemplateData);
        SetButtonSelected(button);
    }

    public void SetButtonSelected(TemplateAvatarButton button)
    {
        selectedIcon.transform.SetParent(button.transform);
        selectedIcon.transform.localPosition = Vector3.zero;
        selectedIcon.SetActive(true);
    }
}
