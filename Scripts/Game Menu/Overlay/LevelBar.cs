using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class LevelBar : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private Text experienceText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text spText;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            PlayerStats stats = GameData.Data.PlayerData.Stats;
            stats.ExperienceLevel.OnExperienceChanged += ChangeProgress;
            stats.ExperienceLevel.OnLevelChanged += ChangeLevel;
            stats.OnSkillPointsChanged += ChangeSkillPoints;
            ChangeProgress(stats.ExperienceLevel.Experience);
            ChangeLevel(stats.ExperienceLevel.Level);
            ChangeSkillPoints(stats.SkillPoints);
        }
        private void OnDisable()
        {
            PlayerStats stats = GameData.Data.PlayerData.Stats;
            stats.ExperienceLevel.OnExperienceChanged -= ChangeProgress;
            stats.ExperienceLevel.OnLevelChanged -= ChangeLevel;
            stats.OnSkillPointsChanged -= ChangeSkillPoints;
        }
        private void ChangeProgress(int value)
        {
            int maxLevel = LevelsInfo.Instance.MaxLevel;
            int currentLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            if (maxLevel <= currentLevel)
            {
                progressBar.Progress = 1;
                experienceText.text = $"{value}/Inf";
            }
            else
            {
                int expToNext = LevelsInfo.Instance.GetLevel(currentLevel).ExpToNext;
                progressBar.Progress = value / (float)expToNext;
                experienceText.text = $"{value}/{expToNext}";
            }
        }
        private void ChangeLevel(int value) => levelText.text = $"#{value}";
        private void ChangeSkillPoints(int value) => spText.text = $"{value} SP";
        #endregion methods
    }
}