using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Camp
{
    public class CampInfo : SingleSceneInstance
    {
        #region fields & properties
        public static CampInfo Instance { get; private set; }
        public IEnumerable<CampEventSO> CampData => campData;
        [SerializeField] private List<CampEventSO> campData;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        public CampEvent GetCamp(int chestId) => campData[chestId].CampEvent;

        [ContextMenu("Get all")]
        private void GetAll()
        {
            campData = new();
            campData = Resources.FindObjectsOfTypeAll<CampEventSO>().OrderBy(x => x.CampEvent.Id).ToList();
            foreach (var el in campData)
            {
                if (campData.Where(x => x.CampEvent.Id == el.CampEvent.Id).Count() > 2)
                {
                    Debug.LogError($"Error {el.CampEvent.Id} Id at {el.name}");
                }
            }
        }

        #endregion methods
    }
}