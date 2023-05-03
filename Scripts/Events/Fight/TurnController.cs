using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class TurnController : SingleSceneInstance
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> Card, which turn was ended
        /// </summary>
        public UnityAction<FightCard> OnTurnEnd;
        public static TurnController Instance { get; private set; }
        public int CurrentTurn => currentTurn;
        public FightCard CurrentTurnCard => currentTurn % 2 == 0 ? player : enemy;
        public FightCard OldTurnCard => currentTurn % 2 == 1 ? player : enemy;
        [SerializeField] private int currentTurn = 0;
        [SerializeField] private PlayerCard player;
        [SerializeField] private EnemyCard enemy;
        [SerializeField] private Text trapText;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }

        public void ResetTurns()
        {
            ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
            if (EventInfo.Instance.Data.Event.Id == 13 && !playerInventory.ContainItem(341))
            {
                StartCoroutine(Trap());
                return;
            }
            currentTurn = 0;
            OnTurnEnd?.Invoke(enemy);
        }
        private IEnumerator Trap()
        {
            StartCoroutine(TrapAnimation());
            currentTurn = 1;
            while (!enemy.IsInitialized || !player.IsInitialized)
                yield return CustomMath.WaitAFrame();
            OnTurnEnd?.Invoke(player);
            yield break;
        }
        private IEnumerator TrapAnimation()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            trapText.gameObject.SetActive(true);
            vsc.StartChange(0, 1, 2);
            while (!vsc.IsChangeEnded)
            {
                Color col = trapText.color;
                col.a = vsc.Out;
                trapText.color = col;
                yield return CustomMath.WaitAFrame();
            }
            vsc.StartChange(1, 0, 2);
            while (!vsc.IsChangeEnded)
            {
                Color col = trapText.color;
                col.a = vsc.Out;
                trapText.color = col;
                yield return CustomMath.WaitAFrame();
            }
            trapText.gameObject.SetActive(false);
            Destroy(vsc);
        }
        [ContextMenu("Next Turn")]
        public void NextTurn()
        {
            currentTurn++;
            OnTurnEnd?.Invoke(OldTurnCard);
        }
        #endregion methods
    }
}