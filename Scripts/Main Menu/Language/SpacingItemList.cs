using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class SpacingItemList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<int> list = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            UpdateListDefault(list, x => x);
            ShowAt(Mathf.RoundToInt(SettingsData.Data.LanguageSettings.FontSpacing * 10f));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            LanguageTempSettings.Settings.FontSpacing = currentPositions.First().listParam / 10f;
        }
        #endregion methods
    }
}