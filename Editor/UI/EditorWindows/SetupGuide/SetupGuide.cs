using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SetupGuide : EditorWindow
{
    private const string SETUP_GUIDE = "SetupGuide";
    private const string STUDIO_URL = "https://studio.readyplayer.me";
    private const string ANALYTICS_PRIVACY_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/help-us-improve-the-unity-sdk";
    private const string SUBDOMAIN_PANEL = "SubdomainPanel";
    private const string STUDIO_URL_LABEL = "StudioUrl";
    private const string SUBDOMAIN_FIELD = "SubdomainField";
    private const string USE_DEMO_SUBDOMAIN_TOGGLE = "UseDemoSubdomainToggle";
    private const string DEMO = "demo";
    private const string ANALYTICS_PANEL = "AnalyticsPanel";
    private const string ANALYTICS_ENABLED_TOGGLE = "AnalyticsEnabledToggle";
    private const string NEXT_BUTTON = "NextButton";
    private const string BACK_BUTTON = "BackButton";
    private const string FINISH_SETUP_BUTTON = "FinishSetupButton";

    [SerializeField] private VisualTreeAsset visualTreeAsset;

    private VisualElement[] panel;
    private VisualElement currentPanel;
    private int currentPanelIndex;

    private ObjectField avatarConfigField;

    private Button backButton;
    private Button nextButton;
    private Button finishSetupButton;
    private Button openQuickStartButton;

    [MenuItem("Ready Player Me/Re-run Setup")]
    public static void ShowWindow()
    {
        var window = GetWindow<SetupGuide>();
        window.titleContent = new GUIContent(SETUP_GUIDE);
        window.minSize = new Vector2(500, 400);
    }

    public void CreateGUI()
    {
        visualTreeAsset.CloneTree(rootVisualElement);
        panel = new[]
        {
            InitializeSubdomainPanel(),
            InitializeAnalyticsPanel()
        };

        InitializeFooter();
        StartStateMachine();
    }

    private VisualElement InitializeSubdomainPanel()
    {
        var subdomainPanel = rootVisualElement.Q<VisualElement>(SUBDOMAIN_PANEL);
        subdomainPanel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

        subdomainPanel.Q<Label>(STUDIO_URL_LABEL).RegisterCallback<MouseUpEvent>(x =>
        {
            Application.OpenURL(STUDIO_URL);
        });

        var subdomainTemplate = subdomainPanel.Q<SubdomainTemplate>();
        var subdomainField = subdomainTemplate.Q<TextField>(SUBDOMAIN_FIELD);

        var useDemoSubdomainToggle = subdomainPanel.Q<Toggle>(USE_DEMO_SUBDOMAIN_TOGGLE);
        if (CoreSettingsHandler.CoreSettings.Subdomain == DEMO)
        {
            useDemoSubdomainToggle.value = true;
            subdomainTemplate.SetFieldEnabled(false);
        }

        subdomainPanel.Q<Toggle>(USE_DEMO_SUBDOMAIN_TOGGLE).RegisterValueChangedCallback(x =>
        {
            if (x.newValue)
            {
                subdomainTemplate.SetSubdomain(DEMO);
                subdomainTemplate.SetFieldEnabled(false);
            }
            else
            {
                subdomainTemplate.SetFieldEnabled(true);
            }
        });

        subdomainField.RegisterValueChangedCallback(x =>
        {
            useDemoSubdomainToggle.SetValueWithoutNotify(x.newValue == DEMO);
        });

        return subdomainPanel;
    }

    private VisualElement InitializeAnalyticsPanel()
    {
        var analyticsPanel = rootVisualElement.Q<VisualElement>(ANALYTICS_PANEL);
        analyticsPanel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

        analyticsPanel.Q<Label>("PrivacyUrl").RegisterCallback<MouseUpEvent>(x =>
        {
            Application.OpenURL(ANALYTICS_PRIVACY_URL);
        });

        var analyticsEnabledToggle = analyticsPanel.Q<Toggle>(ANALYTICS_ENABLED_TOGGLE);
        analyticsEnabledToggle.SetValueWithoutNotify(AnalyticsEditorLogger.IsEnabled);
        analyticsEnabledToggle.RegisterValueChangedCallback(x =>
        {
            if (x.newValue)
            {
                AnalyticsEditorLogger.Enable();
            }
            else
            {
                AnalyticsEditorLogger.Disable();
            }
        });

        return analyticsPanel;
    }

    private void InitializeFooter()
    {
        nextButton = rootVisualElement.Q<Button>(NEXT_BUTTON);
        nextButton.clicked += NextPanel;

        backButton = rootVisualElement.Q<Button>(BACK_BUTTON);
        backButton.clicked += PreviousPanel;

        finishSetupButton = rootVisualElement.Q<Button>(FINISH_SETUP_BUTTON);
        finishSetupButton.clicked += ()=>
        {
            Close();
            IntegrationGuide.ShowWindow();
        };
    }


    private void PreviousPanel()
    {
        SetDisplay(panel[currentPanelIndex], false);
        currentPanelIndex--;
        SetDisplay(panel[currentPanelIndex], true);
        SwitchButtons();
    }

    private void NextPanel()
    {
        SetDisplay(panel[currentPanelIndex], false);
        currentPanelIndex++;
        SetDisplay(panel[currentPanelIndex], true);
        SwitchButtons();
    }

    private void SwitchButtons()
    {
        switch (currentPanelIndex)
        {
            case 0:
                SetVisibility(backButton, false);
                SetDisplay(nextButton, true);
                SetDisplay(finishSetupButton, false);
                break;
            case 1:
                SetVisibility(backButton, true);
                SetDisplay(nextButton, false);
                SetDisplay(finishSetupButton, true);
                break;
        }
    }

    private void OnOpenQuickStartButton()
    {
        Close();

        if (!new QuickStartHelper().Open())
        {
            EditorUtility.DisplayDialog(SETUP_GUIDE, "No quick start sample found.", "OK");
        }
    }

    private void StartStateMachine()
    {
        SetDisplay(panel[currentPanelIndex], true);
    }

    private void SetVisibility(VisualElement visualElement, bool enable)
    {
        visualElement.style.visibility = new StyleEnum<Visibility>(enable ? Visibility.Visible : Visibility.Hidden);
    }

    private void SetDisplay(VisualElement visualElement, bool enable)
    {
        visualElement.style.display = new StyleEnum<DisplayStyle>(enable ? DisplayStyle.Flex : DisplayStyle.None);
    }
}
