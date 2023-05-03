using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Data
{
    [System.Serializable]
    public class ExperienceLevel
    {
        #region fields & properties
        public UnityAction<int> OnLevelChanged;
        public UnityAction<int> OnExperienceChanged;
        public int Level
        {
            get => level;
            set => SetLevel(value);
        }
        [SerializeField] private int level = 0;
        public int Experience
        {
            get => experience;
            set => SetExperience(value);
        }
        [SerializeField] private int experience = 0;
        public virtual List<Level> Levels => LevelsInfo.Instance.PlayerLevelsData;
        public virtual int MaxLevel => Levels.Last().Id;
        #endregion fields & properties

        #region methods
        private void SetLevel(int value)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException("Level");
            level = value;
            OnLevelChanged?.Invoke(value);
        }
        public void SetExperience(int value)
        {
            value = Mathf.Max(0, value);
            experience = value;
            CorrectExperienceLevel();
            OnExperienceChanged?.Invoke(experience);
        }
        private void CorrectExperienceLevel()
        {
            List<Level> Levels = this.Levels;
            while (MaxLevel >= Level)
            {
                int exp = 0;
                Level currentLevel = Levels.Find(x => x.Id == Level);
                Level nextLevel = Levels.Find(x => x.Id > Level);
                if (currentLevel == null)
                {
                    Debug.LogError($"Error - Can't find current level = {Level}. Fixing - Set to next near = {nextLevel.Id}");
                    currentLevel = nextLevel;
                    level = nextLevel.Id;
                    nextLevel = Levels.Find(x => x.Id > Level);
                }
                exp = currentLevel.ExpToNext;

                if (experience - exp >= 0)
                {
                    experience -= exp;
                    Level = nextLevel == null ? MaxLevel + 1 : nextLevel.Id;
                }
                else break;
            }
        }
        public ExperienceLevel Clone()
        {
            ExperienceLevel level = new ExperienceLevel();
            level.level = this.level;
            level.experience = experience;
            return level;
        }
        #endregion methods
    }
}