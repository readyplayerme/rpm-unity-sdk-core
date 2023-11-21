using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Core.Editor
{
    public class SetupGuide : EditorWindow
    {
        private const string SETUP_GUIDE = "Setup Guide";
        private const string HEADER_LABEL = "HeaderLabel";
        private const string STUDIO_URL = "https://studio.readyplayer.me?utm_source=unity-setup-guide";
        private const string ANALYTICS_PRIVACY_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/help-us-improve-the-unity-sdk";
        private const string SUBDOMAIN_PANEL = "SubdomainPanel";
        private const string STUDIO_URL_LABEL = "StudioUrl";
        private const string USE_DEMO_SUBDOMAIN_TOGGLE = "UseDemoSubdomainToggle";
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

        [MenuItem("Ready Player Me/Setup Guide", priority = 12)]
        public static void ShowWindow()
        {
            var window = GetWindow<SetupGuide>();
            window.titleContent = new GUIContent(SETUP_GUIDE);
            window.minSize = new Vector2(500, 380);
            AnalyticsEditorLogger.EventLogger.LogOpenSetupGuide();
        }

        public void CreateGUI()
        {
            visualTreeAsset.CloneTree(rootVisualElement);
            InitializeFooter();
            panel = new[]
            {
                InitializeSubdomainPanel(),
                InitializeAnalyticsPanel()
            };
            StartStateMachine();
        }

        private string currentSubdomain;
        private string currentAppId;

        private VisualElement InitializeSubdomainPanel()
        {
            var headerLabel = rootVisualElement.Q<Label>(HEADER_LABEL);
            headerLabel.text = SETUP_GUIDE;

            var subdomainPanel = rootVisualElement.Q<VisualElement>(SUBDOMAIN_PANEL);
            subdomainPanel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

            subdomainPanel.Q<Label>(STUDIO_URL_LABEL).RegisterCallback<MouseUpEvent>(x =>
            {
                Application.OpenURL(STUDIO_URL);
            });

            var subdomainTemplate = subdomainPanel.Q<SubdomainTemplate>();
            subdomainTemplate.OnSubdomainChanged += subdomain =>
            {
                currentSubdomain = subdomain;
                ToggleNextButton();
            };

            var appIdTemplate = subdomainPanel.Q<AppIdTemplate>();
            appIdTemplate.OnAppIdChanged += appId =>
            {
                currentAppId = appId;
                ToggleNextButton();
            };

            if (!ProjectPrefs.GetBool(USE_DEMO_SUBDOMAIN_TOGGLE) && CoreSettingsHandler.CoreSettings.Subdomain == CoreSettings.DEFAULT_SUBDOMAIN)
            {
                subdomainTemplate.ClearSubdomain();
                nextButton.SetEnabled(false);
            }
            var demoSubdomainToggle = subdomainPanel.Q<Toggle>(USE_DEMO_SUBDOMAIN_TOGGLE);
            demoSubdomainToggle.value = ProjectPrefs.GetBool(USE_DEMO_SUBDOMAIN_TOGGLE);
            subdomainTemplate.SetFieldEnabled(!ProjectPrefs.GetBool(USE_DEMO_SUBDOMAIN_TOGGLE));
            demoSubdomainToggle.RegisterValueChangedCallback(x =>
            {
                if (x.newValue)
                {
                    subdomainTemplate.SetDefaultSubdomain();
                    subdomainTemplate.SetFieldEnabled(false);
                    nextButton.SetEnabled(true);
                    ProjectPrefs.SetBool(USE_DEMO_SUBDOMAIN_TOGGLE, true);
                }
                else
                {
                    subdomainTemplate.ClearSubdomain();
                    subdomainTemplate.SetFieldEnabled(true);
                    nextButton.SetEnabled(false);
                    ProjectPrefs.SetBool(USE_DEMO_SUBDOMAIN_TOGGLE, false);
                }
            });

            return subdomainPanel;
        }

        private void ToggleNextButton()
        {
            if (!string.IsNullOrEmpty(currentAppId) && !string.IsNullOrEmpty(currentSubdomain))
            {
                nextButton.SetEnabled(true);
            }
            else
            {
                nextButton.SetEnabled(false);
            }
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
            finishSetupButton.clicked += () =>
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
}
