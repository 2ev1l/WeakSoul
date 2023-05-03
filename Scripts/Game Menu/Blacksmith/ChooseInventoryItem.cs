using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class ChooseInventoryItem : ChooseItem
    {
        #region fields & properties
        [SerializeField] private InventoryCell inventoryCell;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void AddToCraft()
        {
            if (inventoryCell.ItemId == -1) return;
            GameData.Data.BlacksmithData.CurrentRecipe.SetItem(inventoryCell.ItemId, CraftingThing.Item, CraftItem.ChoosedCellIndex);
            GameData.Data.PlayerData.Inventory.RemoveItem(inventoryCell.Index);
            base.AddToCraft();
        }
        #endregion methods
    }
}