using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class SubZoneData
    {
        #region fields & properties
        public SpawnSubZone SubZone => subZone;
        [SerializeField] private SpawnSubZone subZone = SpawnSubZone.Hunger;
        public Vector2 LevelsScale => levelsScale;
        [SerializeField] private Vector2 levelsScale = new(0, 7);
        #endregion fields & properties

        #region methods
        public SubZoneData() { }
        public SubZoneData(SpawnSubZone subZone) 
        {
            this.subZone = subZone;
        }
        #endregion methods
    }
}