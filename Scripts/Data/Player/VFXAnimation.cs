using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Data
{
    [System.Serializable]
    public class VFXAnimation
    {
        #region fields & properties
        public IEnumerable<VFXData> VFXs => vfxs;
        [SerializeField] private List<VFXData> vfxs = new();
        #endregion fields & properties

        #region methods
        public static IEnumerator Animate(VisualEffect effectPrefab, Transform spawnTransform, IEnumerable<VFXData> vfxs)
        {
            List<VisualEffect> instantiatedEffects = new();
            foreach (VFXData el in vfxs)
            {
                float timeToWait = el.Play(effectPrefab, spawnTransform, out bool isInstantiated, out VisualEffect inst);
                if (isInstantiated)
                    instantiatedEffects.Add(inst);
                yield return new WaitForSecondsRealtime(timeToWait);
            }
            instantiatedEffects.ForEach(x => GameObject.Destroy(x.gameObject, Time.deltaTime + Random.Range(0, 2f)));
        }
        public VFXAnimation Clone()
        {
            VFXAnimation vfxa = new();
            vfxs.ForEach(x => vfxa.vfxs.Add(x));
            return vfxa;
        }
        #endregion methods
    }
}