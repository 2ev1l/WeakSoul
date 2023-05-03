using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class GameData
	{
		#region fields & properties
		public static readonly string SaveName = "save";
		public static readonly string SaveExtension = ".data";
		public static bool CanSaveData = true;
		public static GameData Data
		{
			get => _data;
			set => SetData(value);
		}
		private static GameData _data;

		/// <summary>
		/// <see cref="{T0}"/> - totalTime;
		/// <see cref="{T1}"/> - increasedTime;
		/// </summary>
		public UnityAction<int, int> OnTimePlayedChanged;
		public UnityAction<int> OnDaysChanged;

		[SerializeField] private int _timePlayed = 0;
		[SerializeField] private int _days = -1;

		[SerializeField] private string _sceneName = "Cut Scene";
		[SerializeField] private int _cutSceneId = 0;

		[SerializeField] private PlayerData _playerData = new();
		[SerializeField] private ShopData _shopData = new();
		[SerializeField] private WitchData _witchData = new();
		[SerializeField] private BlacksmithData _blacksmithData = new();
		[SerializeField] private TutorialData _tutorialData = new();
		[SerializeField] private AdventureData _adventureData = new();
		[SerializeField] private TavernData _tavernData = new();

		public int TimePlayed
		{
			get => _timePlayed;
			set => SetTimePlayed(value);
		}
		public string SceneName
		{
			get => _sceneName;
			set => _sceneName = value;
		}
		public int CutSceneId { get => _cutSceneId; set => _cutSceneId = value; }
		public int Days
		{
			get => _days;
			set => SetDays(value);
		}

		public PlayerData PlayerData => _playerData;
		public ShopData ShopData => _shopData;
		public WitchData WitchData => _witchData;
		public BlacksmithData BlacksmithData => _blacksmithData;
		public TutorialData TutorialData => _tutorialData;
		public AdventureData AdventureData => _adventureData;
		public TavernData TavernData => _tavernData;
		#endregion fields & properties

		#region methods
		private void SetTimePlayed(int value)
		{
			if (value < 0)
				throw new System.ArgumentOutOfRangeException("time played must be >= 0");
			int oldValue = _timePlayed;
			int increase = value - _timePlayed;
			ItemsInventory playerInventory = PlayerData.Inventory;
			if (playerInventory.ContainItem(212) && !playerInventory.ContainItem(213))
			{
				value += CustomMath.Multiply(increase, Random.Range(0f, 500f));
			}
			if (playerInventory.ContainItem(213))
			{
				if (!playerInventory.ContainItem(212))
					return;
			}
			_timePlayed = value;
			OnTimePlayedChanged?.Invoke(_timePlayed, _timePlayed - oldValue);
		}
		private void SetDays(int value)
		{
			if (value < 0)
				throw new System.ArgumentOutOfRangeException("days must be >= 0");
			ItemsInventory playerInventory = PlayerData.Inventory;
			int oldValue = _days;
			int increase = value - _days;
			if (playerInventory.ContainItem(212) && !playerInventory.ContainItem(213))
			{
				value += CustomMath.Multiply(increase, Random.Range(0f, 500f));
			}
			if (playerInventory.ContainItem(213))
			{
				if (!playerInventory.ContainItem(212))
					return;
			}
			_days = value;
			OnDaysChanged?.Invoke(_days);
		}
		private static void SetData(GameData value) => _data = value ?? new GameData();
		#endregion methods
	}
}