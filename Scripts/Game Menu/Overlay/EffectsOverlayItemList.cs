using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class EffectsOverlayItemList : ItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.OnEffectAdded += UpdateListData;
            GameData.Data.PlayerData.Stats.OnEffectRemoved += UpdateListData;
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.OnEffectAdded -= UpdateListData;
            GameData.Data.PlayerData.Stats.OnEffectRemoved -= UpdateListData;
        }
        private void UpdateListData(Effect effect) => UpdateListData();
        public override void UpdateListData()
        {
            Clear();
            List<Effect> effects = GameData.Data.PlayerData.Stats.Effects.ToList();
            UpdateListDefault(effects, x => x.Id);
        }
        #endregion methods
    }
}