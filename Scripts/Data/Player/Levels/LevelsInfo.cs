using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Universal;

namespace Data
{
    public class LevelsInfo : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private int levelToSet;
        [SerializeField] private int expToSet;
        [SerializeField] private int spToSet;
        public static LevelsInfo Instance { get; private set; }
        public int MaxLevel => PlayerLevels.Count;
        [field: SerializeField] public List<LevelSO> PlayerLevels { get; private set; } = new();
        [field: SerializeField] public TrainingInfo TrainingLevels { get; private set; } = new();
        public List<Level> PlayerLevelsData { get; private set; } = new();
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            PlayerLevelsData = new();
            PlayerLevels.ForEach(x => PlayerLevelsData.Add(x.Level));
            TrainingLevels.Init();
        }
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += AddLevelReward;
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= AddLevelReward;
        }
        private void AddLevelReward(int level)
        {
            GetLevel(level - 1).AddReward();
        }
        public Level GetLevel(int id) => PlayerLevels[id].Level;

        [ContextMenu("Get all")]
        private void Get()
        {
            PlayerLevels = Resources.FindObjectsOfTypeAll<LevelSO>().OrderBy(x => x.Level.Id).ToList();
            foreach (var el in PlayerLevels)
            {
                if (PlayerLevels.Where(x => x.Level.Id == el.Level.Id).Count() > 1)
                    Debug.LogError($"Error level {el.Level.Id} at {el.name}");
            }
        }
        [ContextMenu("Set lvl")]
        private void setlvl()
        {
            int plvl = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            for (int i = plvl; i < levelToSet; i++)
                addlvl();
        }
        [ContextMenu("Add lvl")]
        private void addlvl() => GameData.Data.PlayerData.Stats.ExperienceLevel.Level++;

        [ContextMenu("Set exp")]
        private void setexp() => GameData.Data.PlayerData.Stats.ExperienceLevel.Experience = expToSet;

        [ContextMenu("Set sp")]
        private void setsp() => GameData.Data.PlayerData.Stats.SkillPoints = spToSet;
		#endregion methods
	}
}