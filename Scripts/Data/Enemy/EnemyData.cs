using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

namespace Data
{
	[System.Serializable]
	public class EnemyData
	{
		#region fields & properties
		public Sprite Texture => texture;
		[Header("Information")][SerializeField] private Sprite texture;
		public int Id => id;
		[Min(0)][SerializeField] private int id;
		public EnemyType Type => type;
		[SerializeField] private EnemyType type;
		public IEnumerable<SpawnSubZone> SpawnZones => spawnZones;
		[SerializeField] private List<SpawnSubZone> spawnZones;

		public EntityStats Stats => stats;
		[Header("Stats")][SerializeField] private EntityStats stats;
		public SkillsInventory Skills => skills;
		[SerializeField] private SkillsInventory skills;
		public int DeltaRewardRange => deltaRewardRange;
		[Header("Reward")][SerializeField] private int deltaRewardRange = 3;
		public IEnumerable<RewardData> Rewards => rewards;
		[SerializeField] private List<RewardData> rewards = new();
		#endregion fields & properties

		#region methods
		public bool IsAllowedToSpawn(SpawnSubZone subZone) => spawnZones.Contains(subZone);
		public bool TryAddReward(out List<RewardData> rewards)
		{
			TryAddExperience();
			bool isItemsRewardAdded = TryAddItems(out rewards);
			TryAddBossReward();
			return isItemsRewardAdded;
		}
		private void TryAddExperience()
		{
			if (GameData.Data.AdventureData.IsBossNotDefeatedWhenMust(out _) && type != EnemyType.Boss) return;
			PlayerData playerData = GameData.Data.PlayerData;
			ExperienceLevel playerLevel = playerData.Stats.ExperienceLevel;
			float expMultiplier = playerLevel.Level < 15 ? 1.5f : 1.2f;
			int addedExp = Mathf.Max(CustomMath.Multiply(stats.ExperienceLevel.Experience, GetChanceByDeltaRange(deltaRewardRange * expMultiplier, playerLevel.Level, stats.ExperienceLevel.Level)), 1);
			bool IsSoulItem_ExpMult10 = playerData.Inventory.ContainItem(285);
			bool IsSoulItem_ExpMult30 = playerData.Inventory.ContainItem(286);
			bool IsSoulItem_ExpDiv15 = playerData.Inventory.ContainItem(287);

			if (IsSoulItem_ExpMult10) addedExp = CustomMath.Multiply(addedExp, 110);
			if (IsSoulItem_ExpMult30) addedExp = CustomMath.Multiply(addedExp, 130);
			if (IsSoulItem_ExpDiv15) addedExp = CustomMath.Multiply(addedExp, 85);
			playerLevel.Experience += addedExp;
		}
		private bool TryAddItems(out List<RewardData> rewards)
		{
			rewards = new();
			if (this.rewards.Count == 0) return false;

			PlayerData playerData = GameData.Data.PlayerData;
			ExperienceLevel playerLevel = playerData.Stats.ExperienceLevel;
			float chance = GetChanceByDeltaRange(deltaRewardRange, playerLevel.Level, stats.ExperienceLevel.Level);

			if (playerLevel.Level <= 2) chance = 100;
			if (GameData.Data.AdventureData.IsBossNotDefeatedWhenMust(out _) && type != EnemyType.Boss) chance /= 1.5f;

			bool isRewardAdded = false;
			foreach (var el in this.rewards)
			{
				if (el.TryAddReward(out RewardData reward, chance))
				{
					isRewardAdded = true;
					rewards.Add(reward);
				}
			}
			return isRewardAdded;
		}
		private bool TryAddBossReward()
		{
			if (Type != EnemyType.Boss) return false;

			int bossId = 0;
			bossId = id switch
			{
				20 => 0, //hunger
				65 => 1, //poverty
				108 => 2, //tranquility
				133 => 3, //apathy
				157 => 4, //death
				_ => throw new System.NotImplementedException($"Boss id at {id} id enemy"),
			};
			GameData.Data.AdventureData.TryAddDefeatedBoss(bossId);

			int openedSkillId = (bossId) switch
			{
				0 => 9,
				1 => 20,
				2 => 28,
				3 => 38,
				4 => 46,
				_ => throw new System.NotImplementedException($"Boss {bossId} id for skill reward"),
			};
			GameData.Data.PlayerData.Skills.TryAddOpenedSkill(openedSkillId);

			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="deltaRange"></param>
		/// <param name="ownLevel"></param>
		/// <param name="targetLevel"></param>
		/// <returns>0..100%</returns>
		private float GetChanceByDeltaRange(float deltaRange, float ownLevel, float targetLevel)
		{
			float deltaStep = 100f / (deltaRange + 1);
			float deltaCount = (ownLevel - targetLevel);
			return Mathf.Clamp(100f - (deltaCount * deltaStep), 0, 100);
		}
		public Wallet GetPossibleSoulsReward()
		{
			Wallet result = new();
			foreach (var el in rewards)
			{
				if (el.Type == RewardType.Soul)
				{
					SoulType soul = (SoulType)el.Id;
					result.SetSoulsByType(result.GetSoulsByType(soul) + el.Count, soul);
				}
			}
			return result;
		}
		public bool ContainHomeZone() => (SpawnZones.Contains(SpawnSubZone.Home) || type == EnemyType.Boss);
		//public void ChangeID(int id) => this.id = id;
		public virtual EnemyData Clone()
		{
			EnemyData data = new();
			data.texture = texture;
			data.type = type;
			data.id = id;
			data.stats = stats.Clone();
			List<RewardData> rewards = new();
			this.rewards.ForEach(x => rewards.Add(x.Clone()));
			data.rewards = rewards;
			data.skills = skills.Clone();
			data.spawnZones = SpawnZones.ToList();
			return data;
		}
		#endregion methods
	}
}