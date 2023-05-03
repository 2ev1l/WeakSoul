using Data;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class LanguageTempSettings : MonoBehaviour
    {
        #region fields & properties
        public static LanguageSettings Settings { get; private set; } = new LanguageSettings();
        #endregion fields & properties

        #region methods
        public void ApplySettings()
        {
            SettingsData.Data.LanguageSettings.FontSpacing = Settings.FontSpacing;
            SettingsData.Data.LanguageSettings.ChoosedLanguage = Settings.ChoosedLanguage;
            SettingsData.Data.LanguageSettings.FontStyle = Settings.FontStyle;
            SettingsData.Data.LanguageSettings.FontType = Settings.FontType;
        }
        #endregion methods
    }
}