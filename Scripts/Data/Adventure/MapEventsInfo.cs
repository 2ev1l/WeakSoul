using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    public class MapEventsInfo : SingleSceneInstance
    {
        #region fields & properties
        public static MapEventsInfo Instance { get; private set; }
        [SerializeField] private List<MapEvent> events;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        public MapEvent GetEvent(int eventId) => events[eventId];
        public List<MapEvent> GetEvents(SpawnZone spawnZone)
        {
            int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            return events.Where(x => x.SpawnZone == spawnZone && x.Level <= playerLevel).ToList();
        }

        [ContextMenu("Order")]
        private void Order()
        {
            events = events.OrderBy(x => x.Id).ToList();
        }
        #endregion methods
    }
}