using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class AdventureData
	{
		#region fields & properties
		public UnityAction<int> OnBossDefeated;
		public IEnumerable<int> BossesDefeated => bossesDefeated;
		[SerializeField] private List<int> bossesDefeated = new();
		public IEnumerable<int> CutScenesViewed => cutScenesViewed;
		[SerializeField] private List<int> cutScenesViewed = new();
		#endregion fields & properties

		#region methods
		public bool TryAddDefeatedBoss(int bossId)
		{
			if (IsBossDefeated(bossId)) return false;
			AddDefeatedBoss(bossId);
			return true;
		}
		public void AddDefeatedBoss(int bossId)
		{
			bossesDefeated.Add(bossId);
			OnBossDefeated?.Invoke(bossId);
		}
		public bool TryLoadBossCutScene()
		{
			foreach (int bossId in BossesDefeated)
			{
				if (TryAddBossCutSceneShown(bossId))
				{
					int cutSceneId = GetCutSceneIdByBossId(bossId);
					if (cutSceneId == -1) continue;
					SceneLoader.Instance.LoadCutSceneFade(2, cutSceneId);
					return true;
				}
			}
			return false;
		}
		public bool IsBossAllowedForPlayer(out int bossId)
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			bossId = 0;
			if (playerLevel >= 14 && !IsBossDefeated(bossId)) return true;

			bossId = 1;
			if (playerLevel >= 27 && !IsBossDefeated(bossId)) return true;

			bossId = 2;
			if (playerLevel >= 38 && !IsBossDefeated(bossId)) return true;

			bossId = 3;
			if (playerLevel >= 45 && !IsBossDefeated(bossId)) return true;

			bossId = 4;
			if (playerLevel >= 51) return true;
			return false;
		}
		public bool IsBossNotDefeatedWhenMust(out int bossId)
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			bossId = 0;
			if (playerLevel >= 16 && !IsBossDefeated(bossId)) return true;

			bossId = 1;
			if (playerLevel >= 30 && !IsBossDefeated(bossId)) return true;

			bossId = 2;
			if (playerLevel >= 41 && !IsBossDefeated(bossId)) return true;

			bossId = 3;
			if (playerLevel >= 48 && !IsBossDefeated(bossId)) return true;

			bossId = 4;
			if (playerLevel >= 52 && !IsBossDefeated(bossId)) return true;
			return false;
		}
		public bool IsBossCutSceneShown(int bossId) => cutScenesViewed.Contains(bossId);
		public bool TryAddBossCutSceneShown(int bossId)
		{
			if (IsBossCutSceneShown(bossId)) return false;
			cutScenesViewed.Add(bossId);
			return true;
		}
		public int GetCutSceneIdByBossId(int bossId) => bossId switch
		{
			0 => 1,
			1 => -1,
			2 => 2,
			3 => -1,
			4 => -1,
			_ => throw new System.NotImplementedException($"Boss id {bossId} for cut scene"),
		};

		public bool IsBossDefeated(int bossId) => bossesDefeated.Contains(bossId);
		private Direction GetCurrentBossDirection()
		{
			int currentBossId = GetCurrentBossId();
			return currentBossId switch
			{
				0 => Direction.SW,
				1 => Direction.E,
				2 => Direction.W,
				3 => Direction.SE,
				4 => Direction.N,
				5 => Direction.N, //after final
                _ => throw new System.NotImplementedException("boss id")
			};
		}
		public int GetCurrentBossId()
		{
			int currentBossId = 0;
			if (bossesDefeated.Count > 0)
				currentBossId = bossesDefeated.Max() + 1;
			return currentBossId;
		}
		public List<Direction> GetCurrentBossDirections()
		{
			int currentBossId = GetCurrentBossId();
			List<Direction> result = new() { GetCurrentBossDirection() };
			switch (currentBossId)
			{
				case 0: result.AddRange(new Direction[] { Direction.S, Direction.W }); break;
				case 1: result.AddRange(new Direction[] { Direction.NE, Direction.SE }); break;
				case 2: result.AddRange(new Direction[] { Direction.NW, CustomMath.GetRandomChance(20) ? Direction.N : Direction.W }); break;
				case 3: result.AddRange(new Direction[] { Direction.S, Direction.E }); break;
				case 4: result.AddRange(new Direction[] { Direction.NE, CustomMath.GetRandomChance(20) ? Direction.E : Direction.N }); break;
				case 5: result.AddRange(new Direction[] { Direction.NE, CustomMath.GetRandomChance(20) ? Direction.E : Direction.N }); break; //after final
				default: throw new System.NotImplementedException("boss id");
			};
			return result;
		}
		#endregion methods
	}
}