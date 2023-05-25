using Data.Adventure;
using Data.ScriptableObjects;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using System.Linq;
using UnityEngine.Events;

namespace WeakSoul.Adventure.Map
{
	public class CardsPanel : SingleSceneInstance
	{
		#region fields & properties
		public static UnityAction OnCardAdded;
		public static CardsPanel Instance { get; private set; }
		public static List<int> ChoosedCards { get; private set; } = new();
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private GameObject cardsRoot;
		[SerializeField] private Card leftCard;
		[SerializeField] private Card rightCard;
		[SerializeField] private LanguageLoader groupNameLanguage;
		[SerializeField] private LanguageLoader groupDescriptionLanguage;
		[SerializeField] private GameObject backButton;

		private CardGroup groupData = null;
		private CardData leftCardData = null;
		private CardData rightCardData = null;
		private SubZoneData subZoneData = null;

		[Header("Debug")]
		[Min(-1)][SerializeField] private int nextGroupId = -1;
		[Min(-1)][SerializeField] private int nextLeftCardId = -1;
		[Min(-1)][SerializeField] private int nextRightCardId = -1;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		private void OnEnable()
		{
			Player.OnSubZoneChanged += SetSubZone;
			Player.OnStartMoving += OnPlayerStartMoving;
			PointsInit.Instance.OnStartGeneration += ResetList;
			Card.OnCardChoosed += AddCard;
		}
		private void OnDisable()
		{
			Player.OnSubZoneChanged -= SetSubZone;
			Player.OnStartMoving -= OnPlayerStartMoving;
			PointsInit.Instance.OnStartGeneration -= ResetList;
			Card.OnCardChoosed -= AddCard;
		}
		private void AddCard(Card card)
		{
			ChoosedCards.Add(card.Data.Id);
			OnCardAdded?.Invoke();
		}
		private void ResetList()
		{
			ChoosedCards = new();
		}
		private void OnPlayerStartMoving(int oldPointId)
		{
			leftCardData = null;
			rightCardData = null;
			groupData = null;
		}
		private void SetSubZone(SubZoneData data) => subZoneData = data;

		[ContextMenu("Randomize or load")]
		private void R()
		{
			GenerateCardData();
			leftCard.EnableCard();
			rightCard.EnableCard();
			UpdateUI();
		}
		public void SetNextCards(int cardGroup, int leftCard = -1, int rightCard = -1)
		{
			nextGroupId = cardGroup;
			nextLeftCardId = leftCard;
			nextRightCardId = rightCard;
		}
		private void ResetNextCards()
		{
			nextGroupId = -1;
			nextLeftCardId = -1;
			nextRightCardId = -1;
		}
		public void UpdateData()
		{
			if (groupData == null || leftCardData == null || rightCardData == null)
				GenerateCardData();
			leftCard.EnableCard();
			rightCard.EnableCard();
			backButton.SetActive(true);
			UpdateUI();
		}
		public void DisableBackButton() => backButton.SetActive(false);
		private void UpdateUI()
		{
			cardsRoot.SetActive(true);
			leftCard.Data = leftCardData;
			rightCard.Data = rightCardData;
			groupNameLanguage.Id = groupData.Id;
			groupDescriptionLanguage.Id = groupData.Id;
		}
		private void GenerateCardData()
		{
			groupData = nextGroupId < 0 ? RandomizeCardGroup() : CardsInfo.Instance.GetGroup(nextGroupId);
			groupData.GetAllowedCards(out List<CardData> optimalCards, out List<CardData> defaultCards, true);
			if (optimalCards.Count + defaultCards.Count < 2)
				groupData.GetAllowedCards(out optimalCards, out defaultCards, false);
			bool zeroCardsError1 = false;
			bool zeroCardsError2 = false;
			leftCardData = nextLeftCardId < 0 ? RandomizeCard(optimalCards, defaultCards, out zeroCardsError1) : CardsInfo.Instance.GetCard(nextLeftCardId);
			rightCardData = nextLeftCardId < 0 ? RandomizeCard(optimalCards, defaultCards, out zeroCardsError2) : CardsInfo.Instance.GetCard(nextRightCardId);

			if (zeroCardsError1 || zeroCardsError2)
			{
				ResetNextCards();
				GenerateCardData();
			}
			ResetNextCards();
		}
		private CardGroup RandomizeCardGroup()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			int playerLevel = playerData.Stats.ExperienceLevel.Level;

			List<CardGroupSO> allowedGroups = CardsInfo.Instance.CardGroups.Where(x =>
				x.CardGroup.Level <= playerLevel && x.CardGroup.Appearance.Contains(subZoneData.SubZone)).ToList();
			List<int> groupIds = new();
			for (int i = allowedGroups.Count - 1; i >= 0; --i)
			{
				CardGroup el = allowedGroups[i].CardGroup;
				el.GetAllowedCards(out List<CardData> oc, out _, true);
				if (oc.Count > 0)
					groupIds.Add(i);
			}
			int groupId = -1;
			bool getRandomGroup = CustomMath.GetRandomChance(30);
			if (GameData.Data.AdventureData.IsBossAllowedForPlayer(out _))
				getRandomGroup = false;

			if (groupIds.Count == 0 || getRandomGroup)
				groupId = Random.Range(0, allowedGroups.Count);
			else
				groupId = groupIds[Random.Range(0, groupIds.Count)];

			CardGroup choosedGroup = allowedGroups[groupId].CardGroup;
			return choosedGroup;
		}
		/// <summary>
		/// Warning, list will be modified and returned without choosed card
		/// </summary>
		/// <param name="optimalCards"></param>
		/// <param name="defaultCards"></param>
		/// <returns></returns>
		private CardData RandomizeCard(List<CardData> optimalCards, List<CardData> defaultCards, out bool zeroCardsError)
		{
			CardData cardData = null;
			int generated = -1;
			zeroCardsError = false;
			if (optimalCards.Count == 0 && defaultCards.Count == 0)
			{
				Debug.LogError($"Error - zero allowed cards count in {groupData.Id}. Fixing - Generate new clearly");
				zeroCardsError = true;
				return null;
			}

			bool getOptimalCard = CustomMath.GetRandomChance(55);
			if (GameData.Data.AdventureData.IsBossAllowedForPlayer(out _))
				getOptimalCard = CustomMath.GetRandomChance(70);

			if (optimalCards.Count > 0 && (getOptimalCard || defaultCards.Count == 0))
			{
				generated = Random.Range(0, optimalCards.Count);
				cardData = optimalCards[generated];
				optimalCards.RemoveAt(generated);
			}
			else
			{
				generated = Random.Range(0, defaultCards.Count);
				cardData = defaultCards[generated];
				defaultCards.RemoveAt(generated);
			}
			return cardData;
		}
		#endregion methods
	}
}