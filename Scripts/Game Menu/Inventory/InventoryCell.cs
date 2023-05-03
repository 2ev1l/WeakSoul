using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeakSoul.GameMenu.Inventory
{
    public class InventoryCell : Cell
    {
        #region fields & properties
        protected override UnityAction<int, int, int> OnInventoryChanged
        {
            get => GameData.Data.PlayerData.Inventory.OnInventoryChanged;
            set => GameData.Data.PlayerData.Inventory.OnInventoryChanged = value;
        }
        public override CellController CellController => InventoryCellController;
        public override int ItemId => GameData.Data.PlayerData.Inventory.GetItem(Index);
        public override Sprite Texture => ItemsInfo.Instance.GetItem(ItemId).Texture;

        [SerializeField] private ShowItemHelp itemHelp;
        [field: SerializeField] public InventoryCellController InventoryCellController { get; private set; }
        #endregion fields & properties

        #region methods
        protected override void RenderCell()
        {
            base.RenderCell();
            itemHelp.ItemId = ItemId;
        }
        #endregion methods

    }
}