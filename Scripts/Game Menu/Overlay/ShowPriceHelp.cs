using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu
{
    public class ShowPriceHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => PriceHelpUpdater.Instance;
        public Wallet BuyPrice { get; set; } = new Wallet();
        public Wallet SellPrice { get; set; } = new Wallet();
        public bool CanOpen
        {
            get => canOpen;
            set => canOpen = value;
        }
        [SerializeField] private bool canOpen = true;
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (!canOpen) return;
            base.OpenPanel();
            PriceHelpUpdater.Instance.OpenPanel(Vector3.zero, BuyPrice, SellPrice);
        }

        #endregion methods
    }
}