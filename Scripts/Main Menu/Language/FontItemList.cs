using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class FontItemList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(TextData.Instance.Fonts, x => TextData.Instance.Fonts.IndexOf(x));
            ShowAt(SettingsData.Data.LanguageSettings.FontType);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            currentPositions.First().OnListUpdate(currentPositions.First().listParam);
            LanguageTempSettings.Settings.FontType = currentPositions.First().listParam;
        }
        #endregion methods
    }
}