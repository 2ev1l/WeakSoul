using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class NextDoor : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite lastDoor;
        [SerializeField] private List<GameObject> shadowOverlayObjects;
        [SerializeField][ReadOnly] private bool isLast;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            isLast = EventInfo.Instance.Data.BattleData.Fights.Count() == 0;
            UpdateUI();
        }
        private void UpdateUI()
        {
            spriteRenderer.sprite = isLast ? lastDoor : EventInfo.Instance.Data.BattleData.Fights.First().EnemyData.Texture;
            shadowOverlayObjects.ForEach(x => x.SetActive(!isLast));
            StartCoroutine(SpriteAlphaChange());
        }
        private IEnumerator SpriteAlphaChange()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 0.8f, 1);
            while (!vsc.IsChangeEnded)
            {
                Color col = spriteRenderer.color;
                col.a = vsc.Out;
                spriteRenderer.color = col;
                yield return CustomMath.WaitAFrame();
            }
            yield return CustomMath.WaitAFrame();
            Destroy(vsc);
        }
        public void Enter()
        {
            WeakSoul.Events.Fight.EventInit.Instance.NextBattleOrLeave(isLast);
        }
        #endregion methods
    }
}