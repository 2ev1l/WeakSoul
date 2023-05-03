using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Events
{
    public class EventState : StateChange
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        public Data.Events.EventType EventType => eventType;
        [SerializeField] private Data.Events.EventType eventType;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            panel.SetActive(active);
        }
        #endregion methods
    }
}