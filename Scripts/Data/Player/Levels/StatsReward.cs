using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class StatsReward
    {
        #region fields & properties
        public int SkillPoints => skillPoints;
        [SerializeField] private int skillPoints;
        public int SoulLife => soulLife;
        [SerializeField] private int soulLife;

        public PhysicalStats Stats => stats;
        [SerializeField] private PhysicalStats stats = new();
        #endregion fields & properties

        #region methods
        public bool RewardIsZero() => soulLife == 0 && skillPoints == 0 && stats.IsStatsZero();
        public StatsReward() { }
        public StatsReward(int soulLife, int skillPoints, PhysicalStats stats)
        {
            this.soulLife = soulLife;
            this.skillPoints = skillPoints;
            this.stats = stats;
        }
        #endregion methods
    }
}