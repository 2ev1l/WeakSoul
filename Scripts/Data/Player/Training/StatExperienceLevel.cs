using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class StatExperienceLevel: ExperienceLevel
    {
        #region fields & properties
        public PhysicalStatsType StatsType => statsType;
        [SerializeField] private PhysicalStatsType statsType;
        public override List<Level> Levels => LevelsInfo.Instance.TrainingLevels.GetLevelsByTypeBase(statsType);
        #endregion fields & properties

        #region methods
        public StatExperienceLevel(PhysicalStatsType type)
        {
            statsType = type;
        }
        #endregion methods
    }
}