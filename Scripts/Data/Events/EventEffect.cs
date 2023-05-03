using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Events
{
    [System.Serializable]
    public class EventEffect
    {
        #region fields & properties
        public GameObject Prefab => prefab;
        [SerializeField] private GameObject prefab;
        public Vector3 Position => position;
        [SerializeField] private Vector3 position;
        public List<SpawnSubZone> SubZones => subZones;
        [SerializeField] private List<SpawnSubZone> subZones;
        #endregion fields & properties

        #region methods
        public bool IsDataAllowed() => subZones.Contains(EventInfo.Instance.Data.SubZoneData.SubZone);
        #endregion methods
    }
}