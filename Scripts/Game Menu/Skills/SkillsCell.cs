using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Skills
{
    public class SkillsCell : Cell
    {
        #region fields & properties
        protected override UnityAction<int, int, int> OnInventoryChanged
        {
            get => GameData.Data.PlayerData.Skills.OnInventoryChanged;
            set => GameData.Data.PlayerData.Skills.OnInventoryChanged = value;
        }
        public override CellController CellController => cellController;
        [SerializeField] private CellController cellController;
        public override int ItemId => GameData.Data.PlayerData.Skills.GetItem(Index);
        public override Sprite Texture => SkillsInfo.Instance.GetSkill(ItemId).Texture;
        [SerializeField] private ShowSkillHelp help;
        #endregion fields & properties

        #region methods
        protected override void RenderCell()
        {
            base.RenderCell();
            help.SkillId = ItemId;
        }
        #endregion methods
    }
}