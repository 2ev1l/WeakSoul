using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Inventory;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.GameMenu.Witch
{
    public class ShopWitchUI : ShopItemUI
    {
        #region fields & properties
        private WitchItem witchItem;
        [SerializeField] private ShowEffectHelp effectHelp;
        #endregion fields & properties

        #region methods
        protected override void OnLoad(ShopItem item)
        {
            base.OnLoad(item);
            witchItem = new WitchItem(item.Id, item.Type, item.Value);
            priceHelp.BuyPrice = witchItem.GetPrice();
            priceHelp.SellPrice = witchItem.GetSellPrice();
            effectHelp.Effect = EffectsInfo.Instance.GetEffect(item.Id);
            spriteRenderer.sprite = witchItem.Texture;
        }
        protected override void OnDisableItem()
        {
            effectHelp.Effect = null;
        }
        protected override void Buy()
        {
            witchItem.BuyItem();
            DisableItem();
        }
        protected override void CheckPrice()
        {
            canBuy = witchItem.CanBuy();
            CheckPriceUI();
        }
        protected override void CheckPriceUI()
        {
            cursorChanger.enabled = canBuy;
            materialRaycastChanger.SetChangedMaterial(canBuy ? MaterialsInfo.Instance.Overlay_Good : MaterialsInfo.Instance.Overlay_Bad);
        }
        #endregion methods
    }
}