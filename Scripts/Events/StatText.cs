using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.Events
{
    public class StatText : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Text txt;
        [SerializeField] private ValueSmoothChanger vscPos;
        [SerializeField] private ValueSmoothChanger vscCol;
        public float AnimationTime => animationTime;
        [SerializeField] private float animationTime = 1f;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns>Animation time to wait</returns>
        private float Init(PhysicalStatsType type, int value)
        {
            string lang = LanguageLoader.GetTextByType(TextType.GameMenu, PhysicalStats.GetStatsLanguageIdByType(type));
            txt.text = $"{lang} {(value > 0 ? "+" : "")}{value}";
            txt.color = value > 0 ? Color.green : Color.red;
            StartCoroutine(StartAnimation());
            Invoke(nameof(DestroyObject), animationTime);
            return animationTime;
        }
        private void DestroyObject() => Destroy(gameObject);
        private IEnumerator StartAnimation()
        {
            vscCol.StartChange(txt.color.a, 0, animationTime);
            vscPos.StartChange(transform.position.y, transform.position.y + 1, animationTime);
            while(!vscPos.IsChangeEnded || !vscCol.IsChangeEnded)
            {
                Vector3 pos = transform.position;
                pos.y = vscPos.Out;
                transform.position = pos;

                Color col = txt.color;
                col.a = vscCol.Out;
                txt.color = col;
                yield return CustomMath.WaitAFrame();
            }
        }
        public static StatText SpawnPrefab(StatText prefab, Transform spawnCanvas, Vector3 position, PhysicalStatsType type, int value)
        {
            StatText text = GameObject.Instantiate(prefab, position, Quaternion.identity, spawnCanvas);
            Vector3 pos = text.transform.localPosition;
            pos.z = 0;
            text.transform.localPosition = pos;
            text.Init(type, value);
            return text;
        }
        #endregion methods
    }
}