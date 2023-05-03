using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace WeakSoul.MainMenu
{
    public class MusicItemList : PercentItemList
    {
        #region methods
        public override void UpdateListData()
        {
            base.UpdateListData();
            ShowAt((int)(SettingsData.Data.AudioSettings.MusicData.Volume * 100f));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            AudioTempSettings.Settings.MusicData.Volume = currentPositions.First().listParam / 100f;
        }
        #endregion methods
    }
}