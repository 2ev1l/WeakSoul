using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu
{
    public class ShowItemHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => ItemHelpUpdater.Instance;
        public int ItemId
        {
            get => itemId;
            set => SetItemId(value);
        }
        [SerializeField] private int itemId;
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (itemId < 0)
                return;
            base.OpenPanel();
            ItemHelpUpdater.Instance.OpenPanel(Vector3.zero, ItemId);
        }
        private void SetItemId(int value)
        {
            if (value == itemId) return;
            if (value < -1)
                throw new System.ArgumentOutOfRangeException("item id");
            itemId = value;
        }
        #endregion methods
    }
}