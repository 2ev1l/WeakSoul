using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Shop
{
    public class EventInit : SingleSceneInstance
    {
        #region fields & properties
        public static EventInit Instance { get; private set; }
        [SerializeField] private EventStorage storage;
        [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
        [SerializeField] private Shop shop;
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
        private void Init()
        {
            Sprite rnd = storage.GetRandomSprite();
            bgSpriteRenderers.ForEach(x => x.sprite = rnd);
            shop.Init();
        }
        #endregion methods
    }
}