using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PlayerSoulsItemList : SoulsItemList
    {
        #region fields & properties
        
        #endregion fields & properties

        #region methods

        private void OnEnable()
        {
            GameData.Data.PlayerData.Wallet.OnSoulsChanged += UpdateSouls;
            UpdateListData();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Wallet.OnSoulsChanged -= UpdateSouls;
        }
        private void UpdateSouls(SoulType soulType) => UpdateListData();
        #endregion methods
    }
}