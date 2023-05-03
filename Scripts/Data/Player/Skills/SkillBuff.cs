using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class SkillBuff
    {
        #region fields & properties
        public IEnumerable<StatScale> StatsScale => statsScale;
        [Header("Stats")][SerializeField] private List<StatScale> statsScale = new();
        public DamageType DamageTypeScaler => damageTypeScaler;
        [SerializeField] private DamageType damageTypeScaler;

        public bool ActivateEvasion => activateEvasion;
        [Header("Activators")][SerializeField] private bool activateEvasion;
        public int Turns => turns;
		[Min(0)][SerializeField] private int turns = 1;
        #endregion fields & properties

        #region methods
        public StatScale GetStatScale(int id) => statsScale[id];
        public StatScale GetStatScale(PhysicalStatsType statType, ValueIncrease increase) => statsScale.Find(x => x.StatsType == statType && x.IncreaseType == increase);
        public void DecreaseTurns() => turns--;
        public void RemoveStatScale(StatScale statScale) => statsScale.Remove(statScale);
        public SkillBuff Clone()
        {
            SkillBuff skillBuff = new();
            skillBuff.damageTypeScaler = damageTypeScaler;
            skillBuff.activateEvasion = activateEvasion;
            List<StatScale> stats = new();
            statsScale.ForEach(x => stats.Add(x.Clone()));
            skillBuff.statsScale = stats;
            skillBuff.turns = turns;
            return skillBuff;
        }
        #endregion methods
    }
}