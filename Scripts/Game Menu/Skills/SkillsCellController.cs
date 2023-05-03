using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Skills
{
    public class SkillsCellController : CellController
    {
        #region fields & properties
        public bool IsCellsFree { get; private set; }
        public int FreeCell { get; private set; }
        protected virtual SkillsInventory SkillsInventory => GameData.Data.PlayerData.Skills;
        #endregion fields & properties

        #region methods
        public override void Init()
        {

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            SkillsInventory.OnSizeChanged += UpdateCells;
            SkillsInventory.OnInventoryChanged += CheckEquippedCells;
            CheckEquippedCells();
            UpdateCells(SkillsInventory.Size);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            SkillsInventory.OnSizeChanged -= UpdateCells;
            SkillsInventory.OnInventoryChanged -= CheckEquippedCells;
        }
        protected override void ActivateMove(Cell cell)
        {
            base.ActivateMove(cell);
            Cells[6].SetActive(true);
        }
        protected override void DropMove(Cell cell)
        {
            base.DropMove(cell);
            Cells[6].SetActive(false);
        }
        private void CheckEquippedCells(int itemId, int newCellId, int oldCellId) => CheckEquippedCells();
        protected void CheckEquippedCells()
        {
            int size = SkillsInventory.Size;
            for (int i = 0; i < size; ++i)
            {
                if (SkillsInventory.GetItem(i) == -1)
                {
                    IsCellsFree = true;
                    FreeCell = i;
                    return;
                }
            }
            IsCellsFree = false;
            FreeCell = -1;
        }
        protected void UpdateCells() => UpdateCells(SkillsInventory.Size);
        protected override void UpdateCells(int inventorySize)
        {
            for (int i = 0; i < Cells.Length; ++i)
            {
                Cells[i].SetActive(false);
                Cells[i].SetActive(i < inventorySize);
            }
            Cells[6].SetActive(false);
        }
        #endregion methods
    }
}