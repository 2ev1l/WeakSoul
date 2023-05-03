using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ZoneData
    {
        #region fields & properties
        public SpawnZone SpawnZone => spawnZone;
        [SerializeField] private SpawnZone spawnZone;
        public int Value => value;
        [SerializeField] private int value;
        #endregion fields & properties

        #region methods
        public ZoneData() { }
        public ZoneData(SpawnZone spawnZone, int value) { this.spawnZone = spawnZone; this.value = value;}
        public ZoneData Clone() => new(spawnZone, value);
        #endregion methods
    }
}