using Data;
using Data.Adventure;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;
using WeakSoul.Adventure.Backpack;
using WeakSoul.GameMenu;
using UnityEngine.Events;

namespace WeakSoul.Adventure
{
	public class AdventureButtons : SingleSceneInstance
	{
		#region fields & properties
		public static UnityAction OnBack;
		public static AdventureButtons Instance { get; private set; }
		private static readonly int textEnterId = 5;
		private static readonly int textChooseId = 4;
		private static bool canIncreaseDays = false;
		private static int pointsCompleted = 0;

		[SerializeField] private GameObject buttonObject;
		[SerializeField] private GameObject backButton;
		[SerializeField] private GameObject enterInCityButton;
		[SerializeField] private LanguageLoader buttonLanguage;

		[SerializeField] private StateChange emptyState;
		[SerializeField] private StateChange cardsState;
		[SerializeField] private StateChange mainState;
		[SerializeField] private StateMachine stateMachine;
		private bool IsEnterEvent
		{
			get => isEnterEvent;
			set
			{
				isEnterEvent = value;
				buttonLanguage.Id = isEnterEvent ? textEnterId : textChooseId;
			}
		}
		private bool isEnterEvent = false;
		private int eventId = 0;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			SetEmptyState();
			Instance = this;
			CheckInstances(GetType());
		}
		private void OnEnable()
		{
			Player.OnStartMoving += OnPlayerStartMoving;
			Player.OnPointChanged += CheckPointId;
			PointsInit.Instance.OnEndGeneration += ResetValues;
			PointsInit.Instance.OnLoaded += SetMainState;
			PointsInit.Instance.OnStartGeneration += SetEmptyState;
		}
		private void OnDisable()
		{
			Player.OnStartMoving -= OnPlayerStartMoving;
			Player.OnPointChanged -= CheckPointId;
			PointsInit.Instance.OnEndGeneration -= ResetValues;
			PointsInit.Instance.OnLoaded -= SetMainState;
			PointsInit.Instance.OnStartGeneration -= SetEmptyState;
		}

		public void SetMainState()
		{
			stateMachine.TryApplyState(mainState);
		}
		public void SetEmptyState()
		{
			stateMachine.TryApplyState(emptyState);
		}
		private void ResetValues()
		{
			pointsCompleted = 0;
			canIncreaseDays = false;
		}
		private void OnPlayerStartMoving(int oldPointId) => SetEmptyState();
		private void CheckPointId(int oldPointId, int newPointId)
		{
			Point point = PointsInit.Instance.GetPoint(newPointId);
			eventId = point.Data.ChoosedEvent.Id;
			IsEnterEvent = eventId != 0 && eventId != 19;

			buttonObject.SetActive(true);
			if (!IsEnterEvent)
				CheckBackButton();
			else
				backButton.SetActive(false);
			enterInCityButton.SetActive(eventId == 19);

			pointsCompleted++;
			SetMainState();
		}
		public void EnterInCity()
		{
			PointData choosed = PointsInit.Instance.GetPointData(Player.CurrentPointId);
			choosed.SetEventHidden(MapEventsInfo.Instance.GetEvent(0));
			EventInfo.Instance.Data.SetMapEvent(MapEventsInfo.Instance.GetEvent(eventId));
			SceneLoader.Instance.LoadSceneFade("City", 2f);
		}
		public void OnButtonClick()
		{
			if (IsEnterEvent)
			{
				EventInfo.Instance.Data.LoadEvent(eventId);
				PointData choosed = PointsInit.Instance.GetPointData(Player.CurrentPointId);
				choosed.SetEventHidden(MapEventsInfo.Instance.GetEvent(0));
				return;
			}
			CardsPanel.Instance.UpdateData();
			stateMachine.TryApplyState(cardsState);
		}
		public void LoadGameMenu()
		{
			if (canIncreaseDays)
			{
				GameData.Data.Days++;
				KarmaReward.DoReward();
			}
			if (GameData.Data.AdventureData.TryLoadBossCutScene())
			{
				OnBack?.Invoke();
				return;
			}
			SceneLoader.Instance.LoadSceneFade("Game Menu", 2f);
			OnBack?.Invoke();
		}

		private void CheckBackButton()
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			bool zoneAllow = Player.Instance.SubZoneData.SubZone == SpawnSubZone.Home;
			bool pointsAllow = Player.CurrentPointId != 0;
			bool isPlayerLevelSmall = playerLevel <= 7 && GameData.Data.TutorialData.IsCompleted;
			bool timePointsAllow = pointsCompleted >= (5 + playerLevel / 5f);
			backButton.SetActive((zoneAllow && pointsAllow) || CustomMath.GetRandomChance(10));
			if (!zoneAllow || isPlayerLevelSmall || timePointsAllow)
			{
				canIncreaseDays = true;
			}
		}

		[ContextMenu("Allow reward and days increase")]
		private void IncreaseDays() => canIncreaseDays = true;
		#endregion methods
	}
}