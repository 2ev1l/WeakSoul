using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class LanguageSettings
    {
        #region fields & properties
        public UnityAction OnSettingsChanged;

        [SerializeField] private string choosedLanguage = "English";
        [SerializeField] private int fontType = 3;
        [SerializeField] private FontStyle fontStyle = FontStyle.Bold;
        [SerializeField] private float fontSpacing = 0.8f;

        public string ChoosedLanguage { get => choosedLanguage; set => ChangeLanguage(value); }
        public int FontType { get => fontType; set => ChangeType(value); }
        public FontStyle FontStyle { get => fontStyle; set => ChangeStyle(value); }
        public float FontSpacing { get => fontSpacing; set => ChangeSpacing(value); }
        #endregion fields & properties

        #region methods
        private void ChangeType(int fontType)
        {
            if (FontType < 0 || FontType >= TextData.Instance.Fonts.Count)
                throw new System.ArgumentOutOfRangeException("font type");
            this.fontType = fontType;
            OnSettingsChanged?.Invoke();
        }
        private void ChangeStyle(FontStyle fontStyle)
        {
            this.fontStyle = fontStyle;
            OnSettingsChanged?.Invoke();
        }
        private void ChangeSpacing(float fontSpacing)
        {
            if (FontSpacing < 0)
                throw new System.ArgumentOutOfRangeException("font spacing");
            this.fontSpacing = fontSpacing;
            OnSettingsChanged?.Invoke();
        }
        private void ChangeLanguage(string choosedLanguage)
        {
            try
            {
				this.choosedLanguage = choosedLanguage;
				OnSettingsChanged?.Invoke();
			}
            catch
            {
                Debug.LogError($"{choosedLanguage} isn't supported. Set to default - English");
                this.choosedLanguage = "English";
				OnSettingsChanged?.Invoke();
			}


		}
        #endregion methods
    }
}