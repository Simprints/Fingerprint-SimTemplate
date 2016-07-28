﻿using SimTemplate.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTemplate.Utilities
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IDictionary<Setting, SettingInfo> SETTINGS_LOOKUP =
            new Dictionary<Setting, SettingInfo>
            {
                {
                    Setting.ApiKey,
                    new SettingInfo(
                        () => { return Properties.Settings.Default.ApiKey; },
                        ValidateApiKey,
                        "Must be a valid, 32 character GUID")
                },
                {
                    Setting.RootUrl,
                    new SettingInfo(
                        () => { return Properties.Settings.Default.RootUrl; },
                        ValidateRootUrl,
                        "Must be a valid URL")
                },
            };

        #region ISettingsValidator

        object ISettingsManager.GetSetting(Setting setting)
        {
            CheckSupportedSetting(setting);
            return SETTINGS_LOOKUP[setting].GetCurrentValue();
        }

        bool ISettingsManager.ValidateCurrentSettings()
        {
            bool IsValid = true;
            foreach (KeyValuePair<Setting, SettingInfo> entry in SETTINGS_LOOKUP)
            {
                if(!entry.Value.Validate(entry.Value.GetCurrentValue()))
                {
                    IsValid = false;
                    break;
                }
            }
            return IsValid;
        }

        bool ISettingsManager.ValidateQuerySetting(Setting setting, object queryValue)
        {
            CheckSupportedSetting(setting);
            return SETTINGS_LOOKUP[setting].Validate(queryValue);
        }

        string ISettingsManager.SettingHelpText(Setting setting)
        {
            CheckSupportedSetting(setting);
            return SETTINGS_LOOKUP[setting].ValidationText;
        }

        #endregion

        #region Helper Methods

        private void CheckSupportedSetting(Setting setting)
        {
            IntegrityCheck.IsTrue(
                SETTINGS_LOOKUP.ContainsKey(setting),
                "Validator doesn't recognise setting {0}",
                setting);
        }

        #endregion

        #region Validation Methods

        private static bool ValidateApiKey(object apikey)
        {
            string apiKeyText = apikey as string;
            
            Guid result;
            return !Guid.TryParse(apiKeyText, out result);
        }

        private static bool ValidateRootUrl(object rootUrl)
        {
            string rootUrlText = rootUrl as string;

            Uri uriResult;
            return Uri.TryCreate(rootUrlText, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        #endregion

        public class SettingInfo
        {
            private readonly Func<object> m_GetSettingMethod;
            private readonly Func<object, bool> m_ValidateSettingMethod;
            private readonly string m_ValidationText;

            public object GetCurrentValue() { return m_GetSettingMethod.Invoke(); }

            public bool Validate(object value) { return m_ValidateSettingMethod.Invoke(value); }

            public string ValidationText { get { return m_ValidationText; } }

            public SettingInfo(
                Func<object> getSettingMethod,
                Func<object, bool> validateSettingMethod,
                string validationText)
            {
                m_GetSettingMethod = getSettingMethod;
                m_ValidateSettingMethod = validateSettingMethod;
                m_ValidationText = validationText;
            }
        }
    }
}