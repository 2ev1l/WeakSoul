using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
    public class PhysicalStatsItemListEffects : PhysicalStatsItemList
    {
        #region fields & properties
        public Effect Effect
        {
            get => effect;
            set
            {
                effect = value;
                UpdateListData();
            }
        }
        private Effect effect;
        #endregion fields & properties

        #region methods
        protected override void GetStatsAndList()
        {
            Stats = Effect.Stats;
            StatsList = Stats?.GetEnabledStatsList();
        }
        #endregion methods
    }
}