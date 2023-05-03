using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.Events.Fight
{
	public abstract class FightCard : MonoBehaviour
	{
		#region fields & properties
		/// <summary>
		/// <see cref="{T0}"/> IsLastEnemy
		/// </summary>
		public static UnityAction<bool> OnFightEnd;
		public UnityAction OnCardInit;
		public FightCard OwnEnemy => ownEnemy;
		[SerializeField] private FightCard ownEnemy;
		public abstract EntityStats Stats { get; }
		public abstract SkillsInventory SkillsInventory { get; }
		public FightBuffes Buffes => buffes;
		[Header("Debug")]
		[SerializeField][ReadOnly] private FightBuffes buffes;
		public bool IsPlayer => isPlayer;
		[SerializeField][ReadOnly] private bool isPlayer = false;
		public bool IsInitialized => isInitialized;
		[SerializeField][ReadOnly] private bool isInitialized;
		public bool IsDeathInitialized => isDeathInitialized;
		[SerializeField][ReadOnly] private bool isDeathInitialized;
		[SerializeField][ReadOnly] private bool isBuffesRemoved;

		#endregion fields & properties

		#region methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">-1 for player, other for enemy</param>
		public virtual void Init(int id)
		{
			buffes.Init(Stats);
			isInitialized = true;
			isPlayer = id == -1;
			OnCardInit?.Invoke();
			OnDisable();
			OnEnable();
		}
		protected virtual void OnEnable()
		{
			TurnController.Instance.OnTurnEnd += TryDecreaseBuffes;
			TurnController.Instance.OnTurnEnd += TryResetStamina;
			Stats.OnDead += OnDead;
			OnFightEnd += TryResetValues;
		}
		protected virtual void OnDisable()
		{
			TurnController.Instance.OnTurnEnd -= TryDecreaseBuffes;
			TurnController.Instance.OnTurnEnd -= TryResetStamina;
			Stats.OnDead -= OnDead;
			OnFightEnd -= TryResetValues;
		}
		private void TryResetStamina(FightCard card)
		{
			if (card == this) return;
			Stats.ResetStamina();
		}
		private void TryDecreaseBuffes(FightCard card)
		{
			buffes.DecreaseBuffes();
			if (card != this) return;
		}
		protected virtual void TryResetValues(bool isLastEnemy)
		{
			buffes.CloseBuffes();
			isBuffesRemoved = true;
		}
		protected virtual void OnDead()
		{
			if (isDeathInitialized) return;
			isDeathInitialized = true;
			OnFightEnd?.Invoke(EventInfo.Instance.Data.BattleData.TryRemoveFirstFight() == 0);
		}
		#endregion methods
	}
}