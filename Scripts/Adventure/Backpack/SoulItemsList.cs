using Data;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Adventure.Backpack
{
    public class SoulItemsList : ItemList
    {
        #region fields & properties
        private List<Data.SoulItem> allowedItems = new();
        #endregion fields & properties

        #region methods
        private void GetAllowedItems()
        {
            allowedItems = new();
            foreach (int el in GameData.Data.PlayerData.Inventory.Items)
            {
                if (el == -1) continue;
                Data.SoulItem soulItem = ItemsInfo.Instance.TryGetSoulItem(el);
                if (soulItem != null && soulItem.CanUse)
                    allowedItems.Add(soulItem);
            }
        }
        private void OnEnable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckInventory;
            UpdateListData();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckInventory;
        }
        private void CheckInventory(int itemId, int newCellId, int oldCellId)
        {
            bool isRemoved = itemId == -1;
            if (!isRemoved)
                return;
            UpdateListData();
        }
        public override void UpdateListData()
        {
            GetAllowedItems();
            UpdateListDefault(allowedItems, x => x.Id);
        }
        #endregion methods
    }
}