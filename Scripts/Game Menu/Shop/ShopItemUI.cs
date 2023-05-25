using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Shop
{
    public class ShopItemUI : MonoBehaviour, IPointerClickHandler
    {
        #region fields & properties
        public ShopItemType Type => type;
        [SerializeField] private ShopItemType type;
        protected ShopItem item;

        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected ShowItemHelp itemHelp;
        [SerializeField] protected ShowPriceHelp priceHelp;
        [SerializeField] protected MaterialRaycastChanger materialRaycastChanger;
        [SerializeField] protected Image image;
        [SerializeField] protected CursorChanger cursorChanger;

        protected bool canBuy;
        #endregion fields & properties

        #region methods
        public void Load(ShopItem item)
        {
            enabled = true;
            image.enabled = true;
            cursorChanger.enabled = true;
            this.item = item;
            spriteRenderer.sprite = item.Texture;
            OnLoad(item);
            CheckPrice();
        }
        protected virtual void OnLoad(ShopItem item) 
        {
            if (itemHelp != null)
                itemHelp.ItemId = item.Id;
            if (priceHelp != null)
            {
                priceHelp.BuyPrice = item.GetPrice();
                priceHelp.SellPrice = item.GetSellPrice();
            }
        }
        private void OnEnable()
        {
            if (item == null)
                return;
            GameData.Data.PlayerData.Wallet.OnSoulsChanged += CheckPrice;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckPrice;
            CheckPrice();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Wallet.OnSoulsChanged -= CheckPrice;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckPrice;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!canBuy || eventData.button != PointerEventData.InputButton.Left) return;
            Buy();
        }
        protected virtual void Buy()
        {
            item.BuyItem();
            DisableItem();
        }
        public void DisableItem()
        {
            OnDisable();
            canBuy = false;
            spriteRenderer.sprite = null;
            image.enabled = false;
            cursorChanger.enabled = false;
            OnDisableItem();
            enabled = false;
        }
        protected virtual void OnDisableItem() { }
        private void CheckPrice(int itemId, int newIndex, int oldIndex) => CheckPrice();
        private void CheckPrice(SoulType soulType) => CheckPrice();
        protected virtual void CheckPrice()
        {
            canBuy = item.CanBuy();
            CheckPriceUI();
        }
        protected virtual void CheckPriceUI()
        {
            cursorChanger.enabled = canBuy;
            materialRaycastChanger.SetChangedMaterial(canBuy ? ShopInfo.Instance.GoodPrice : ShopInfo.Instance.BadPrice);
        }
        #endregion methods
    }
}