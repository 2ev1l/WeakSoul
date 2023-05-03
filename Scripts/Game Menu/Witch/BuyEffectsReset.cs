using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.GameMenu.Witch
{
    public class BuyEffectsReset : BuyCustomItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void OnEnable()
        {
            GameData.Data.PlayerData.Stats.OnEffectRemoved += CheckPrice;
            GameData.Data.PlayerData.Stats.OnEffectAdded += CheckPrice;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            GameData.Data.PlayerData.Stats.OnEffectRemoved -= CheckPrice;
            GameData.Data.PlayerData.Stats.OnEffectAdded -= CheckPrice;
            base.OnDisable();
        }
        protected override void Buy()
        {
            GameData.Data.PlayerData.Wallet.DecreaseValues(BuyPrice);
            OnBought?.Invoke();
            GameData.Data.PlayerData.Stats.RemoveAllEffects(true);
            CheckPrice();
        }
        private void CheckPrice(Effect effect) => CheckPrice();
        protected override void CheckPrice()
        {
            buyPrice = new();
            List<Effect> effects = GameData.Data.PlayerData.Stats.Effects.ToList();
            foreach (var el in effects)
                BuyPrice.IncreaseValues(el.RemovePrice);
            base.CheckPrice();
            image.enabled = effects.Count != 0;
            materialRaycastChanger.SetChangedMaterial(canBuy ? MaterialsInfo.Instance.Overlay_Good : MaterialsInfo.Instance.Overlay_Bad);
        }
        #endregion methods
    }
}