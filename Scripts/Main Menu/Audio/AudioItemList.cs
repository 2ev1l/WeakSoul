using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace WeakSoul.MainMenu
{
    public class AudioItemList : PercentItemList
    {
        #region methods
        public override void UpdateListData()
        {
            base.UpdateListData();
            ShowAt((int)(SettingsData.Data.AudioSettings.AudioData.Volume * 100f));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            AudioTempSettings.Settings.AudioData.Volume = currentPositions.First().listParam / 100f;
        }
        #endregion methods
    }
}