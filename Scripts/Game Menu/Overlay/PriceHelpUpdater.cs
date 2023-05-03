using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PriceHelpUpdater : HelpUpdater
    {
        #region fields & properties
        public static PriceHelpUpdater Instance { get; private set; }
        [SerializeField] private PriceSoulsItemList buyList;
        [SerializeField] private PriceSoulsItemList sellList;

        #endregion fields & properties

        #region methods
        public override void Init()
        {
            base.Init();
            Instance = this;
        }
        public void OpenPanel(Vector3 position, Wallet buyPrice, Wallet sellPrice)
        {
            base.OpenPanel(position);
            buyList.Wallet = buyPrice;
            sellList.Wallet = sellPrice;
            buyList.UpdateListData();
            sellList.UpdateListData();
        }
        #endregion methods
    }
}