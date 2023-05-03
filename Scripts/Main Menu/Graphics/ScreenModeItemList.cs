using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class ScreenModeItemList : ItemList
    {
        #region fields & properties
        private static readonly List<FullScreenMode> ScreenModes = new List<FullScreenMode>()
        { FullScreenMode.FullScreenWindow, FullScreenMode.ExclusiveFullScreen, FullScreenMode.Windowed };
        public static List<string> ScreenModesText { get; } = new List<string>()
        { "Borderless", "Full Screen", "Windowed"};
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(ScreenModes, x => ScreenModes.IndexOf(x));
            int index = ScreenModes.FindIndex(x => x == Screen.fullScreenMode);
            if (index < 0)
                index = 0;
            ShowAt(index);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            GraphicsTempSettings.Settings.ScreenMode = ScreenModes[currentPositions.First().listParam];
        }
        #endregion methods
    }
}