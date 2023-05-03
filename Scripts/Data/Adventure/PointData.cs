using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.Adventure.Map;

namespace Data
{
    [System.Serializable]
    public class PointData
    {
        #region fields & properties
        public UnityAction OnOpened;
        public UnityAction OnEventChanged;
        public int PointId => pointId;
        [SerializeField][ReadOnly] private int pointId = -1;
        public ZoneData ChoosedZone => choosedZone;
        [SerializeField][ReadOnly] private ZoneData choosedZone = new();
        public MapEvent ChoosedEvent => choosedEvent;
        [SerializeField][ReadOnly] private MapEvent choosedEvent = new();
        public DirectionsInfo DirectionsInfo => directionsInfo;
        [SerializeField][ReadOnly] private DirectionsInfo directionsInfo = new();

        public Vector3 Position => position;
        [SerializeField][ReadOnly] private Vector3 position;
        public bool IsOpened => isOpened;
        [SerializeField][ReadOnly] private bool isOpened = false;
		#endregion fields & properties

		#region methods
		public void SetZone(ZoneData value) => choosedZone = value;
        /// <summary>
        /// Doesn't affect UI
        /// </summary>
        /// <param name="value"></param>
        public void SetEventHidden(MapEvent value)
        {
			choosedEvent = value;
		}
		/// <summary>
		/// Affect UI
		/// </summary>
		/// <param name="value"></param>
		public void SetEvent(MapEvent value)
		{
            SetEventHidden(value);
            OnEventChanged?.Invoke();
		}
		public void Open()
        {
            if (isOpened) return;
            isOpened = true;
            OnOpened?.Invoke();
        }
        public PointData(Point point)
        {
            this.position = point.transform.position;
            this.pointId = PointsInit.Instance.GetFreePointId();
        }
        public PointData() { }
        public PointData Clone()
        {
            PointData data = new()
            {
                pointId = pointId,
                choosedZone = choosedZone.Clone(),
                choosedEvent = choosedEvent.Clone(),
                directionsInfo = directionsInfo.Clone(),
                position = position,
                isOpened = isOpened
            };
            return data;
        }
        #endregion methods
    }
}