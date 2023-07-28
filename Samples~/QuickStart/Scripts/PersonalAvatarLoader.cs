using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples
{
    public class PersonalAvatarLoader : MonoBehaviour
    {
        [SerializeField] private Button openPersonalAvatarPanelButton;
        [SerializeField] private Text openPersonalAvatarPanelButtonText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button linkButton;
        [SerializeField] private Button loadAvatarButton;
        [SerializeField] private InputField avatarUrlField;
        [SerializeField] private ThirdPersonLoader thirdPersonLoader;
        [SerializeField] private GameObject personalAvatarPanel;

        private string defaultButtonText;

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
            personalAvatarPanel.SetActive(true);
        }

        private void OnCloseButton()
        {
            personalAvatarPanel.SetActive(false);
        }

        private void OnLinkButton()
        {
            Application.OpenURL("https://readyplayer.me/");
        }

        private void OnLoadAvatarButton()
        {
            thirdPersonLoader.OnLoadComplete += OnLoadComplete;
            defaultButtonText = openPersonalAvatarPanelButtonText.text;
            openPersonalAvatarPanelButtonText.text = "Loading...";
            openPersonalAvatarPanelButton.interactable = false;
            thirdPersonLoader.LoadAvatar(avatarUrlField.text);
            personalAvatarPanel.SetActive(false);
        }

        private void OnLoadComplete()
        {
            thirdPersonLoader.OnLoadComplete -= OnLoadComplete;
            openPersonalAvatarPanelButtonText.text = defaultButtonText;
            openPersonalAvatarPanelButton.interactable = true;
        }

        private void OnAvatarUrlFieldValueChanged(string url)
        {
            loadAvatarButton.interactable = !string.IsNullOrEmpty(url);
        }
    }
}
