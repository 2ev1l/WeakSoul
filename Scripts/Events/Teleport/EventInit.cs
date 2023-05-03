using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Teleport
{
    public class EventInit : SingleSceneInstance
    {
        #region fields & properties
        public static EventInit Instance { get; private set; }
        [SerializeField] private EventStorage storage;
        [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
        [SerializeField] private Teleport teleport;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            Init();
        }
        [ContextMenu("Init")]
        private void Init()
        {
            Sprite rnd = storage.GetRandomSprite();
            bgSpriteRenderers.ForEach(x => x.sprite = rnd);
            int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            List<PointData> allowedPoints = PointsInit.GeneratedPointsData.Where(x => x.ChoosedEvent.Id == 0).ToList();
            if (allowedPoints.Count == 0)
            {
                Debug.LogError("Error - No allowed points for teleport. Fixing - Set By Default");
                teleport.Init(0);
                return;
            }
            teleport.Init(allowedPoints[Random.Range(0, allowedPoints.Count)].PointId);
        }
        #endregion methods
    }
}