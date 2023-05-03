using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace WeakSoul.MainMenu
{
    public class SoundItemList : PercentItemList
    {
        #region methods
        public override void UpdateListData()
        {
            base.UpdateListData();
            ShowAt((int)(SettingsData.Data.AudioSettings.SoundData.Volume * 100f));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            AudioTempSettings.Settings.SoundData.Volume = currentPositions.First().listParam / 100f;
        }
        #endregion methods
    }
}