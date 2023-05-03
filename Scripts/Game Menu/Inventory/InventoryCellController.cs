using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System.Linq;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu.Inventory
{
    public class InventoryCellController : CellController
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> itemId; <see cref="{T1}"/> lastCellId;
        /// </summary>
        public UnityAction<int, int> OnRemoveCatched;
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.PlayerData.Inventory.OnSizeChanged += UpdateCells;
            UpdateCells(GameData.Data.PlayerData.Inventory.Size);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.PlayerData.Inventory.OnSizeChanged -= UpdateCells;
        }
        protected override void UpdateCells(int inventorySize)
        {
            for (int i = 0; i < Cells.Length; ++i)
            {
                Cells[i].SetActive(i < inventorySize || ItemsInventory.EquipmentCells.Contains(i) || i == 20);
            }
        }
        #endregion methods
    }
}