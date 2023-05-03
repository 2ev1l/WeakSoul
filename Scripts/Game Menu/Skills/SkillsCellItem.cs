using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Skills
{
    public class SkillsCellItem : CellItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void MoveItem()
        {
            if (CellController.CatchedCellIndex == 6)
            {
                GameData.Data.PlayerData.Skills.RemoveItem(CellController.StartCell.Index);
                return;
            }

            GameData.Data.PlayerData.Skills.MoveItem(Cell.Index, CellController.CatchedCellIndex);
        }
        #endregion methods
    }
}