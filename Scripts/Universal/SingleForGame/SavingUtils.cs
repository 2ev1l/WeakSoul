using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Data;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class SavingUtils : Cryptography
    {
        #region fields & properties
        public static SavingUtils Instance { get; private set; }
        public static UnityAction OnBeforeSave;
        public static UnityAction OnAfterSave;
        public static UnityAction OnDataReset;
        public static UnityAction OnSettingsReset;
        public static string StreamingAssetsPath => Application.dataPath + "/StreamingAssets";
        public static string LanguagePath => Application.dataPath + "/StreamingAssets/Language";
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;

            CheckSaves();
            LoadAll();
        }
        private void CheckSaves()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, GameData.SaveName + GameData.SaveExtension)))
                ResetTotalProgress();
            if (!File.Exists(Path.Combine(Application.persistentDataPath, SettingsData.SaveName + SettingsData.SaveExtension)))
                ResetSettings();
        }

        public void Start()
        {
            SaveAll();
        }

        [ContextMenu("Save ENGLISH Language")]
        private void SaveLanguage()
        {
            LanguageData data = TextData.Instance.GetEnglishData();
            string json = JsonUtility.ToJson(data);
            string path = Path.Combine(LanguagePath, $"English.json");
            File.WriteAllText(path, json);
            Debug.Log(path + " saved");
        }
        public static List<string> GetLanguageNames()
        {
            var diInfo = new DirectoryInfo(LanguagePath);
            var filesInfo = diInfo.GetFiles("*.json");
            List<string> list = new List<string>();
            for (int i = 0; i < filesInfo.Length; i++)
                list.Add(filesInfo[i].Name.Remove(filesInfo[i].Name.Length - 5));
            return list;
        }
        public static LanguageData GetLanguage() => GetLanguage(SettingsData.Data.LanguageSettings.ChoosedLanguage);
        public static LanguageData GetLanguage(string lang)
        {
            string json = File.ReadAllText(Path.Combine(LanguagePath, $"{lang}.json"));
            LanguageData data = JsonUtility.FromJson<LanguageData>(json);
            return data;
        }

        private void SaveAll()
        {
            TrySaveGameData();
            SaveSettings();
        }
        public static bool CanSave() => GameData.CanSaveData && GameData.Data.TutorialData.IsCompleted;
        public static bool TrySaveGameData()
        {
            if (!CanSave()) return false;
            SaveGameData();
            return true;
        }
        public static void SaveGameData()
        {
            OnBeforeSave?.Invoke();
			string rawJson = JsonUtility.ToJson(GameData.Data);
            string json = Encrypt(rawJson);
            using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, GameData.SaveName + GameData.SaveExtension), FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, json);
                fs.Close();
            }
            OnAfterSave?.Invoke();
        }
        public static void SaveSettings()
        {
            SaveJson(SettingsData.Data, SettingsData.SaveName + SettingsData.SaveExtension);
        }
        public static void SaveJson<T>(T data, string saveName)
        {
            string json = JsonUtility.ToJson(data);
            string path = Path.Combine(Application.persistentDataPath, saveName);
            File.WriteAllText(path, json);
        }
        public static T LoadJson<T>(string saveName)
        {
            string path = Path.Combine(Application.persistentDataPath, saveName);
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        private void LoadAll()
        {
            LoadGameData();
            LoadSettings();
        }
        private static void LoadGameData()
        {
            string json;
            using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, GameData.SaveName + ".data"), FileMode.Open))
            {
                var bf = new BinaryFormatter();
                json = bf.Deserialize(fs).ToString();
                json = Decrypt(json);
                fs.Close();
            }
            GameData.Data = JsonUtility.FromJson<GameData>(json);
        }
        private static void LoadSettings()
        {
            SettingsData.Data = LoadJson<SettingsData>(SettingsData.SaveName + SettingsData.SaveExtension);
        }
        private void ResetSettings()
        {
            SettingsData.Data = new SettingsData();
            SaveSettings();
            OnSettingsReset?.Invoke();
            Debug.Log("Settings reset");
        }
        public static void ResetTotalProgress(bool doAction = true)
        {
            GameData oldData = GameData.Data;
            GameData.Data = new GameData();
            TryApplyNonModifyingParams(oldData);
			TutorialData.ResetProgresses();
			SaveGameData();
            if (doAction)
                OnDataReset?.Invoke();
            Debug.Log("Progress reset");
        }
		private void OnApplicationQuit()
        {
            SaveAll();
        }
        private static void TryApplyNonModifyingParams(GameData oldData)
        {
            if (oldData == null) return;
            IEnumerable<int> openedRecipes = oldData.BlacksmithData.OpenedRecipes;
            BlacksmithData bd = GameData.Data.BlacksmithData;
			foreach (var recipeId in openedRecipes)
				bd.TryOpenRecipe(recipeId);
		}
        #endregion methods
    }
}