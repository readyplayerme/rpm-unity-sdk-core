using ReadyPlayerMe.Core.Analytics;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Core.Editor
{
    public class AppIdTemplate : VisualElement
    {
        private const string XML_PATH = "AppIdTemplate";

        public new class UxmlFactory : UxmlFactory<AppIdTemplate, UxmlTraits>
        {
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        private readonly TextField appIdField;

        private readonly string appId;

        public AppIdTemplate()
        {
            var visualTree = Resources.Load<VisualTreeAsset>(XML_PATH);
            visualTree.CloneTree(this);

            appId = CoreSettingsHandler.CoreSettings.AppId;
            appIdField = this.Q<TextField>("AppIdField");
            appIdField.value = appId;

            this.Q<Button>("AppIdHelpButton").clicked += OnAppIdHelpClicked;

            appIdField.RegisterCallback<FocusOutEvent>(OnAppIdFocusOut);
        }

        private static void OnAppIdHelpClicked()
        {
            AnalyticsEditorLogger.EventLogger.LogFindOutMore(HelpSubject.Subdomain);
            Application.OpenURL(Constants.Links.DOCS_PARTNERS_LINK);
        }

        private void OnAppIdFocusOut(FocusOutEvent _)
        {
            if (ValidateAppId())
            {
                SaveAppId();
            }
        }

        private bool ValidateAppId()
        {
            return !string.IsNullOrEmpty(appIdField.value);
        }

        private void SaveAppId()
        {
            var id = CoreSettingsHandler.CoreSettings.AppId;
            if (id != appId)
            {
                AnalyticsEditorLogger.EventLogger.LogUpdatePartnerURL(id, appIdField.value);
            }

            CoreSettingsHandler.SaveAppId(appIdField.value);
        }
    }
}
