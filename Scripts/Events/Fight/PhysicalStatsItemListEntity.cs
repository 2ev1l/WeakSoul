using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.Events.Fight
{
    public class PhysicalStatsItemListEntity : PhysicalStatsItemList
    {
        #region fields & properties
        public EntityStats EnitityStats
        {
            get => stats;
            set
            {
                stats = value;
                OnDisable();
                OnEnable();
                UpdateListData();
            }
        }
        [SerializeField][ReadOnly] EntityStats stats;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            stats.OnStatsChanged += UpdateListData;
        }
        private void OnDisable()
        {
            stats.OnStatsChanged -= UpdateListData;
        }
        protected override void GetStatsAndList()
        {
            Stats = EnitityStats;
            StatsList = Stats.GetStatsList();
        }
        #endregion methods
    }
}