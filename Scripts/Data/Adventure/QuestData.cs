using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class QuestData
	{
		#region fields & properties
		public IEnumerable<int> EnemiesId => enemiesId;
		[SerializeField] private List<int> enemiesId = new();
		public bool IsCompleted => isCompleted;
		[SerializeField] private bool isCompleted = false;
		public bool IsTaken => isTaken;
		[SerializeField] private bool isTaken = false;
		public Wallet BuyPrice => buyPrice;
		[SerializeField] private Wallet buyPrice = new();

		public Wallet WalletReward => walletReward;
		[SerializeField] private Wallet walletReward = new();
		public int KarmaReward => karmaReward;
		[SerializeField] private int karmaReward = 0;
		public bool IsRewardAdded => isRewardAdded;
		[SerializeField] private bool isRewardAdded = false;
		#endregion fields & properties

		#region methods
		public bool TryRemoveEnemy(int enemyId)
		{
			int index = enemiesId.Find(x => x == enemyId);
			isCompleted = enemiesId.Count == 0;
			if (index < 0 || !isTaken) return false;
			enemiesId.Remove(index);
			isCompleted = enemiesId.Count == 0;
			return true;
		}
		public string GetEnemiesText()
		{
			string result = "";
			List<int> uniqueEnemyIds = new();
			foreach (var el in enemiesId)
			{
				if (uniqueEnemyIds.Contains(el)) continue;
				uniqueEnemyIds.Add(el);
			}
			foreach (var el in uniqueEnemyIds)
			{
				result += $"{LanguageLoader.GetTextByType(TextType.EnemyName, el)} x{enemiesId.Where(x => x == el).Count()}\n";
			}
			return result;
		}
		public bool TryGetReward()
		{
			if (isRewardAdded || !isCompleted || !isTaken) return false;
			PlayerData player = GameData.Data.PlayerData;
			player.Stats.ChangeKarmaBy(karmaReward);
			player.Wallet.IncreaseValues(walletReward);
			isRewardAdded = true;
			return true;
		}
		public void GenerateNewData(List<EnemyData> allowedEnemies)
		{
			int count = Random.Range(1, 4);
			enemiesId = new();
			buyPrice = new();
			walletReward = new();
			if (allowedEnemies.Count == 0)
			{
				Debug.Log("0 allowed enemies for quests");
				return;
			}
			for (int i = 0; i < count; i++)
			{
				enemiesId.Add(allowedEnemies[Random.Range(0, allowedEnemies.Count)].Id);
				EnemyData enemy = EnemiesInfo.Instance.GetEnemy(enemiesId[i]);
				IncreaseSoulsPrice(enemy.GetPossibleSoulsReward());
			}
			walletReward.IncreaseValues(buyPrice);
			walletReward.IncreaseValues(buyPrice);

			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			playerLevel = Mathf.Max(playerLevel, 1);
			float log = Mathf.Log(playerLevel);
			float pow = Mathf.Pow(log, 1.4f);
			int roundPow = Mathf.RoundToInt(pow);
			karmaReward = Mathf.Clamp(roundPow + 1, 1, 20);
		}
		private void IncreaseSoulsPrice(Wallet enemySouls)
		{
			for (int i = 0; i < (int)SoulType.Legendary; ++i)
			{
				SoulType soul = (SoulType)i;
				buyPrice.SetSoulsByType(buyPrice.GetSoulsByType(soul) + GetMinSoulsCount(enemySouls, soul), soul);
			}
		}
		private int GetMinSoulsCount(Wallet wallet, SoulType soulType)
		{
			float value = wallet.GetSoulsByType(soulType);
			value *= 0.33f;
			value = soulType switch
			{
				SoulType.Weak => 1,
				SoulType.Unique => 0,
				SoulType.Legendary => 0,
				_ => value
			};
			return Mathf.Clamp(Mathf.RoundToInt(value), 0, 1);
		}
		public void TakeQuest()
		{
			isTaken = true;
		}
		public QuestData() { }
		public QuestData Clone() => new()
		{
			enemiesId = EnemiesId.ToList(),
			isCompleted = isCompleted,
			isRewardAdded = isRewardAdded,
			karmaReward = karmaReward,
			walletReward = walletReward.Clone(),
			buyPrice = buyPrice.Clone(),
			isTaken = isTaken,
		};
		#endregion methods
	}
}