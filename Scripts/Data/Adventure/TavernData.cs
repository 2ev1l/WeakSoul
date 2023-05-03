using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
	[System.Serializable]
	public class TavernData
	{
		#region fields & properties
		public UnityAction<IEnumerable<QuestData>> OnDataGenerated;
		public IEnumerable<QuestData> Quests => quests;
		[SerializeField] private List<QuestData> quests = new();
		public int LastDayGenerated => lastDayGenerated;
		[SerializeField] private int lastDayGenerated = 0;
		#endregion fields & properties

		#region methods
		public void GenerateData()
		{
			int questsCount = Random.Range(1, 5);
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			List<EnemyData> enemies = EnemiesInfo.Instance.EnemiesData.Where(x =>
			{
				int enemyLevel = x.Stats.ExperienceLevel.Level;
				return playerLevel - enemyLevel <= 3 && playerLevel - enemyLevel >= -2;
			}).ToList();
			quests = new();
			for (int i = questsCount - 1; i >= 0; --i)
			{
				QuestData data = new();
				data.GenerateNewData(enemies);
				quests.Add(data);
			}
			lastDayGenerated = GameData.Data.Days;
			OnDataGenerated?.Invoke(Quests);
		}
		public bool TryRemoveEnemy(int enemyId)
		{
			bool isRemoved = false;
			foreach (var el in quests)
			{
				if (el.TryRemoveEnemy(enemyId))
				{
					isRemoved = true;
				}
				el.TryGetReward();
			}
			return isRemoved;
		}
		#endregion methods
	}
}