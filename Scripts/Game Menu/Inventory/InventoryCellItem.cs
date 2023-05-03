using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeakSoul.GameMenu.Inventory
{
    public class InventoryCellItem : CellItem
    {
        #region fields & properties
        [SerializeField] private InventoryCell iCell;
        #endregion fields & properties

        #region methods
        protected override void MoveItem()
        {
            if (CellController.CatchedCellIndex == 20)
                iCell.InventoryCellController.OnRemoveCatched?.Invoke(Cell.ItemId, Cell.Index);
            GameData.Data.PlayerData.Inventory.MoveItem(Cell.Index, CellController.CatchedCellIndex);
        }
        protected override bool IsCellAllowed(Cell cell)
        {
            int lastIndex = CellController.CatchedCellIndex;
            if (!ItemsInventory.EquipmentCells.Contains(lastIndex))
                return true;

            Armor armor = ItemsInfo.Instance.TryGetArmor(cell.ItemId);
            if (armor != null)
            {
                if (lastIndex == 16 && armor.ArmorType == ArmorType.Head)
                    return true;
                if (lastIndex == 18 && armor.ArmorType == ArmorType.Body)
                    return true;
                if (lastIndex == 19 && armor.ArmorType == ArmorType.Legs)
                    return true;
            }

            Weapon weapon = ItemsInfo.Instance.TryGetWeapon(cell.ItemId);
            if (weapon != null && lastIndex == 17 && weapon.IsPlayerClassAllowed())
                return true;

            return false;
        }
        #endregion methods

    }
}