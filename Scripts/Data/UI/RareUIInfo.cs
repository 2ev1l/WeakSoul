using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class RareUIInfo
    {
        #region fields & properties
        public Vector4 Color => color;
        [SerializeField] private Color color;
        public Sprite Sprite => sprite;
        [SerializeField] private Sprite sprite;
        public float BurnAmount => Random.Range(burnAmount * 0.9f, burnAmount * 1.1f);
        [SerializeField] private float burnAmount;
        public int Rare => rare;
        [SerializeField] private int rare;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}