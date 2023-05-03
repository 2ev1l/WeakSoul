using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Events
{
    [System.Serializable]
    public class SpriteData
    {
        #region fields & properties
        public IEnumerable<Sprite> Texutres => textures;
        [SerializeField] private List<Sprite> textures;
        public List<SpawnSubZone> SubZones => subZones;
        [SerializeField] private List<SpawnSubZone> subZones;
        #endregion fields & properties

        #region methods
        public bool IsDataAllowed() => subZones.Contains(EventInfo.Instance.Data.SubZoneData.SubZone);
        #endregion methods
    }
}