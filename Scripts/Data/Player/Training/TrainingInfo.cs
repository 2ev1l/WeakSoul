using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Data
{
    [System.Serializable]
    public class TrainingInfo
    {
        #region fields & properties
        [field: SerializeField] public List<TrainingLevel> HealthLevels;
        private List<Level> healthLevels;
        [field: SerializeField] public List<TrainingLevel> DamageLevels;
        private List<Level> damageLevels;
        [field: SerializeField] public List<TrainingLevel> DefenseLevels;
        private List<Level> defenseLevels;
        [field: SerializeField] public List<TrainingLevel> EvasionChanceLevels;
        private List<Level> evasionChanceLevels;
        [field: SerializeField] public List<TrainingLevel> CriticalChanceLevels;
        private List<Level> criticalChanceLevels;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            HealthLevels = HealthLevels.OrderBy(x => x.Id).ToList();
            DamageLevels = DamageLevels.OrderBy(x => x.Id).ToList();
            DefenseLevels = DefenseLevels.OrderBy(x => x.Id).ToList();
            EvasionChanceLevels = EvasionChanceLevels.OrderBy(x => x.Id).ToList();
            CriticalChanceLevels = CriticalChanceLevels.OrderBy(x => x.Id).ToList();

            healthLevels = new();
            damageLevels = new();
            defenseLevels = new();
            evasionChanceLevels = new();
            criticalChanceLevels = new();

            HealthLevels.ForEach(x => healthLevels.Add(x));
            DamageLevels.ForEach(x => damageLevels.Add(x));
            DefenseLevels.ForEach(x => defenseLevels.Add(x));
            EvasionChanceLevels.ForEach(x => evasionChanceLevels.Add(x));
            CriticalChanceLevels.ForEach(x => criticalChanceLevels.Add(x));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<TrainingLevel> GetLevelsByType(PhysicalStatsType type) => type switch
        {
            PhysicalStatsType.Health => HealthLevels,
            PhysicalStatsType.Damage => DamageLevels,
            PhysicalStatsType.Defense => DefenseLevels,
            PhysicalStatsType.EvasionChance => EvasionChanceLevels,
            PhysicalStatsType.CriticalChance => CriticalChanceLevels,
            _ => throw new System.NotImplementedException()
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TrainingLevel GetCurrentLevelByType(PhysicalStatsType type)
        {
            int index = GameData.Data.PlayerData.Stats.TrainingData.GetLevelByType(type).Level;
            try
            {
                return type switch
                {
                    PhysicalStatsType.Health => HealthLevels.Find(x => x.Id >= index),
                    PhysicalStatsType.Damage => DamageLevels.Find(x => x.Id >= index),
                    PhysicalStatsType.Defense => DefenseLevels.Find(x => x.Id >= index),
                    PhysicalStatsType.EvasionChance => EvasionChanceLevels.Find(x => x.Id >= index),
                    PhysicalStatsType.CriticalChance => CriticalChanceLevels.Find(x => x.Id >= index),
                    _ => throw new System.NotImplementedException()
                };
            }
            catch { return null; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TrainingLevel GetPreviousLevelByType(PhysicalStatsType type)
        {
            int index = GameData.Data.PlayerData.Stats.TrainingData.GetLevelByType(type).Level;
            try
            {
                return type switch
                {
                    PhysicalStatsType.Health => HealthLevels.FindLast(x => x.Id < index),
                    PhysicalStatsType.Damage => DamageLevels.FindLast(x => x.Id < index),
                    PhysicalStatsType.Defense => DefenseLevels.FindLast(x => x.Id < index),
                    PhysicalStatsType.EvasionChance => EvasionChanceLevels.FindLast(x => x.Id < index),
                    PhysicalStatsType.CriticalChance => CriticalChanceLevels.FindLast(x => x.Id < index),
                    _ => throw new System.NotImplementedException()
                };
            }
            catch { return null; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TrainingLevel GetLastLevelByType(PhysicalStatsType type)
        {
            return type switch
            {
                PhysicalStatsType.Health => HealthLevels.Last(),
                PhysicalStatsType.Damage => DamageLevels.Last(),
                PhysicalStatsType.Defense => DefenseLevels.Last(),
                PhysicalStatsType.EvasionChance => EvasionChanceLevels.Last(),
                PhysicalStatsType.CriticalChance => CriticalChanceLevels.Last(),
                _ => throw new System.NotImplementedException()
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Health, Damage, Defense, EvasionChance, CriticalChance</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<Level> GetLevelsByTypeBase(PhysicalStatsType type) => type switch
        {
            PhysicalStatsType.Health => healthLevels,
            PhysicalStatsType.Damage => damageLevels,
            PhysicalStatsType.Defense => defenseLevels,
            PhysicalStatsType.EvasionChance => evasionChanceLevels,
            PhysicalStatsType.CriticalChance => criticalChanceLevels,
            _ => throw new System.NotImplementedException()
        };
        #endregion methods
    }
}