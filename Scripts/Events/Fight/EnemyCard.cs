using Data;
using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.Events.Fight
{
	public class EnemyCard : FightCard
	{
		#region fields & properties
		public UnityAction<List<RewardData>> OnRewardAdded;
		/// <summary>
		/// <see cref="{T0}"/> - enemy id;
		/// </summary>
		public static UnityAction<int> OnEnemyDefeated;
		public EnemyData EnemyData => enemyData;
		[Header("Enemy Card")]
		[SerializeField] private EnemyData enemyData;
		[SerializeField] private List<SkillUse> skills;
		public override EntityStats Stats => enemyData.Stats;
		public override SkillsInventory SkillsInventory => enemyData.Skills;
		[SerializeField][ReadOnly] private int startHealth;
		#endregion fields & properties

		#region methods
		public override void Init(int id)
		{
			enemyData = EnemiesInfo.Instance.GetEnemy(id).Clone();
			CheckPlayerSoulItemsOnInit();
			startHealth = enemyData.Stats.Health;
			base.Init(id);
		}
		private void CheckPlayerSoulItemsOnInit()
		{
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			bool IsSoulItem_338 = playerInventory.ContainItem(338);
			bool IsSoulItem_339 = playerInventory.ContainItem(339);
			bool IsSoulItem_340 = playerInventory.ContainItem(340);
			if (IsSoulItem_338)
			{
				Stats.Health = CustomMath.Multiply(Stats.Health, 120);
				Stats.Defense = CustomMath.Multiply(Stats.Defense, 80);
				Stats.Resistance = CustomMath.Multiply(Stats.Resistance, 80);
			}
			if (IsSoulItem_339)
			{
				Stats.Health = CustomMath.Multiply(Stats.Health, 80);
				Stats.Defense = CustomMath.Multiply(Stats.Defense, 120);
				Stats.Resistance = CustomMath.Multiply(Stats.Resistance, 120);
			}
			if (IsSoulItem_340)
			{
				Stats.Health = CustomMath.Multiply(Stats.Health, 200);
			}
		}
		protected override void OnEnable()
		{
			base.OnEnable();
			TurnController.Instance.OnTurnEnd += CheckTurn;
		}
		protected override void OnDisable()
		{
			base.OnDisable();
			TurnController.Instance.OnTurnEnd -= CheckTurn;
		}
		protected override void OnDead()
		{
			if (enemyData.Stats.IsDead && !IsDeathInitialized)
			{
				AddReward();
				OnEnemyDefeated?.Invoke(enemyData.Id);
			}
			base.OnDead();
		}
		private void AddReward()
		{
			GameData.Data.TavernData.TryRemoveEnemy(enemyData.Id);
			CheckClassReward();
			CheckInventoryItemsReward();
			bool isRewardAdded = enemyData.TryAddReward(out List<RewardData> rewards);
			if (!isRewardAdded) return;
			OnRewardAdded?.Invoke(rewards);
		}
		protected override void TryResetValues(bool isLastEnemy)
		{
			if (!Stats.IsDead)
			{
				//print("player dead, no need to reapply buffes");
				return;
			}
			base.TryResetValues(isLastEnemy);
		}
		private void CheckClassReward()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			switch (playerData.Stats.Class)
			{
				case PlayerClass.Omnivorous:
					playerData.Stats.Health += CustomMath.Multiply(startHealth, 7);
					break;
				case PlayerClass.HaterOfEvil:
					playerData.Stats.ChangeKarmaBy(1);
					break;
				default: break;
			}
		}
		private void CheckInventoryItemsReward()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			ItemsInventory playerInventory = playerData.Inventory;
			if (playerInventory.ContainItem(211))
			{
				playerData.Stats.Health += CustomMath.Multiply(startHealth, 3);
			}
			if (playerInventory.ContainItem(340))
			{
				playerData.Stats.Health += 5;
			}
		}
		private void CheckTurn(FightCard card)
		{
			bool isMyTurn = card != this;
			if (!isMyTurn) return;
			StartCoroutine(RandomizeSkillUse());
		}
		private IEnumerator RandomizeSkillUse()
		{
			List<SkillUse> allowedSkills = GetAllowedSkills();
			List<SkillUse> disAllowedSkills = skills.Where(x => !x.CanUse && x.Skill.Id > -1 && x.IsInitialized).ToList();
			if (disAllowedSkills.Count > 0)
			{
				int maxStaminaUsed = disAllowedSkills.Max(x => x.Skill.StaminaPrice);
				if (maxStaminaUsed > enemyData.Stats.StaminaRegen && CustomMath.GetRandomChance(30))
				{
					TurnController.Instance.NextTurn();
					yield break;
				}
			}
			while (enemyData.Stats.Stamina > 0 && allowedSkills.Count > 0 && !OwnEnemy.IsDeathInitialized)
			{
				SkillUse possibleSkill = null;
				if (CustomMath.GetRandomChance(50))
				{
					List<SkillUse> attackSkills = allowedSkills.Where(x => x.Skill.SkillType == SkillType.Attack).ToList();
					if (attackSkills.Count > 0)
					{
						int index = Random.Range(0, attackSkills.Count);
						possibleSkill = attackSkills[index];
					}
				}
				if (possibleSkill == null)
				{
					int index = Random.Range(0, allowedSkills.Count);
					possibleSkill = allowedSkills[index];
				}
				yield return possibleSkill.UseSkill();
				allowedSkills = GetAllowedSkills();
			}
			if (!OwnEnemy.IsDeathInitialized)
				TurnController.Instance.NextTurn();
		}
		private List<SkillUse> GetAllowedSkills()
		{
			List<SkillUse> allowedSkills = skills.Where(x => x.CanUse && x.IsInitialized).ToList();
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			if (playerInventory.ContainItem(203))
			{
				allowedSkills = allowedSkills.Where(x => x.Skill.SkillType != SkillType.Block || CustomMath.GetRandomChance(50)).ToList();
			}
			if (playerInventory.ContainItem(204))
			{
				allowedSkills = allowedSkills.Where(x => x.Skill.SkillType != SkillType.Evade || CustomMath.GetRandomChance(50)).ToList();
			}
			if (playerInventory.ContainItem(205))
			{
				allowedSkills = allowedSkills.Where(x => x.Skill.SkillType != SkillType.Block).ToList();
			}
			if (playerInventory.ContainItem(206))
			{
				allowedSkills = allowedSkills.Where(x => x.Skill.SkillType != SkillType.Evade).ToList();
			}
			return allowedSkills;
		}
		#endregion methods
	}
}