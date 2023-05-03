using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu
{
    public class ShowPriceInventoryHelp : ShowPriceHelp
    {
        #region fields & properties
        [SerializeField] private InventoryCell inventoryCell;
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (!CanOpen || inventoryCell.ItemId == -1) return;
            Item item = ItemsInfo.Instance.GetItem(inventoryCell.ItemId);
            BuyPrice = item.Price;
            SellPrice = item.GetSellPrice();
            base.OpenPanel();
        }

        #endregion methods
    }
}