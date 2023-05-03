using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    [System.Serializable]
    public class ShopItem
    {
        #region fields & properties
        public int Id => id;
        [SerializeField] private int id;
        public int Value => value;
        [SerializeField] private int value;
        public ShopItemType Type => type;
        [SerializeField] private ShopItemType type;
        public virtual Sprite Texture => ItemsInfo.Instance.GetItem(Id).Texture;
        #endregion fields & properties

        #region methods
        public virtual bool CanBuyForType() => GameData.Data.PlayerData.Inventory.GetFreeCell() != -1;
        public bool CanBuy() => CanBuyForType() && GetPrice().CanBuyThis();

        /// <summary>
        /// Check with <see cref="CanBuyForSouls"/> before
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SoulType GetInsufficientSoulTypeToBuy()
        {
            Wallet playerWallet = GameData.Data.PlayerData.Wallet;
            Wallet price = GetPrice();
            if (playerWallet.WeakSouls < price.WeakSouls)
                return SoulType.Weak;
            if (playerWallet.NormalSouls < price.NormalSouls)
                return SoulType.Normal;
            if (playerWallet.StrongSouls < price.StrongSouls)
                return SoulType.Strong;
            if (playerWallet.UniqueSouls < price.UniqueSouls)
                return SoulType.Unique;
            if (playerWallet.LegendarySouls < price.LegendarySouls)
                return SoulType.Legendary;
            throw new System.NotImplementedException();
        }
        public virtual void BuyItem()
        {
            GameData.Data.PlayerData.Wallet.DecreaseValues(GetPrice());
            GameData.Data.ShopData.RemoveItem(this);
            GameData.Data.PlayerData.Inventory.SetItem(Id, GameData.Data.PlayerData.Inventory.GetFreeCell());
            AudioManager.PlayClip(AudioStorage.Instance.BuySound, Universal.AudioType.Sound);
        }
        public virtual Wallet GetPrice() => ItemsInfo.Instance.GetItem(Id).Price;
        public virtual Wallet GetSellPrice() => ItemsInfo.Instance.GetItem(Id).GetSellPrice();
        public ShopItem(int id, ShopItemType type, int value = 1)
        {
            this.id = id;
            this.value = value;
            this.type = type;
        }
        #endregion methods
    }
}