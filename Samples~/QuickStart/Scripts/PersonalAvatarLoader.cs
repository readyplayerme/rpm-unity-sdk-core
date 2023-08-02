using System;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples
{
    public class PersonalAvatarLoader : MonoBehaviour
    {
        [SerializeField] private Button openPersonalAvatarPanelButton;
        [SerializeField] private Text openPersonalAvatarPanelButtonText;
        [SerializeField] private GameObject avatarLoading;
        [SerializeField] private Button closeButton;
        [SerializeField] private Text linkText;
        [SerializeField] private Button linkButton;
        [SerializeField] private Button loadAvatarButton;
        [SerializeField] private InputField avatarUrlField;
        [SerializeField] private ThirdPersonLoader thirdPersonLoader;
        [SerializeField] private GameObject personalAvatarPanel;

        private string defaultButtonText;

        private void Start()
        {
            AnalyticsRuntimeLogger.EventLogger.LogRunQuickStartScene();
        }

        private void OnEnable()
        {
            openPersonalAvatarPanelButton.onClick.AddListener(OnOpenPersonalAvatarPanel);
            closeButton.onClick.AddListener(OnCloseButton);
            linkButton.onClick.AddListener(OnLinkButton);
            loadAvatarButton.onClick.AddListener(OnLoadAvatarButton);
            avatarUrlField.onValueChanged.AddListener(OnAvatarUrlFieldValueChanged);
        }

        private void OnDisable()
        {
            openPersonalAvatarPanelButton.onClick.RemoveListener(OnOpenPersonalAvatarPanel);
            closeButton.onClick.RemoveListener(OnCloseButton);
            linkButton.onClick.RemoveListener(OnLinkButton);
            loadAvatarButton.onClick.RemoveListener(OnLoadAvatarButton);
            avatarUrlField.onValueChanged.RemoveListener(OnAvatarUrlFieldValueChanged);
        }

        private void OnOpenPersonalAvatarPanel()
        {
            linkText.text = $"https://{CoreSettingsHandler.CoreSettings.Subdomain}.readyplayer.me";
            personalAvatarPanel.SetActive(true);
            AnalyticsRuntimeLogger.EventLogger.LogLoadPersonalAvatarButton();
        }

        private void OnCloseButton()
        {
            personalAvatarPanel.SetActive(false);
        }

        private void OnLinkButton()
        {
            Application.OpenURL(linkText.text);
        }

        private void OnLoadAvatarButton()
        {
            thirdPersonLoader.OnLoadComplete += OnLoadComplete;
            defaultButtonText = openPersonalAvatarPanelButtonText.text;
            openPersonalAvatarPanelButtonText.text = "Loading...";
            openPersonalAvatarPanelButton.interactable = false;
            avatarLoading.SetActive(true);
            thirdPersonLoader.LoadAvatar(avatarUrlField.text);
            personalAvatarPanel.SetActive(false);
            AnalyticsRuntimeLogger.EventLogger.LogPersonalAvatarLoading();
        }

        private void OnAvatarUrlFieldValueChanged(string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out Uri _))
            {
                loadAvatarButton.interactable = true;
            }
            else
            {
                loadAvatarButton.interactable = false;
            }
        }

        private void OnLoadComplete()
        {
            thirdPersonLoader.OnLoadComplete -= OnLoadComplete;
            openPersonalAvatarPanelButtonText.text = defaultButtonText;
            openPersonalAvatarPanelButton.interactable = true;
            avatarLoading.SetActive(false);
        }
    }
}
