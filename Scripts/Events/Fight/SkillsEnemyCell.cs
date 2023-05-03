using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WeakSoul.GameMenu.Skills;

namespace WeakSoul.Events.Fight
{
    public class SkillsEnemyCell : SkillsCell
    {
        #region fields & properties
        [SerializeField] private EnemyCard card;
        protected override UnityAction<int, int, int> OnInventoryChanged
        {
            get => card.EnemyData.Skills.OnInventoryChanged;
            set => card.EnemyData.Skills.OnInventoryChanged = value;
        }
        public override int ItemId => card.EnemyData.Skills.GetItem(Index);
        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            try { base.OnEnable(); }
            catch { }
            card.OnCardInit += UpdateEvents;
        }
        protected override void OnDisable()
        {
            try { base.OnDisable(); }
            catch { }
            card.OnCardInit -= UpdateEvents;
        }
        private void UpdateEvents()
        {
            OnDisable();
            OnEnable();
        }
        #endregion methods
    }
}