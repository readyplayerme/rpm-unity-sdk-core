using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Core.Editor
{
    public class IntegrationGuide : EditorWindow
    {
        private const string INTEGRATION_GUIDE = "Integration Guide";
        private const string HEADER_LABEL = "HeaderLabel";
        private const string LOAD_AVATARS_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/load-avatars";
        private const string ADD_ANIMATION_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/animations";
        private const string AVATAR_CREATOR_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/avatar-creator";
        private const string OPTIMIZE_PERFORMANCE_URL = "https://docs.readyplayer.me/ready-player-me/integration-guides/unity/optimize";
        private const string QUICK_START = "QuickStart";
        private const string LOAD_AVATARS = "LoadAvatars";
        private const string ADD_ANIMATIONS = "AddAnimations";
        private const string INTEGRATE_AVATAR_CREATOR = "IntegrateAvatarCreator";
        private const string OPTIMIZE_THE_PERFORMANCE = "OptimizeThePerformance";

        [SerializeField] private VisualTreeAsset visualTreeAsset;

        [MenuItem("Ready Player Me/Integration Guide")]
        public static void ShowWindow()
        {
            var window = GetWindow<IntegrationGuide>();
            window.titleContent = new GUIContent(INTEGRATION_GUIDE);
            window.minSize = new Vector2(500, 530);
            AnalyticsEditorLogger.EventLogger.LogOpenIntegrationGuide();
        }

        public void CreateGUI()
        {
            visualTreeAsset.CloneTree(rootVisualElement);

            var headerLabel = rootVisualElement.Q<Label>(HEADER_LABEL);
            headerLabel.text = INTEGRATION_GUIDE;

            var quickStartButton = rootVisualElement.Q<VisualElement>(QUICK_START).Q<Button>();
            quickStartButton.clicked += OnOpenQuickStartButton;
            var loadAvatarsDocButton = rootVisualElement.Q<VisualElement>(LOAD_AVATARS).Q<Button>();
            loadAvatarsDocButton.clicked += () =>
            {
                AnalyticsEditorLogger.EventLogger.LogOpenAvatarDocumentation();
                OpenDocumentation(LOAD_AVATARS_URL);
            };

            var addAnimationDocButton = rootVisualElement.Q<VisualElement>(ADD_ANIMATIONS).Q<Button>();
            addAnimationDocButton.clicked += () =>
            {
                AnalyticsEditorLogger.EventLogger.LogOpenAnimationDocumentation();
                OpenDocumentation(ADD_ANIMATION_URL);
            };

            var integrateAvatarCreatorDocButton = rootVisualElement.Q<VisualElement>(INTEGRATE_AVATAR_CREATOR).Q<Button>();
            integrateAvatarCreatorDocButton.clicked += () =>
            {
                AnalyticsEditorLogger.EventLogger.LogOpenAvatarCreatorDocumentation();
                OpenDocumentation(AVATAR_CREATOR_URL);
            };

            var optimizePerformanceDocButton = rootVisualElement.Q<VisualElement>(OPTIMIZE_THE_PERFORMANCE).Q<Button>();
            optimizePerformanceDocButton.clicked += () =>
            {
                AnalyticsEditorLogger.EventLogger.LogOpenOptimizationDocumentation();
                OpenDocumentation(OPTIMIZE_PERFORMANCE_URL);
            };
        }

        private void OnOpenQuickStartButton()
        {
            Close();

            if (!new QuickStartHelper().Open())
            {
                EditorUtility.DisplayDialog(INTEGRATION_GUIDE, "No quick start sample found.", "OK");
            }
            AnalyticsEditorLogger.EventLogger.LogLoadQuickStartScene();
        }

        private void OpenDocumentation(string link)
        {
            Application.OpenURL(link);
        }
    }
}
