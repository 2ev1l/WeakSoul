using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PlayerSoulsItem : SoulsItem
    {
        #region fields & properties
        protected override Wallet wallet => GameData.Data.PlayerData.Wallet;
        #endregion fields & properties

        #region methods

        #endregion methods

    }
}