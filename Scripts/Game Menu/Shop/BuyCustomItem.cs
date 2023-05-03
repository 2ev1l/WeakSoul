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
    public class BuyCustomItem : MonoBehaviour, IPointerClickHandler
    {
        #region fields & properties
        public UnityEvent OnBought;

        public Wallet BuyPrice
        {
            get => buyPrice;
            set
            {
                buyPrice = value;
                CheckPrice();
            }
        }
        [SerializeField] protected Wallet buyPrice;
		public Wallet SellPrice
		{
			get => sellPrice;
			set
			{
				sellPrice = value;
				showPriceHelp.SellPrice = sellPrice;
			}
		}
		[SerializeField] protected Wallet sellPrice;
		[SerializeField] private ShowPriceHelp showPriceHelp;
        [SerializeField] private CursorChanger cursorChanger;
        [SerializeField] protected MaterialRaycastChanger materialRaycastChanger;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] protected Image image;
        protected bool canBuy { get; private set; } = false;
        #endregion fields & properties

        #region methods
        protected virtual void OnEnable()
        {
            GameData.Data.PlayerData.Wallet.OnSoulsChanged += CheckPrice;
            CheckPrice();
        }
        protected virtual void OnDisable()
        {
            GameData.Data.PlayerData.Wallet.OnSoulsChanged -= CheckPrice;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!canBuy || eventData.button != PointerEventData.InputButton.Left) return;
            Buy();
        }
        protected virtual void Buy()
        {
            GameData.Data.PlayerData.Wallet.DecreaseValues(BuyPrice);
            OnBought?.Invoke();
            DisableUI();
		}
        protected void DisableUI()
        {
			OnDisable();
			spriteRenderer.sprite = null;
			image.enabled = false;
			enabled = false;
		}
        private void CheckPrice(SoulType type) => CheckPrice();
        protected virtual void CheckPrice()
        {
            showPriceHelp.BuyPrice = BuyPrice;
            canBuy = BuyPrice.CanBuyThis();
            cursorChanger.enabled = canBuy;
            materialRaycastChanger.SetChangedMaterial(canBuy ? ShopInfo.Instance.GoodPrice : ShopInfo.Instance.BadPrice);
        }
        #endregion methods
    }
}