using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System.Linq;
using UnityEngine.Events;

namespace Universal
{
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(Text))]
    public class TextOutline : MonoBehaviour
    {
        #region fields & properties
        private Outline Outline
        {
            get
            {
                outline = outline != null ? outline : GetComponent<Outline>();
                return outline;
            }
        }
        private Outline outline;
        private Text CurrentText
        {
            get
            {
                currentText = currentText != null ? currentText : GetComponent<Text>();
                return currentText;
            }
        }
        private Text currentText;
        private readonly float lineScale = 1.3f;

        [field: SerializeField] public bool UpdateFont { get; set; } = true;
        public bool UpdateFontStyle { get; set; } = true;
        public bool UpdateLineSpacing { get; set; } = true;
        #endregion fields & properties

        #region methods
        public IEnumerator Start()
        {
            if (UpdateFont) SetFont();
            if (UpdateFontStyle) SetStyle();
            if (UpdateLineSpacing) SetLineSpacing();
            yield return CustomMath.WaitAFrame();
            SetOutline();
        }
        public void SetAll()
        {
            SetOutline();
            if (UpdateFont) SetFont();
            if (UpdateFontStyle) SetStyle();
            if (UpdateLineSpacing) SetLineSpacing();
        }
        private void SetOutline() =>
            Outline.effectDistance = (CurrentText.cachedTextGenerator.fontSizeUsedForBestFit / 35f) * lineScale * Vector2.one;
        private void SetFont() =>
            CurrentText.font = TextData.Instance.Fonts[SettingsData.Data.LanguageSettings.FontType];
        private void SetStyle() =>
            CurrentText.fontStyle = SettingsData.Data.LanguageSettings.FontStyle;
        private void SetLineSpacing() =>
            CurrentText.lineSpacing = SettingsData.Data.LanguageSettings.FontSpacing;
        #endregion methods
    }
}