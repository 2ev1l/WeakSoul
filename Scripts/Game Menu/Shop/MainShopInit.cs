using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu.Shop
{
    public class MainShopInit : ShopInit
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Start()
        {
            Init(GameData.Data.ShopData);
            base.Start();
        }
        protected override void OnEnable()
        {
            GameData.Data.OnDaysChanged += ReloadShop;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            GameData.Data.OnDaysChanged -= ReloadShop;
            base.OnDisable();
        }
        private void ReloadShop(int days)
        {
            if (days <= 0) return;
            ReloadShop();
        }
        public void ReloadShop()
        {
            GameData.Data.ShopData.GenerateItems();
            Start();
        }
        [ContextMenu("Generate new data")]
        private void GND()
        {
            ReloadShop();
        }
        #endregion methods
    }
}