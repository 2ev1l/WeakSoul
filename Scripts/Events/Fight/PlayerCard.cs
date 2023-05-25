using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.Events.Fight
{
	public class PlayerCard : FightCard
	{
		#region fields & properties
		public static UnityAction OnPlayerEscaped;
		public override EntityStats Stats => playerData.Stats;
		public PlayerData PlayerData => playerData;
		[Header("Player Card")]
		[SerializeField] private PlayerData playerData;
		public override SkillsInventory SkillsInventory => playerData.Skills;
		#endregion fields & properties

		#region methods
		public override void Init(int id)
		{
			playerData = GameData.Data.PlayerData;
			CheckClassOnInit();
			base.Init(id);
			TurnController.Instance.ResetTurns();
		}
		protected override void OnEnable()
		{
			base.OnEnable();
			OnFightEnd += CheckEnd;
			TurnController.Instance.OnTurnEnd += CheckTurns;
		}
		protected override void OnDisable()
		{
			base.OnDisable();
			OnFightEnd -= CheckEnd;
			TurnController.Instance.OnTurnEnd -= CheckTurns;
		}
		private void CheckClassOnInit()
		{
			switch (playerData.Stats.Class)
			{
				case PlayerClass.Stoic:
					playerData.Stats.ChangeKarmaBy(1);
					playerData.Stats.Health -= Mathf.Max(CustomMath.Multiply(playerData.Stats.Health, 5), 1);
					break;
				default: break;
			}
		}
		private void CheckTurns(FightCard card)
		{
			bool isMyTurn = card != this;
			bool isPlayerLevelSmall = GameData.Data.PlayerData.Stats.ExperienceLevel.Level < 6;
			bool isSoulItem_InfinityTurns = GameData.Data.PlayerData.Inventory.ContainItem(90);
			if (isPlayerLevelSmall || isSoulItem_InfinityTurns) return;
			if (isMyTurn) Invoke(nameof(SkipTurn), Random.Range(20, 40));
			else CancelInvoke(nameof(SkipTurn));
		}
		private void SkipTurn()
		{
			if (SkillUse.IsSkillAnimation)
			{
				Invoke(nameof(SkipTurn), 5f);
				return;
			}
			TurnController.Instance.NextTurn();
		}
		private void CheckEnd(bool isLastEnemy)
		{
			if (isLastEnemy || playerData.Stats.IsDead)
			{
				playerData.Stats.Stamina = Mathf.Clamp(playerData.Stats.Stamina, 1, Mathf.RoundToInt(playerData.Stats.StaminaRegen * 1.2f));
			}
		}
		protected override void TryResetValues(bool isLastEnemy)
		{
			if (!OwnEnemy.Stats.IsDead)
			{
				//print("player dead, no need to reapply buffes");
				return;
			}
			base.TryResetValues(isLastEnemy);
		}
		#endregion methods
	}
}