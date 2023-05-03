using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.MainMenu
{
    public class LanguageItemList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<string> list = SavingUtils.GetLanguageNames();
            UpdateListDefault(list, x => list.IndexOf(x));
            ShowAt(list.IndexOf(SettingsData.Data.LanguageSettings.ChoosedLanguage));
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            LanguageTempSettings.Settings.ChoosedLanguage = LanguageItem.LanguageNames[currentPositions.First().listParam];
        }
        #endregion methods
    }
}