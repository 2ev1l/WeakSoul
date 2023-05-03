using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class SoulsItemList : ItemList
    {
        #region fields & properties
        
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(SoulsInfo.Instance.Souls, x => (int)x.SoulType);
        }
        #endregion methods
    }
}