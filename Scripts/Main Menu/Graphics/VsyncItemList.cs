using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class VsyncItemList : ItemList
    {
        #region fields & properties
        public static List<bool> Vsync { get; } = new List<bool>() { false, true };
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(Vsync, x => Vsync.IndexOf(x));
            ShowAt(Vsync.FindIndex(x => x == SettingsData.Data.GraphicsSettings.Vsync));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            GraphicsTempSettings.Settings.Vsync = Vsync[currentPositions.First().listParam];
        }
        #endregion methods
    }
}