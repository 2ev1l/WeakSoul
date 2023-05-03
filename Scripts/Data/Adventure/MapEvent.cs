using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class MapEvent
    {
        #region fields & properties
        public Sprite Texture => texture;
        [SerializeField] private Sprite texture;
        public int Id => id;
        [Min(0)] [SerializeField] private int id;
        public int Level => level;
        [Min(0)] [SerializeField] private int level;
        public Events.EventType EventType => eventType;
        [SerializeField] private Events.EventType eventType;
        public SpawnZone SpawnZone => spawnZone;
        [SerializeField] private SpawnZone spawnZone;
        public float Probability => probability;
        [Range(0f, 100f)] [SerializeField] private float probability;
        #endregion fields & properties

        #region methods
        public MapEvent Clone()
        {
            MapEvent data = new()
            {
                id = Id,
                spawnZone = SpawnZone,
                probability = Probability,
                texture = Texture
            };
            return data;
        }
        #endregion methods
    }
}