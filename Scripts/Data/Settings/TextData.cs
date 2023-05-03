using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    public class TextData : MonoBehaviour
    {
        #region fields & properties
        public static TextData Instance { get; private set; }
        [field: SerializeField] public List<Font> Fonts { get; private set; }

        [SerializeField] private LanguageData LanguageData;
        public LanguageData LoadedData
        {
            get
            {
                loadedData = loadedData == null ? LanguageData : loadedData;
                return loadedData;
            }
            set => loadedData = value;
        }
        private LanguageData loadedData;

        [SerializeField] private Color colorPicker;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            LoadChoosedLanguage();
        }
        public IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            UpdateText();
        }
        private void OnEnable()
        {
            SettingsData.Data.LanguageSettings.OnSettingsChanged += UpdateText;
        }
        private void OnDisable()
        {
            SettingsData.Data.LanguageSettings.OnSettingsChanged -= UpdateText;
        }
        private void UpdateText()
        {
            LoadChoosedLanguage();
            FindObjectsOfType<LanguageLoader>(true).ToList().ForEach(x => x.Load());
            FindObjectsOfType<TextOutline>(true).ToList().ForEach(x => x.SetAll());
        }
        private void LoadChoosedLanguage()
        {
            try
            { LoadedData = SavingUtils.GetLanguage(); }
            catch
            {
                Debug.LogError("Error - Can't find a language. Settting English by default.");
                SettingsData.Data.LanguageSettings.ChoosedLanguage = "English";
            }
        }
        public LanguageData GetEnglishData() => LanguageData;
        #endregion methods
    }
}