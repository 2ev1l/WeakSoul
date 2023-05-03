using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Adventure.Backpack
{
    public class BackpackPanel : SingleSceneInstance
    {
        #region fields & properties
        public static BackpackPanel Instance { get; private set; }
        [SerializeField] private GameObject panel;
        public GameObject BreakEffectPrefab => breakEffectPrefab;
        [SerializeField] private GameObject breakEffectPrefab;
        public GameObject SoulItemDefaultEffectPrefab => soulItemDefaultEffectPrefab;
        [SerializeField] private GameObject soulItemDefaultEffectPrefab;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}