using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class CanvasAlphaChanger : MonoBehaviour
    {
        #region fields & properties
        public CanvasGroup CanvasGroup => canvasGroup;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private ValueSmoothChanger vsc;
        bool disableObjectAfterTime = false;
        #endregion fields & properties

        #region methods
        public void StartChange(float startValue, float endValue, float time, bool disableObjectAfterTime = false)
        {
            CancelInvoke(nameof(ChangeCheck));
            vsc.StartChange(startValue, endValue, time);
            this.disableObjectAfterTime = disableObjectAfterTime;
            ChangeCheck();
        }
        private void ChangeCheck()
        {
            canvasGroup.alpha = vsc.Out;
            if (vsc.IsChangeEnded)
            {
                if (gameObject.activeSelf == disableObjectAfterTime)
                    gameObject.SetActive(!disableObjectAfterTime);
                return;
            }
            Invoke(nameof(ChangeCheck), Time.deltaTime);
        }
        #endregion methods
    }
}