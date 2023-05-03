using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class PercentItemList : ItemList
    {
        #region methods
        public List<int> List { get; } = new List<int> { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        public override void UpdateListData()
        {
            UpdateListDefault(List, x => x);
        }
        #endregion methods
    }
}