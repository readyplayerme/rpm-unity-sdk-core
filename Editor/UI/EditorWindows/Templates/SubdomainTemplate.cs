using System;
using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using ReadyPlayerMe.Core.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.Core.Editor
{
    public class SubdomainTemplate : VisualElement
    {
        private const string XML_PATH = "SubdomainTemplate";
        private const string DOMAIN_VALIDATION_ERROR = "Please enter a valid partner subdomain (e.g. demo). Click here to read more about this issue.";
        private const string WEB_VIEW_PARTNER_SAVE_KEY = "WebViewPartnerSubdomainName";

        public new class UxmlFactory : UxmlFactory<SubdomainTemplate, UxmlTraits>
        {
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        private readonly TextField subdomainField;

        public event Action<string> OnSubdomainChanged;

        private string partnerSubdomain;
        private bool isSubdomainFieldEnabled;

        public SubdomainTemplate()
        {
            var visualTree = Resources.Load<VisualTreeAsset>(XML_PATH);
            visualTree.CloneTree(this);

            partnerSubdomain = CoreSettingsHandler.CoreSettings.Subdomain;
            subdomainField = this.Q<TextField>("SubdomainField");
            subdomainField.value = partnerSubdomain;

            var errorIcon = this.Q<VisualElement>("ErrorIcon");
            errorIcon.tooltip = DOMAIN_VALIDATION_ERROR;

            this.Q<Button>("SubdomainHelpButton").clicked += OnSubdomainHelpClicked;

            errorIcon.RegisterCallback<MouseUpEvent>(OnErrorIconClicked);

            subdomainField.RegisterValueChangedCallback(SubdomainChanged(errorIcon));
            subdomainField.RegisterCallback<FocusOutEvent>(OnSubdomainFocusOut);
        }

        public void SetDefaultSubdomain()
        {
            partnerSubdomain = CoreSettings.DEFAULT_SUBDOMAIN;
            subdomainField.SetValueWithoutNotify(partnerSubdomain);
            SaveSubdomain();
        }

        public void SetFieldEnabled(bool enabled)
        {
            isSubdomainFieldEnabled = enabled;
            subdomainField.SetEnabled(isSubdomainFieldEnabled);
        }

        private static void OnSubdomainHelpClicked()
        {
            AnalyticsEditorLogger.EventLogger.LogFindOutMore(HelpSubject.Subdomain);
            Application.OpenURL(Constants.Links.DOCS_PARTNERS_LINK);
        }

        private void OnErrorIconClicked(MouseUpEvent _)
        {
            Application.OpenURL(Constants.Links.DOCS_QUICKSTART_LINK);
        }

        private void OnSubdomainFocusOut(FocusOutEvent _)
        {
            if (ValidateSubdomain())
            {
                SaveSubdomain();
            }
        }

        private EventCallback<ChangeEvent<string>> SubdomainChanged(VisualElement errorIcon)
        {
            return changeEvent =>
            {
                partnerSubdomain = ExtractSubdomain(changeEvent.newValue);
                errorIcon.visible = !ValidateSubdomain();
                subdomainField.SetValueWithoutNotify(partnerSubdomain);
                OnSubdomainChanged?.Invoke(partnerSubdomain);
            };
        }

        private static string ExtractSubdomain(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                url = uri.Host;
            }
            var hostParts = url.Split('.');
            if (hostParts.Length > 1)
            {
                return hostParts[0];
            }
            return url;
        }

        private bool ValidateSubdomain()
        {
            return !partnerSubdomain.All(char.IsWhiteSpace) && !partnerSubdomain.Contains('/') && !EditorUtilities.IsUrlShortcodeValid(partnerSubdomain);
        }

        private void SaveSubdomain()
        {
            EditorPrefs.SetString(WEB_VIEW_PARTNER_SAVE_KEY, partnerSubdomain);
            var subDomain = CoreSettingsHandler.CoreSettings.Subdomain;
            if (subDomain != partnerSubdomain)
            {
                AnalyticsEditorLogger.EventLogger.LogUpdatePartnerURL(subDomain, partnerSubdomain);
            }

            CoreSettingsHandler.SaveSubDomain(partnerSubdomain);
        }
    }
}
