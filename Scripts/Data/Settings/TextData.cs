using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        [ContextMenu("Export language to txt")]
        private void ExportToTxt()
        {
            LanguageData data = LoadedData;
            string path = Application.persistentDataPath + "/export.txt";
            string text = "";
            text += "\n===Интерфейс===\n";
            text += "\n\n=MD=";
            text += GetTextFromArray(data.MenuData);
            text += "\n\n=GMD=\n";
            text += GetTextFromArray(data.GameMenuData);
            text += "\n\n=AD=\n";
            text += GetTextFromArray(data.AdventureData);
            text += "\n\n=ED=\n";
            text += GetTextFromArray(data.EventsData);
            text += "\n\n=TD=\n";
            text += GetTextFromArray(data.TutorialData);
            text += "\n\n=HD=\n";
            text += GetTextFromArray(data.HelpData);
            text += "\n===Повествование===\n";
            text += GetTextFromArray(data.CutSceneData);

            text += "\n===Названия предметов===\n";
            text += "\n\n=ID=\n";
            text += GetTextFromItemText(data.ItemsData);
            text += "\n\n=SD=\n";
            text += GetTextFromItemText(data.SkillsData);
            text += "\n\n=ED=\n";
            text += GetTextFromItemText(data.EffectsData);

            text += "\n===Описания действий===\n";
            text += "\n\n=CGD=\n";
            text += GetTextFromItemText(data.CardGroupsData);
            text += "\n\n=CD=\n";
            text += GetTextFromItemText(data.CardsData);

            text += "\n===Названия существ===\n";
            text += "\n\n=ED=\n";
            text += GetTextFromItemText(data.EnemiesData);
            File.WriteAllText(path, text);
        }
        private string GetTextFromItemText(List<ItemTextData> text)
        {
            string result = "";
            foreach(var el in text)
            {
                if (el.Name != "" && !el.Name[0].ToString().Equals("="))
                {
                    result += $"{el.Name}\n";
                }
                if (el.Description != "" && !el.Description[0].ToString().Equals("="))
                {
                    result += $"    {el.Description}\n";
                }
            }
            return result;
        }
        private string GetTextFromArray(string[] array)
        {
            string result = "";
            foreach (var el in array)
                result += $"{el}\n";
            return result;
        }
        #endregion methods
    }
}