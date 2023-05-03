using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class TrainingLevel: Level
    {
        #region fields & properties
        public float TimeDeviation => timeDeviation;
        [Min(0.01f)] [SerializeField] private float timeDeviation;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}