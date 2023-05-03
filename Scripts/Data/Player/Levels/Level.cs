using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class Level
    {
        #region fields & properties
        public int Id => id;
        [Min(0)]
        [SerializeField] private int id;
        public int ExpToNext => expToNext;
        [Min(0)]
        [SerializeField] private int expToNext;
        public StatsReward Reward => reward;
        [SerializeField] private StatsReward reward = new();
        #endregion fields & properties

        #region methods
        public void AddReward()
        {
            PlayerData player = GameData.Data.PlayerData;
            player.Stats.SoulLife += reward.SoulLife;
            player.Stats.SkillPoints += reward.SkillPoints;
            player.Stats.IncreaseStatsHidden(reward.Stats);
        }
        #endregion methods
    }
}