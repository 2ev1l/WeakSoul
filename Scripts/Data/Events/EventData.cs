using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Universal;
using WeakSoul.Events.Treasure;

namespace Data.Events
{
    [System.Serializable]
    public class EventData
    {
        #region fields & properties
        public static UnityAction<MapEvent> OnEventLoading;
        public UnityAction<SubZoneData> OnSubZoneChanged;
        public UnityAction<MapEvent> OnMapEventChanged;
        public BattleData BattleData => battleData;
        [SerializeField] private BattleData battleData;
        public ShopData ShopData => shopData;
        [SerializeField] private ShopData shopData;
        public MapEvent Event => mapEvent;
        [SerializeField] private MapEvent mapEvent;
        public SubZoneData SubZoneData => subZoneData;
        [SerializeField] private SubZoneData subZoneData;
        #endregion fields & properties

        #region methods
        public void LoadEvent(int eventId)
        {
            SetMapEvent(MapEventsInfo.Instance.GetEvent(eventId));
            switch (Event.EventType)
            {
                case EventType.Fight: GenerateBattleData(); break;
                case EventType.Shop: GenerateShopData(); break;
                case EventType.Loot: break; //on scene
				case EventType.Blacksmith: break; //on scene
				case EventType.Teleport: break; //on scene
				case EventType.Camp: break; //on scene
				case EventType.Puzzle: break; //none
            }
            OnEventLoading?.Invoke(Event);
            SceneLoader.Instance.LoadSceneFade("Events", 2f);
        }
        public void GenerateBattleData() => battleData.GenerateData(subZoneData, mapEvent);
        private void GenerateShopData() => shopData.GenerateItems(1, 3, 1, true, 50, false, 1);

        public void SetMapEvent(MapEvent mapEvent)
        {
            this.mapEvent = mapEvent;
            OnMapEventChanged?.Invoke(mapEvent);
		}
        public void SetSubZone(SubZoneData subZone)
        {
            subZoneData = subZone;
            OnSubZoneChanged?.Invoke(subZone);
        }
        #endregion methods
    }
}