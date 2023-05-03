using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class TrainingData
    {
        #region fields & properties
        public StatExperienceLevel HealthLevel => healthLevel;
        [SerializeField] private StatExperienceLevel healthLevel = new(PhysicalStatsType.Health);
        public StatExperienceLevel DamageLevel => damageLevel;
        [SerializeField] private StatExperienceLevel damageLevel = new(PhysicalStatsType.Damage);
        public StatExperienceLevel DefenseLevel => defenseLevel;
        [SerializeField] private StatExperienceLevel defenseLevel = new(PhysicalStatsType.Defense);
        public StatExperienceLevel EvasionChanceLevel => evasionChanceLevel;
        [SerializeField] private StatExperienceLevel evasionChanceLevel = new(PhysicalStatsType.EvasionChance);
        public StatExperienceLevel CriticalChanceLevel => criticalChanceLevel;
        [SerializeField] private StatExperienceLevel criticalChanceLevel = new(PhysicalStatsType.CriticalChance);
        #endregion fields & properties

        #region methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public StatExperienceLevel GetLevelByType(PhysicalStatsType type) => type switch
        {
            PhysicalStatsType.Health => healthLevel,
            PhysicalStatsType.Damage => damageLevel,
            PhysicalStatsType.Defense => defenseLevel,
            PhysicalStatsType.EvasionChance => evasionChanceLevel,
            PhysicalStatsType.CriticalChance => criticalChanceLevel,
            _ => throw new System.NotImplementedException()
        };
        #endregion methods
    }
}