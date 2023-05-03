using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Events
{
    [System.Serializable]
    public class CampEvent
    {
        #region fields & properties
        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public int Level => level;
        [Min(0)][SerializeField] private int level;
        public int LanguageDescriptionId => langaugeDescriptionId;
        [Min(0)][SerializeField] private int langaugeDescriptionId;
        public IEnumerable<StatScale> StatsScale => statsScale;
        [SerializeField] private List<StatScale> statsScale;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Don't modify <see cref="defaultStats"/>
        /// </summary>
        /// <returns>Increased stats</returns>
        public PhysicalStats GetBuff(PhysicalStats defaultStats)
        {
            PhysicalStats increased = new();
            foreach (var el in statsScale)
            {
                el.SetIncreasedValue(defaultStats);
                increased.IncreaseStatsByType(el.StatsType, el.IncreasedValue);
            }
            return increased;
        }
        public CampEvent Clone()
        {
            CampEvent clone = new();
            clone.langaugeDescriptionId = langaugeDescriptionId;
                clone.id = id;
            clone.level = level;
            clone.statsScale = new();
            foreach (var el in statsScale)
                clone.statsScale.Add(el.Clone());
            return clone;
        }
        #endregion methods
    }
}