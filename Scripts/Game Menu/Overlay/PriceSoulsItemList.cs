using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PriceSoulsItemList : SoulsItemList
    {
        #region fields & properties
        public Wallet Wallet { get; set; }
        #endregion fields & properties

        #region methods
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach (var el in currentPositions)
            {
                el.rootObject.GetComponent<SoulsItem>().Wallet = Wallet;
                el.OnListUpdate(el.listParam);
            }
        }
        #endregion methods
    }
}