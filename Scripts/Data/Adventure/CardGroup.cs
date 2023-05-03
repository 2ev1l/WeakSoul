using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace Data.Adventure
{
	[System.Serializable]
	public class CardGroup
	{
		#region fields & properties
		public int Id => id;
		[SerializeField] private int id;
		public int Level => level;
		[SerializeField] private int level;
		public IEnumerable<SpawnSubZone> Appearance => appearance;
		[SerializeField] private List<SpawnSubZone> appearance;
		#endregion fields & properties

		#region methods
		public bool IsGroupAllowed() => GameData.Data.PlayerData.Stats.ExperienceLevel.Level >= level;
		public IEnumerable<CardSO> GetGroupCards(PlayerData playerData) => CardsInfo.Instance.Cards.Where(x => x.CardData.GroupId == id && !IsClosedBySoulItems(x) && x.CardData.IsCardAllowed(playerData));
		public IEnumerable<CardSO> GetGroupCardsRaw(PlayerData playerData) => CardsInfo.Instance.Cards.Where(x => x.CardData.GroupId == id && x.CardData.IsCardAllowed(playerData));
		public void GetAllowedCards(out List<CardData> optimalCards, out List<CardData> defaultCards, bool includeSoulItemCheck)
		{
			PlayerData playerData = GameData.Data.PlayerData;
			IEnumerable<CardSO> num = includeSoulItemCheck ? GetGroupCards(playerData) : GetGroupCardsRaw(playerData);
			optimalCards = new();
			defaultCards = new();
			List<Direction> currentBossDirections = GameData.Data.AdventureData.GetCurrentBossDirections();
			int playerLevel = playerData.Stats.ExperienceLevel.Level;
			float randomChanceForNonDirectedCard = PointsInit.IsBossMustBeBeaten ? 5 : 20;
			foreach (CardSO card in num)
			{
				if (card.CardData.Level > playerLevel) continue;
				bool isOptimal = (IsOptimalDirection(card, currentBossDirections) || IsOptimalBySoulItems(card));
				if (isOptimal)
					optimalCards.Add(card.CardData);
				else
				{
					if (CustomMath.GetRandomChance(randomChanceForNonDirectedCard))
					{
						bool isNullDirection = card.CardData.MoveDirections.Count() == 0 || card.CardData.MoveDirections.Contains(Direction.RND);
						if (isNullDirection)
						{
							optimalCards.Add(card.CardData);
							continue;
						}
					}

					defaultCards.Add(card.CardData);
				}
			}
		}
		private bool IsClosedBySoulItems(CardSO card)
		{
			List<Direction> directions = new();
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			if (inventory.ContainItem(15)) directions.Add(Direction.N);
			if (inventory.ContainItem(84)) directions.Add(Direction.S);
			if (inventory.ContainItem(86)) directions.Add(Direction.W);
			if (inventory.ContainItem(88)) directions.Add(Direction.E);
			foreach (var el in directions)
			{
				if (card.CardData.MoveDirections.Contains(el))
					return true;
			}
			return false;
		}
		private bool IsOptimalBySoulItems(CardSO card)
		{
			List<Direction> directions = new();
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			if (inventory.ContainItem(16)) directions.Add(Direction.N);
			if (inventory.ContainItem(85)) directions.Add(Direction.S);
			if (inventory.ContainItem(87)) directions.Add(Direction.W);
			if (inventory.ContainItem(89)) directions.Add(Direction.E);
			return IsOptimalDirection(card, directions);
		}
		private bool IsOptimalDirection(CardSO card, List<Direction> directions)
		{
			foreach (Direction direction in directions)
				if (card.CardData.MoveDirections.Contains(direction))
					return true;
			return false;
		}
		#endregion methods
	}
}