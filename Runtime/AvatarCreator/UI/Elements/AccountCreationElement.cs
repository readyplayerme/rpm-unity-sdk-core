using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AccountCreationElement : MonoBehaviour
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private Button sendEmailButton;
        [SerializeField] private Button continueWithoutSignupButton;

        public UnityEvent<string> onSendEmail;
        public UnityEvent onContinueWithoutSignup;

        private void Update()
        {
            var email = emailField.text;
            sendEmailButton.interactable = !string.IsNullOrEmpty(email) && ValidatorUtil.IsValidEmail(email);
        }

        private void OnEnable()
        {
            sendEmailButton.onClick.AddListener(OnSendEmailButton);
            continueWithoutSignupButton.onClick.AddListener(OnContinueWithoutSignupButton);
        }

        private void OnDisable()
        {
            sendEmailButton.onClick.RemoveListener(OnSendEmailButton);
            continueWithoutSignupButton.onClick.RemoveListener(OnContinueWithoutSignupButton);
        }

        private void OnSendEmailButton()
        {
            var email = emailField.text;
            onSendEmail?.Invoke(email);
            gameObject.SetActive(false);
        }

        private void OnContinueWithoutSignupButton()
        {
            onContinueWithoutSignup?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
