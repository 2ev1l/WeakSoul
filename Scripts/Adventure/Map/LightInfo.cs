using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.Adventure.Map
{
    [System.Serializable]
    public class LightInfo
    {
        #region fields & properties
        public Light Light => light;
        [SerializeField] private Light light;
        public float DefaultRange => defaultRange;
        [SerializeField] private float defaultRange;
        public float DefaultIntensity => defaultIntensity;
        [SerializeField] private float defaultIntensity;
        #endregion fields & properties

        #region methods
        public LightInfo(Light light, float defaultRange, float defaultIntensity)
        {
            this.light = light;
            this.defaultRange = defaultRange;
            this.defaultIntensity = defaultIntensity;
        }
        #endregion methods
    }
}