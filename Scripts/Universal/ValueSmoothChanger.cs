using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Universal
{
    [System.Serializable]
    public class ValueSmoothChanger : MonoBehaviour
    {
        #region fields & properties
        public UnityAction OnChangeEnd;
        public float Out => _out;
        [SerializeField][ReadOnly] private float _out = 0;
        public bool IsChangeEnded => _isChangeEnded;
        [SerializeField][ReadOnly] private bool _isChangeEnded = false;

        private float startValue;
        private float finalValue;
        private float time;
        private float lerp;

        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Old change will be depricated
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="finalValue"></param>
        /// <param name="time"></param>
        public void StartChange(float startValue, float finalValue, float time)
        {
            EndChange();
            _isChangeEnded = false;
            this.startValue = startValue;
            this.finalValue = finalValue;
            this.time = time;
            lerp = 0f;
            Change();
        }
        public void StartChange(float startValue, float finalValue, float time, AnimationCurve curve)
        {
            this._curve = curve;
            StartChange(startValue, finalValue, time);
        }
        private void Change()
        {
            _out = Mathf.Lerp(startValue, finalValue, _curve.Evaluate(lerp / time));
            lerp += Time.deltaTime;
            if (lerp >= time)
            {
                EndChange();
                OnChangeEnd?.Invoke();
                return;
            }
            Invoke(nameof(Change), Time.unscaledDeltaTime);
        }
        private void EndChange()
        {
            _out = finalValue;
            _isChangeEnded = true;
            CancelInvoke(nameof(Change));
        }
        #endregion methods
    }
}