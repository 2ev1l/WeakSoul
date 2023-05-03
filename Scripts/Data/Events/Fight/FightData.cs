using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Events
{
    [System.Serializable]
    public class FightData
    {
        #region fields & properties
        public int EnemyId => enemyId;
        [Min(0)] [SerializeField] private int enemyId;
        public EnemyData EnemyData => EnemiesInfo.Instance.GetEnemy(enemyId);
        #endregion fields & properties

        #region methods
        public FightData(int enemyId)
        {
            this.enemyId = enemyId;
        }
        #endregion methods
    }
}