using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data.Events
{
	[System.Serializable]
	public class BattleData
	{
		#region fields & properties
		public UnityAction<FightData> OnCurrentFightChanged;
		public IEnumerable<FightData> Fights => fights;
		[SerializeField] private List<FightData> fights = new();
		#endregion fields & properties

		#region methods
		public void GenerateData(SubZoneData subZoneData, MapEvent mapEvent)
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			if (subZoneData.LevelsScale.x > playerLevel)
				playerLevel += 2;
			bool isBossNotDefeatedWhenMust = GameData.Data.AdventureData.IsBossNotDefeatedWhenMust(out _);
			if (isBossNotDefeatedWhenMust)
				playerLevel -= CustomMath.GetRandomChance(70) ? 1 : 0;
			List<EnemyData> denemies = GetAllowedEnemies(subZoneData.SubZone).Where(x => x.Stats.ExperienceLevel.Level <= playerLevel + 3).ToList();
			List<EnemyData> enemies = denemies.Where(x => x.Stats.ExperienceLevel.Level <= playerLevel).ToList();
			if (enemies.Count == 0)
			{
				Debug.Log($"Zero enemies for {subZoneData.SubZone} subzone. Fixing - Random");
				enemies = EnemiesInfo.Instance.EnemiesData.Where(x => x.Stats.ExperienceLevel.Level <= playerLevel).ToList();
			}
			if (denemies.Count == 0)
				denemies = enemies;

			bool isPlayerLevelHigh = playerLevel > 29 && !isBossNotDefeatedWhenMust;
			int normalDiff = isPlayerLevelHigh ? 1 : 0;
			List<EnemyData> normalizedEnemies = denemies.Where(x =>
			{
				int difference = x.Stats.ExperienceLevel.Level - playerLevel;
				return difference <= normalDiff && difference >= -2;
			}).ToList();

			List<EnemyData> dangerousEnemies = denemies.Where(x =>
			{
				int difference = x.Stats.ExperienceLevel.Level - playerLevel;
				return difference <= 2 && difference >= 0;
			}).ToList();

			fights = new();
			switch (mapEvent.Id)
			{
				case 1: GenerateDefaultEnemy(enemies, normalizedEnemies, dangerousEnemies); break;
				case 2: GenerateDungeon(enemies, normalizedEnemies, dangerousEnemies); break;
				case 4: GenerateUniqueEnemy(enemies, normalizedEnemies); break;
				case 9: GenerateBoss(subZoneData.SubZone); break;
				case 10: goto case 1;
				case 13: goto case 1;
				case 16: goto case 1;
				case 17: goto case 1;
				default: throw new System.NotImplementedException($"Event id {mapEvent.Id} for Battle Data");
			}
		}
		private void GenerateDefaultEnemy(List<EnemyData> enemies, List<EnemyData> normalizedEnemies, List<EnemyData> dangerousEnemies)
		{
			if (CustomMath.GetRandomChance(20))
			{
				ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
				if (playerInventory.ContainItem(215))
				{
					if (playerInventory.ContainItem(214) && CustomMath.GetRandomChance(50))
					{
						GenerateDungeon(enemies, normalizedEnemies, dangerousEnemies);
						return;
					}
					GenerateUniqueEnemy(enemies, normalizedEnemies);
					return;
				}
				if (playerInventory.ContainItem(214))
				{
					GenerateDungeon(enemies, normalizedEnemies, dangerousEnemies);
					return;
				}
			}
			normalizedEnemies = normalizedEnemies.Where(x => x.Type == EnemyType.Default).ToList();
			enemies = enemies.Where(x => x.Type == EnemyType.Default).ToList();
			List<EnemyData> merged = MergeEnemies(enemies, normalizedEnemies);
			fights.Add(new(GetRandomEnemyId(merged)));
		}
		private List<EnemyData> MergeEnemies(List<EnemyData> allEnemies, List<EnemyData> mainEnemies)
		{
			List<EnemyData> choosed = new();
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			foreach (EnemyData el in mainEnemies)
				choosed.Add(el);
			foreach (EnemyData el in allEnemies.OrderByDescending(x => x.Stats.ExperienceLevel.Level))
			{
				if (choosed.Contains(el)) continue;
				choosed.Add(el);
				if (choosed.Count >= 3 && (CustomMath.GetRandomChance(25) || playerLevel - el.Stats.ExperienceLevel.Level >= 4)) break;
			}
			if (choosed.Count == 0)
				foreach (EnemyData el in allEnemies)
					choosed.Add(el);
			return choosed;
		}
		private void GenerateDungeon(List<EnemyData> enemies, List<EnemyData> normalizedEnemies, List<EnemyData> dangerousEnemies)
		{
			normalizedEnemies = normalizedEnemies.Where(x => x.Type == EnemyType.Default).ToList();
			enemies = enemies.Where(x => x.Type == EnemyType.Default).ToList();
			dangerousEnemies = dangerousEnemies.Where(x => x.Type != EnemyType.Boss).ToList();
			List<EnemyData> merged = MergeEnemies(enemies, normalizedEnemies);
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			merged = merged.Where(x => x.Stats.ExperienceLevel.Level <= playerLevel).ToList();
			int dugeonEnemiesCount = Random.Range(1, 4);
			for (int i = 0; i < dugeonEnemiesCount; ++i)
			{
				fights.Add(new(GetRandomEnemyId(merged)));
			}
			fights.Add(new(GetRandomEnemyId(dangerousEnemies.Count > 0 ? dangerousEnemies : (normalizedEnemies.Count > 0 ? normalizedEnemies : enemies))));
		}
		private void GenerateUniqueEnemy(List<EnemyData> enemies, List<EnemyData> normalizedEnemies)
		{
			enemies = enemies.Where(x => x.Type == EnemyType.Unique).ToList();
			normalizedEnemies = normalizedEnemies.Where(x => x.Type == EnemyType.Unique).ToList();
			List<EnemyData> merged = MergeEnemies(enemies, normalizedEnemies);
			fights.Add(new(GetRandomEnemyId(merged)));
		}
		private void GenerateBoss(SpawnSubZone spawnSubZone)
		{
			fights.Add(new(EnemiesInfo.Instance.GetBoss(spawnSubZone).Id));
		}
		private int GetRandomEnemyId(List<EnemyData> enemies) => enemies[Random.Range(0, enemies.Count)].Id;
		protected IEnumerable<EnemyData> GetAllowedEnemies(SpawnSubZone subZone) => EnemiesInfo.Instance.GetAllowedEnemies(subZone);
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Fights count after remove</returns>
		public int TryRemoveFirstFight()
		{
			try
			{
				fights.RemoveAt(0);
				return fights.Count;
			}
			catch { return 0; }
		}
		public BattleData() { }
		public BattleData(List<FightData> fights)
		{
			this.fights = fights;
		}
		#endregion methods
	}
}