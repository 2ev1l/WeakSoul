using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.Adventure.Map
{
    [System.Serializable]
    public class DirectionInfo
    {
        #region fields & properties
        public Direction Direction => direction;
        [SerializeField] private Direction direction;
        public int PointId => pointId;
        [SerializeField] private int pointId = -1;
        public Point Point => PointsInit.Instance.GetPoint(pointId);
        #endregion fields & properties

        #region methods
        public DirectionInfo(Direction direction)
        {
            this.direction = direction;
        }
        public void SetPoint(int pointId)
        {
            if (pointId < -1) throw new System.ArgumentOutOfRangeException(nameof(pointId));
            this.pointId = pointId;
        }
        public void SetPoint(Point point) => SetPoint(point.Data.PointId);
        public DirectionInfo Clone()
        {
            DirectionInfo data = new(Direction)
            {
                pointId = PointId
            };
            return data;
        }
        #endregion methods
    }
}