using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Universal;

namespace Data
{
    [System.Serializable]
    public class VFXData
    {
        #region fields & properties
        public VisualEffectAsset VFX => vfx;
        [SerializeField] private VisualEffectAsset vfx;
        public GameObject ParticleSystem => particleSystem;
        [SerializeField] private GameObject particleSystem;
        public AudioClip AudioClip => audioClip;
        [SerializeField] private AudioClip audioClip;
        public float TimeToWait => timeToWait;
        [Min(0)][SerializeField] private float timeToWait = 0f;
        public string ExposedName => exposedName;
        [SerializeField] private string exposedName = "";
        public int ExposedValue => exposedValue;
        [SerializeField] private int exposedValue;

        public Vector3 Position => position;
        private Vector3 position;

        #endregion fields & properties

        #region methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isInstantiated"></param>
        /// <param name="instantiated"></param>
        /// <param name=""></param>
        /// <returns>Time to playing</returns>
        public float Play(VisualEffect effectPrefab, Transform spawnTransform, out bool isInstantiated, out VisualEffect instantiated)
        {
            isInstantiated = false;
            instantiated = null;
            if (VFX != null)
            {
                VisualEffect inst = GameObject.Instantiate(effectPrefab, Position, Quaternion.identity, spawnTransform) as VisualEffect;
                inst.visualEffectAsset = VFX;
                Vector3 pos = inst.transform.localPosition;
                pos.z = 0;
                inst.transform.localPosition = pos;
                if (ExposedName != "")
                    inst.SetInt(ExposedName, ExposedValue);
                inst.Play();
                instantiated = inst;
                isInstantiated = true;
            }
            if (particleSystem != null)
            {
                GameObject inst = GameObject.Instantiate(particleSystem, Position, Quaternion.identity, spawnTransform);
                Vector3 pos = inst.transform.localPosition;
                pos.z = 0;
                inst.transform.localPosition = pos;
                inst.SetActive(true);
            }
            if (AudioClip != null)
                AudioManager.PlayClip(AudioClip, Universal.AudioType.Sound);
            return timeToWait;
        }
        public void SetParamValue(int value) => exposedValue = value;
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
        protected void ExposeValues(VFXData VFXData)
        {
            vfx = VFXData.vfx;
            timeToWait = VFXData.timeToWait;
            position = VFXData.position;
            exposedName = VFXData.exposedName;
            audioClip = VFXData.audioClip;
            particleSystem = VFXData.particleSystem;
        }
        public VFXData Clone() => new()
        {
            vfx = vfx,
            timeToWait = timeToWait,
            position = position,
            exposedName = exposedName,
            exposedValue = exposedValue,
            audioClip = audioClip,
            particleSystem = particleSystem
        };
        #endregion methods
    }
}