﻿using System.Linq;
using ReadyPlayerMe.Core.Analytics;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    public class SubdomainField
    {
        private const string SUBDOMAIN_FIELD_CONTROL_NAME = "subdomain";
        private const string SUBDOMAIN_DOCS_LINK = "https://docs.readyplayer.me/ready-player-me/for-partners/partner-subdomains";
        private const string WEB_VIEW_PARTNER_SAVE_KEY = "WebViewPartnerSubdomainName";
        private const string ERROR_ICON_SEARCH_FILTER = "t:Texture rpm_error_icon";
        private const string DOMAIN_VALIDATION_ERROR = "Please enter a valid partner subdomain (e.g. demo). Click here to read more about this issue.";

        private string partnerSubdomain;
        private bool subdomainFocused;
        private string subdomainAfterFocus = string.Empty;

        public string PartnerSubdomain => partnerSubdomain;

        private GUIStyle textLabelStyle;
        private GUIStyle textFieldStyle;
        private GUIStyle errorButtonStyle;

        private Texture errorIcon;

        public SubdomainField()
        {
            partnerSubdomain = CoreSettingsHandler.CoreSettings.Subdomain ?? "demo";
            SaveSubdomain();
            LoadAssets();
        }

        public void SetSubdomain(string subdomain)
        {
            if (partnerSubdomain != subdomain)
            {
                partnerSubdomain = subdomain;
                SaveSubdomain();
            }
        }

        public void Draw()
        {
            LoadStyles();

            Layout.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Your subdomain:          https:// ", textLabelStyle, GUILayout.Width(176));
                var oldValue = partnerSubdomain;
                GUI.SetNextControlName(SUBDOMAIN_FIELD_CONTROL_NAME);
                partnerSubdomain = EditorGUILayout.TextField(oldValue, textFieldStyle, GUILayout.Width(128), GUILayout.Height(20));

                EditorGUILayout.LabelField(".readyplayer.me", textLabelStyle, GUILayout.Width(116), GUILayout.Height(20));
                var button = new GUIContent(errorIcon, DOMAIN_VALIDATION_ERROR);

                var isSubdomainValid = ValidateSubdomain();

                if (!isSubdomainValid)
                {
                    if (GUILayout.Button(button, errorButtonStyle))
                    {
                        Application.OpenURL(SUBDOMAIN_DOCS_LINK);
                    }

                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                }

                if (IsSubdomainFocusLost())
                {
                    SaveSubdomain();
                }
            });
        }

        private void LoadAssets()
        {
            if (errorIcon == null)
            {
                var assetGuid = AssetDatabase.FindAssets(ERROR_ICON_SEARCH_FILTER).FirstOrDefault();
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

                if (assetPath != null)
                {
                    errorIcon = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)) as Texture;
                }
            }
        }

        private void LoadStyles()
        {
            textFieldStyle ??= new GUIStyle(GUI.skin.textField);
            textFieldStyle.fontSize = 12;

            textLabelStyle ??= new GUIStyle(GUI.skin.label);
            textLabelStyle.fontStyle = FontStyle.Bold;
            textLabelStyle.fontSize = 12;

            errorButtonStyle ??= new GUIStyle();
            errorButtonStyle.fixedWidth = 20;
            errorButtonStyle.fixedHeight = 20;
            errorButtonStyle.margin = new RectOffset(2, 0, 2, 2);
        }

        private bool ValidateSubdomain()
        {
            partnerSubdomain ??= "demo";
            return !partnerSubdomain.All(char.IsWhiteSpace) && !partnerSubdomain.Contains('/') && !EditorUtilities.IsUrlShortcodeValid(partnerSubdomain);
        }

        private bool IsSubdomainFocusLost()
        {
            // focus changed from subdomain to another item
            if (GUI.GetNameOfFocusedControl() == string.Empty && subdomainFocused)
            {
                subdomainFocused = false;

                if (subdomainAfterFocus != partnerSubdomain)
                {
                    return true;
                }
            }
            if (GUI.GetNameOfFocusedControl() == SUBDOMAIN_FIELD_CONTROL_NAME && !subdomainFocused)
            {
                subdomainFocused = true;
                subdomainAfterFocus = partnerSubdomain;
            }

            return false;
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
