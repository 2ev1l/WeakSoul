using Data.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace Data.Adventure
{
	[System.Serializable]
	public class CardData
	{
		#region fields & properties
		public Sprite Texture => texture;
		[SerializeField] private Sprite texture;
		public int GroupId => groupID;
		[Min(0)][SerializeField] private int groupID;

		public int Id => id;
		[Min(0)][SerializeField] private int id;
		public int Level => level;
		[Min(0)][SerializeField] private int level;
		public IEnumerable<Direction> MoveDirections => moveDirections;
		[SerializeField] private List<Direction> moveDirections;
		public IEnumerable<RewardData> Rewards => rewards;
		[SerializeField] private List<RewardData> rewards = new();

		public ChanceData EventChance => eventChance;
		[Header("Mechanics")]
		[SerializeField] private ChanceData eventChance;
		public List<ChanceData> CardGroupsChance => cardGroupsChance;
		[SerializeField] private List<ChanceData> cardGroupsChance;
		public CardAllow AllowData => allowData;
		[SerializeField] private CardAllow allowData;
		public IEnumerable<StatScale> StatsScale => statsScale;
		[SerializeField] private List<StatScale> statsScale = new();

		public int Karma => karma;
		[SerializeField] private int karma;
		public int Experience => experience;
		[SerializeField] private int experience;

		#endregion fields & properties

		#region methods
		public bool IsCardAllowed(PlayerData playerData) => allowData.IsCardAllowed(playerData);
		public string GetDirectionText(bool allowCount, bool allowDirection)
		{
			string result = "";
			List<Tuple<Direction, int>> directionsUsed = new();
			foreach (Direction direction in MoveDirections)
			{
				int index = directionsUsed.FindIndex(x => x.Item1 == direction);
				if (index > -1)
				{
					directionsUsed[index] = new Tuple<Direction, int>(direction, directionsUsed[index].Item2 + 1);
				}
				else
				{
					directionsUsed.Add(new Tuple<Direction, int>(direction, 1));
				}
			}
			foreach (var el in directionsUsed)
			{
				result += $"\n{(allowCount ? el.Item2 : "?")}-{(allowDirection ? el.Item1 : "?")}";
			}
			return result;
		}
		public string GetRewardText()
		{
			string result = "";
			if (rewards.Count > 0)
			{
				int count = rewards.Count;
				for (int i = 0; i < count; ++i)
				{
					if (i >= 3)
					{
						result += $"\n...";
						break;
					}
					RewardData el = rewards[i];
					result += "\n";
					result += LanguageLoader.GetRewardTextByType(el.Type, el.Id);
					if (el.Type != RewardType.Random)
						result += $" x{el.Count}";
					result += $" {el.Chance}%";
				}
			}
			return result;
		}
		public void GetStatsReward()
		{
			int expValue = Experience;
			int karmaValue = Karma;
			PlayerStats playerStats = GameData.Data.PlayerData.Stats;
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			if (playerStats.Class == PlayerClass.HaterOfEvil)
				karmaValue *= 2;
			if (playerInventory.ContainItem(337))
				playerStats.Health += 1;
			playerStats.ChangeKarmaBy(karmaValue);
			playerStats.ExperienceLevel.Experience += expValue;
		}
		//public void ChangeID(int id) => this.id = id;
		#endregion methods
	}
}