using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PhysicalStatsItemList : ItemList
    {
        #region fields & properties
        public int ItemId
        {
            get => itemId;
            set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException("item id");
                if (value == itemId)
                    return;
                itemId = value;
                UpdateListData();
            }
        }
        private int itemId = -1;
        public PhysicalStats Stats { get; protected set; }
        public List<PhysicalStatsType> StatsList { get; protected set; }
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            Clear();
            GetStatsAndList();
            if (StatsList != null)
                UpdateListDefault(StatsList, x => (int)x);
        }
        protected virtual void GetStatsAndList()
        {
            ItemType itemType = ItemsInfo.Instance.GetItemType(itemId);
            if (itemType != ItemType.SoulItem)
            {
                Stats = ItemsInfo.Instance.GetPhysicalStats(itemId);
                StatsList = Stats?.GetEnabledStatsListInherit(itemType);
            }
            else
            {
                Stats = null;
                StatsList = null;
            }
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach (var el in currentPositions)
                el.rootObject.GetComponent<PhysicalStatsItem>().SetItemList(this);
        }
        #endregion methods
    }
}