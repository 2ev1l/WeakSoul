using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class GainText : MonoBehaviour
    {
        #region fields & properties
        protected virtual float removeTime => 1;
        protected virtual float yIncrease => -100;
        [SerializeField] protected Text txt;
        #endregion fields & properties

        #region methods
        protected virtual void Start()
        {
            StartCoroutine(StartAll());
        }
        private IEnumerator StartAll()
        {
            StartCoroutine(TextAlpha());
            StartCoroutine(TextPosition());
            yield return new WaitForSeconds(removeTime+0.5f);
            Destroy(gameObject);
        }
        private IEnumerator TextPosition()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(transform.localPosition.y, transform.localPosition.y + yIncrease, removeTime);
            while (!vsc.IsChangeEnded)
            {
                Vector3 pos = transform.localPosition;
                pos.y = vsc.Out;
                transform.localPosition = pos;
                yield return CustomMath.WaitAFrame();
            }
        }
        private IEnumerator TextAlpha()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(1, 0, removeTime);
            while (!vsc.IsChangeEnded)
            {
                Color col = txt.color;
                col.a = vsc.Out;
                txt.color = col;
                yield return CustomMath.WaitAFrame();
            }
        }
        #endregion methods
    }
}