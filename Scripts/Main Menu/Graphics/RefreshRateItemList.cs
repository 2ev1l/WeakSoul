using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class RefreshRateItemList : ItemList
    {
        #region fields & properties
        public static List<int> RefreshRates { get; } = new List<int>() { 60, 75, 120, 144, 170, 270 };
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(RefreshRates, x => x);
            int index = RefreshRates.FindIndex(x => x == SettingsData.Data.GraphicsSettings.RefreshRate);
            if (index < 0)
                ShowAt(RefreshRates.First());
            else
                ShowAt(SettingsData.Data.GraphicsSettings.RefreshRate);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            GraphicsTempSettings.Settings.RefreshRate = currentPositions.First().listParam;
        }
        #endregion methods
    }
}