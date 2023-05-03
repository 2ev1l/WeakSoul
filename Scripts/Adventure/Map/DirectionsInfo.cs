using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace WeakSoul.Adventure.Map
{
    [System.Serializable]
    public class DirectionsInfo
    {
        #region fields & properties
        private static readonly float step = 0.7f;

        [SerializeField]
        private List<DirectionInfo> directions = new()
        {
            new DirectionInfo(Direction.N),
            new DirectionInfo(Direction.S),
            new DirectionInfo(Direction.W),
            new DirectionInfo(Direction.E),
            new DirectionInfo(Direction.NW),
            new DirectionInfo(Direction.NE),
            new DirectionInfo(Direction.SW),
            new DirectionInfo(Direction.SE)
        };
        #endregion fields & properties

        #region methods
        public void SetDirection(Direction direction, Point point) => SetDirection(direction, point.Data.PointId);
        public void SetDirection(Direction direction, int pointId)
        {
            int index = directions.FindIndex(x => x.Direction == direction);
            directions[index].SetPoint(pointId);
        }
        public void LinkDirection(Direction direction, DirectionInfo directionInfo)
        {
            int index = directions.FindIndex(x => x.Direction == direction);
            directions[index] = directionInfo;
        }

        public DirectionInfo GetDirection(Point point) => directions.Find(x => x.PointId == point.Data.PointId);
        public DirectionInfo GetDirection(Direction direction) => directions.Find(x => x.Direction == direction);
        public DirectionInfo GetNonWaterDirection()
        {
            foreach (DirectionInfo el in directions)
            {
                if (el.PointId != -1)
                    if (el.Point.Data.ChoosedEvent.SpawnZone != SpawnZone.Water)
                        return el;
            }
            Debug.Log("Cant find non water direction");
            return directions.Find(x => x.PointId != -1);
        }

        public void SetParentDirections(Point current)
        {
            List<Direction> relativeDirections = GetRelativeParentDirections(current);
            DirectionInfo di = current.Parent.Data.DirectionsInfo.GetDirection(current);
            Direction myDir = di.Direction;
            foreach (var el in relativeDirections)
            {
                try { SetDirection(el, current.Parent.Data.DirectionsInfo.GetDirection(GetRelativeParentDirection(el, myDir)).PointId); }
                catch { }
            }
            SetDirection(GetDiagonalDirection(myDir), current.Parent);
        }
        private Direction GetRelativeParentDirection(Direction realtiveDir, Direction yourDir)
        {
            return realtiveDir switch
            {
                Direction i when (i == Direction.N && yourDir == Direction.W) => Direction.NW,
                Direction i when (i == Direction.N && yourDir == Direction.E) => Direction.NE,
                Direction i when (i == Direction.N && yourDir == Direction.SW) => Direction.W,
                Direction i when (i == Direction.N && yourDir == Direction.SE) => Direction.E,

                Direction i when (i == Direction.S && yourDir == Direction.W) => Direction.SW,
                Direction i when (i == Direction.S && yourDir == Direction.E) => Direction.SE,
                Direction i when (i == Direction.S && yourDir == Direction.NW) => Direction.W,
                Direction i when (i == Direction.S && yourDir == Direction.NE) => Direction.E,

                Direction i when (i == Direction.W && yourDir == Direction.N) => Direction.NW,
                Direction i when (i == Direction.W && yourDir == Direction.S) => Direction.SW,
                Direction i when (i == Direction.W && yourDir == Direction.SE) => Direction.S,
                Direction i when (i == Direction.W && yourDir == Direction.NE) => Direction.N,

                Direction i when (i == Direction.E && yourDir == Direction.N) => Direction.NE,
                Direction i when (i == Direction.E && yourDir == Direction.S) => Direction.SE,
                Direction i when (i == Direction.E && yourDir == Direction.SW) => Direction.S,
                Direction i when (i == Direction.E && yourDir == Direction.NW) => Direction.N,

                Direction i when (i == Direction.NW && yourDir == Direction.E) => Direction.N,
                Direction i when (i == Direction.NW && yourDir == Direction.S) => Direction.W,

                Direction i when (i == Direction.NE && yourDir == Direction.S) => Direction.E,
                Direction i when (i == Direction.NE && yourDir == Direction.W) => Direction.N,

                Direction i when (i == Direction.SW && yourDir == Direction.E) => Direction.S,
                Direction i when (i == Direction.SW && yourDir == Direction.N) => Direction.W,

                Direction i when (i == Direction.SE && yourDir == Direction.N) => Direction.E,
                Direction i when (i == Direction.SE && yourDir == Direction.W) => Direction.S,

                _ => throw new System.NotImplementedException()
            };
        }
        private List<Direction> GetRelativeParentDirections(Point current)
        {
            Direction yourDirInParent = current.Parent.Data.DirectionsInfo.GetDirection(current).Direction;
            List<Direction> result = new();
            result.Add(GetDiagonalDirection(yourDirInParent));
            switch (yourDirInParent)
            {
                case Direction.N:
                    result.Add(Direction.SE);
                    result.Add(Direction.SW);
                    result.Add(Direction.W);
                    result.Add(Direction.E);
                    break;
                case Direction.S:
                    result.Add(Direction.NE);
                    result.Add(Direction.NW);
                    result.Add(Direction.W);
                    result.Add(Direction.E);
                    break;
                case Direction.W:
                    result.Add(Direction.NE);
                    result.Add(Direction.SE);
                    result.Add(Direction.N);
                    result.Add(Direction.S);
                    break;
                case Direction.E:
                    result.Add(Direction.NW);
                    result.Add(Direction.SW);
                    result.Add(Direction.N);
                    result.Add(Direction.S);
                    break;
                case Direction.NW:
                    result.Add(Direction.S);
                    result.Add(Direction.E);
                    break;
                case Direction.NE:
                    result.Add(Direction.S);
                    result.Add(Direction.W);
                    break;
                case Direction.SW:
                    result.Add(Direction.N);
                    result.Add(Direction.E);
                    break;
                case Direction.SE:
                    result.Add(Direction.N);
                    result.Add(Direction.W);
                    break;
            }
            return result;
        }
        public List<Direction> GetFreeDirections()
        {
            List<Direction> result = new List<Direction>();
            directions.ForEach(x =>
            {
                if (x.PointId == -1)
                    result.Add(x.Direction);
            });
            return result;
        }
        public List<Direction> GetFilledDirections()
        {
            List<Direction> result = new List<Direction>();
            directions.ForEach(x =>
            {
                if (x.PointId > -1)
                    result.Add(x.Direction);
            });
            return result;
        }
        public Direction GetDiagonalDirection(Direction dir) => dir switch
        {
            Direction.N => Direction.S,
            Direction.S => Direction.N,
            Direction.W => Direction.E,
            Direction.E => Direction.W,
            Direction.NW => Direction.SE,
            Direction.NE => Direction.SW,
            Direction.SW => Direction.NE,
            Direction.SE => Direction.NW,
            _ => throw new System.NotImplementedException()
        };
        public Vector3 GetPositionByDirection(Direction dir, Transform transform) => dir switch
        {
            Direction.N => Vector3.up * step,
            Direction.S => Vector3.down * step,
            Direction.W => Vector3.left * step,
            Direction.E => Vector3.right * step,
            Direction.NW => (Vector3.up + Vector3.left) * step,
            Direction.NE => (Vector3.up + Vector3.right) * step,
            Direction.SW => (Vector3.down + Vector3.left) * step,
            Direction.SE => (Vector3.down + Vector3.right) * step,
            _ => throw new System.NotImplementedException()
        } + transform.position;

        public DirectionsInfo Clone()
        {
            DirectionsInfo data = new();
            List<DirectionInfo> directions = new();
            for (int i = this.directions.Count - 1; i >= 0; --i)
                directions.Add(this.directions[i]);
            data.directions = directions;
            return data;
        }
        #endregion methods
    }
}