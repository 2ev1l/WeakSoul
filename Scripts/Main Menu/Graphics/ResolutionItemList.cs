using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class ResolutionItemList : ItemList
    {
        #region fields & properties
        public static List<Resolution> Resolutions
        {
            get
            {
                resolutions ??= GetResolutions();
                return resolutions;
            }
        }
        private static List<Resolution> resolutions;
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(Resolutions, x => Resolutions.IndexOf(x));
            int index = Resolutions.FindIndex(x => x.width == SettingsData.Data.GraphicsSettings.Resolution.width && x.height == SettingsData.Data.GraphicsSettings.Resolution.height);
            if (index < 0)
                index = Resolutions.Count - 1;
            ShowAt(index);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            Resolution currentRes = Resolutions[currentPositions.First().listParam];
            SimpleResolution tempRes = GraphicsTempSettings.Settings.Resolution;
            tempRes.width = currentRes.width;
            tempRes.height = currentRes.height;
            GraphicsTempSettings.Settings.Resolution = tempRes;
        }
        private static List<Resolution> GetResolutions()
        {
            List<Resolution> resolutions = new();
            foreach (var el in Screen.resolutions)
            {
                if (resolutions.FindIndex(x => x.height == el.height && x.width == el.width) > -1) continue;
                resolutions.Add(el);
            }
            return resolutions;
        }
        #endregion methods
    }
}