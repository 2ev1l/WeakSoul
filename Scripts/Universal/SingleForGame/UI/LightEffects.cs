using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal
{
    public class LightEffects : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Light pointLight;
        [SerializeField] private bool useTemperature;
        [SerializeField] private Vector3 colorTemperatureRange = new(0, 0, 1f);
        [SerializeField] private ValueSmoothChanger colorChanger;
        [SerializeField] private bool useIntensity;
        [SerializeField] private Vector3 intensityRange = new(0, 0, 0.1f);
        [SerializeField] private ValueSmoothChanger intensityChanger;
        [SerializeField] private bool useLightRange;
        [SerializeField] private Vector3 lightRange = new(0, 0, 0.1f);
        [SerializeField] private ValueSmoothChanger lightChanger;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            RandomValues();
        }
        private void RandomValues()
        {
            if (useTemperature)
                StartCoroutine(ChangeAny(pointLight.colorTemperature, colorTemperatureRange, 0));
            if (useIntensity)
                StartCoroutine(ChangeAny(pointLight.intensity, intensityRange, 1));
            if (useLightRange)
                StartCoroutine(ChangeAny(pointLight.range, lightRange, 2));
        }
        private IEnumerator ChangeAny(float currentValue, Vector3 range, int param, bool increase = false)
        {
            ValueSmoothChanger vsc = param switch
            {
                0 => colorChanger,
                1 => intensityChanger,
                2 => lightChanger,
                _ => throw new System.NotImplementedException(),
            };
            vsc.StartChange(currentValue, increase ? range.y : range.x, range.z);
            while (!vsc.IsChangeEnded)
            {
                switch (param)
                {
                    case 0: pointLight.colorTemperature = vsc.Out; break;
                    case 1: pointLight.intensity = vsc.Out; break;
                    case 2: pointLight.range = vsc.Out; break;
                    default: throw new System.NotImplementedException();
                }
                yield return CustomMath.WaitAFrame();
            }
            currentValue = vsc.Out;
            StartCoroutine(ChangeAny(currentValue, range, param, !increase));
        }

        private float GetValuePoint(float currentValue, Vector3 range)
        {
            float normalDistance = Mathf.Max(range.x, range.y) - Mathf.Min(range.x, range.y);
            float valueDistance = currentValue - Mathf.Min(range.x, range.y);
            float valuePoint = 0f;
            try { valuePoint = valueDistance / normalDistance; }
            catch { }
            return valuePoint;
        }
        private float RandomAny(float currentValue, Vector3 range)
        {
            float valuePoint = GetValuePoint(currentValue, range);
            int scale = CustomMath.GetRandomChance(valuePoint * 100) ? -1 : 1;
            currentValue += (scale + Time.deltaTime) * range.z;
            currentValue = Mathf.Clamp(currentValue, Mathf.Min(range.x, range.y), Mathf.Max(range.x, range.y));
            return currentValue;
        }
        #endregion methods
    }
}