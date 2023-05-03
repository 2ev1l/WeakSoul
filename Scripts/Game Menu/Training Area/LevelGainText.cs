using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class LevelGainText : GainText
    {
        #region fields & properties
        protected override float removeTime => 2f;
        protected override float yIncrease => 100f;
        public PhysicalStatsType StatsType
        {
            get => statsType;
            set
            {
                statsType = value;
                S();
            }
        }
        private PhysicalStatsType statsType;
        [SerializeField] private LanguageLoader languageLoader;
        #endregion fields & properties

        #region methods
        protected override void Start() { }
        private void S()
        {
            base.Start();
            languageLoader.Id = PhysicalStats.GetStatsLanguageIdByType(statsType);
            languageLoader.AddText("+");
        }
        #endregion methods
    }
}