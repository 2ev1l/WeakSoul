using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Shop
{
    public class ShopCellUI : ShopItemUI, IPointerClickHandler
    {
        #region fields & properties
        private ShopCell cell;
        [SerializeField] private LanguageLoader languageLoader;
        #endregion fields & properties

        #region methods
        protected override void OnLoad(ShopItem item)
        {
            cell = new ShopCell(item.Id, item.Type, item.Value);
            Wallet price = null;
            try { price = cell.GetPrice(); }
            catch
            {
                DisableItem();
                return;
            }
            base.OnLoad(item);
            priceHelp.BuyPrice = price;
            priceHelp.SellPrice = cell.GetSellPrice();
            spriteRenderer.sprite = cell.Texture;
            languageLoader.Id = item.Id switch
            {
                0 => 28,
                1 => 29,
                _ => throw new System.NotImplementedException(),
            };
        }
        protected override void OnDisableItem()
        {
            languageLoader.Id = -1;
        }
        protected override void Buy()
        {
            cell.BuyItem();
            DisableItem();
        }
        protected override void CheckPrice()
        {
            try { canBuy = cell.CanBuy(); }
            catch { canBuy = false; }
            CheckPriceUI();
        }
        #endregion methods
    }
}