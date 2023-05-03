using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace Data
{
    [System.Serializable]
    public class WitchItem : ShopItem
    {
        #region fields & properties
        public override Sprite Texture => EffectsInfo.Instance.GetEffect(Id).Sprite;
        #endregion fields & properties

        #region methods
        public override bool CanBuyForType() => true;
        public override void BuyItem()
        {
            GameData.Data.PlayerData.Wallet.DecreaseValues(GetPrice());
            GameData.Data.WitchData.RemoveItem(Id);
            GameData.Data.PlayerData.Stats.TryAddOrStackEffect(Id);
            AudioManager.PlayClip(AudioStorage.Instance.BuySound, Universal.AudioType.Sound);
        }
        public override Wallet GetPrice() => EffectsInfo.Instance.GetEffect(Id).Price;
        public override Wallet GetSellPrice() => new Wallet();
        public WitchItem(int id, ShopItemType type = ShopItemType.Effect, int value = 1) : base(id, type, value) { }
        #endregion methods
    }
}