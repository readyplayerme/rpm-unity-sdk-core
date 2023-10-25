using ReadyPlayerMe;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.UI;

public class LoginWithEmailSelection : State
{
    private const string TAG = nameof(LoginWithEmailSelection);

    [SerializeField] private Button sendActivationCodeButton;
    [SerializeField] private Button haveCodeButton;
    [SerializeField] private Button changeEmailButton;
    [SerializeField] private Button loginButton;

    [SerializeField] private InputField emailField;
    [SerializeField] private InputField codeField;

    [SerializeField] private GameObject emailPanel;
    [SerializeField] private GameObject codePanel;

    public override StateType StateType => StateType.LoginWithCodeFromEmail;
    public override StateType NextState => StateType.BodyTypeSelection;

    public override void ActivateState()
    {
        sendActivationCodeButton.onClick.AddListener(OnSendActivationCode);
        haveCodeButton.onClick.AddListener(OnHaveCodeButton);
        changeEmailButton.onClick.AddListener(OnChangeEmail);
        loginButton.onClick.AddListener(OnLogin);
    }

    public override void DeactivateState()
    {
        sendActivationCodeButton.onClick.RemoveListener(OnSendActivationCode);
        haveCodeButton.onClick.RemoveListener(OnHaveCodeButton);
        changeEmailButton.onClick.RemoveListener(OnChangeEmail);
        loginButton.onClick.RemoveListener(OnLogin);
    }

    private void OnSendActivationCode()
    {
        AuthManager.SendEmailCode(emailField.text);
        OnHaveCodeButton();
    }

    private void OnHaveCodeButton()
    {
        emailPanel.SetActive(false);
        codePanel.SetActive(true);
    }

    private void OnChangeEmail()
    {
        emailPanel.SetActive(true);
        codePanel.SetActive(false);
    }

    private async void OnLogin()
    {
        LoadingManager.EnableLoading("Signing In");

        AuthManager.OnSignInError += OnSignInError;

        if (await AuthManager.LoginWithCode(codeField.text))
        {
            OnChangeEmail();
            LoadingManager.DisableLoading();
            StateMachine.SetState(StateType.AvatarSelection);
            SDKLogger.Log(TAG, "Login successful");
        }
    }

    private void OnSignInError(string error)
    {
        AuthManager.OnSignInError -= OnSignInError;
        LoadingManager.EnableLoading(error, LoadingManager.LoadingType.Popup, false);
        SDKLogger.Log(TAG, $"Login failed with error: {error}");
    }
}
