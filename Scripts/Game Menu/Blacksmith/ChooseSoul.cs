using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class ChooseSoul : ChooseItem
    {
        #region fields & properties
        [SerializeField] private SoulType soulType;
        [SerializeField] private MaterialRaycastChanger materialRaycastChanger;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private int value = 0;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            spriteRenderer.sprite = SoulsInfo.Instance.GetInfo(soulType).Sprite2x;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            GameData.Data.PlayerData.Wallet.OnSoulsChanged += CheckSouls;
            CheckSouls(soulType);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.PlayerData.Wallet.OnSoulsChanged -= CheckSouls;
        }
        private void CheckSouls(SoulType soulType)
        {
            if (soulType != this.soulType) return;
            value = GameData.Data.PlayerData.Wallet.GetSoulsByType(soulType);
            customButton.enabled = value > 0;
            materialRaycastChanger.enabled = value > 0;
            spriteRenderer.material = (value > 0 ? SoulsInfo.Instance.NormalMaterial : SoulsInfo.Instance.NullMaterial);
        }
        protected override void AddToCraft()
        {
            if (value == 0) return;
            Wallet playerWallet = GameData.Data.PlayerData.Wallet;
            playerWallet.SetSoulsByType(playerWallet.GetSoulsByType(soulType) - 1, soulType);
            GameData.Data.BlacksmithData.CurrentRecipe.SetItem((int)soulType, CraftingThing.Soul, CraftItem.ChoosedCellIndex);
            base.AddToCraft();
        }
        #endregion methods
    }
}