using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace Data
{
    [System.Serializable]
    public class ShopCell : ShopItem
    {
        #region fields & properties
        public override Sprite Texture => CellsInfo.Instance.GetInfo(Id).Texture;
        #endregion fields & properties

        #region methods
        public override bool CanBuyForType() => CellsInfo.Instance.GetInfo(Id).GetLastAllowedValue() != null;
        public override void BuyItem()
        {
            GameData.Data.PlayerData.Wallet.DecreaseValues(GetPrice());
            GameData.Data.ShopData.RemoveItem(Id, Type);
            switch (Id)
            {
                case 0: GameData.Data.PlayerData.Inventory.Size += Value; break;
                case 1: GameData.Data.PlayerData.Skills.Size += Value; break;
                default: throw new System.NotImplementedException();
            }
            AudioManager.PlayClip(AudioStorage.Instance.BuySound, Universal.AudioType.Sound);
        }
        public override Wallet GetPrice() => CellsInfo.Instance.GetInfo(Id).GetLastAllowedValue().Price;
        public override Wallet GetSellPrice() => new Wallet();
        public ShopCell(int id, ShopItemType type, int value) : base(id, type, value) { }
        #endregion methods
    }
}