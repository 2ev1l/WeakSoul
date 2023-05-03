using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class StyleItemList : ItemList
    {
        #region methods
        public static readonly List<string> StylesText = new() { "N", "B", "I", "B&I" };
        public override void UpdateListData()
        {
            List<int> list = new List<int> { 0, 1, 2, 3 };
            UpdateListDefault(list, x => list.IndexOf(x));
            ShowAt((int)SettingsData.Data.LanguageSettings.FontStyle);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            currentPositions.First().OnListUpdate(currentPositions.First().listParam);
            LanguageTempSettings.Settings.FontStyle = (FontStyle)currentPositions.First().listParam;
        }
        #endregion methods
    }
}