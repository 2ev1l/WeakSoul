using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;
using WeakSoul.City.Tavern;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.City
{
	public class SceneInit : SingleSceneInstance
	{
		#region fields & properties
		public static SceneInit Instance { get; private set; }
		[SerializeField] private StateMachine citiesStateMachine;
		[SerializeField] private ShopInit shopInit;
		[SerializeField] private TavernInit tavernInit;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
			SetCurrentCity();
			UpdateUI();
			HealPlayer();
			GetReward();
		}

		public void SetCurrentCity()
		{
			SpawnSubZone zone = EventInfo.Instance.Data.SubZoneData.SubZone;
			if (zone == SpawnSubZone.Home)
			{
				citiesStateMachine.TryApplyState(0);
				return;
			}
			foreach (ZoneState el in citiesStateMachine.States.ToList().Cast<ZoneState>())
			{
				if (el.SubZone == zone)
				{
					citiesStateMachine.TryApplyState(el);
					return;
				}
			}
			Debug.LogError($"Error - {zone} spawn sub zone for city isn't implemented. Fixing - Set to default");
			citiesStateMachine.TryApplyState(0);
		}
		private void UpdateUI()
		{
			GameData.CanSaveData = false;
			GameData.Data.WitchData.GenerateItems();
			ShopData shopData = new();
			shopData.GenerateItems(2, 4, 1, false, 30, false, 1);
			shopInit.Init(shopData);
			if (GameData.Data.TavernData.LastDayGenerated == 0 || (GameData.Data.Days - GameData.Data.TavernData.LastDayGenerated - 7 >= 0))
			{
				GameData.Data.TavernData.GenerateData();
			}
			tavernInit.Init(GameData.Data.TavernData);
		}
		private void HealPlayer()
		{
			PhysicalStats stats = PlayerStatsController.Instance.GetDefaultStats();
			PlayerStats playerStats = GameData.Data.PlayerData.Stats;
			stats.IncreaseStatsHidden(GameData.Data.PlayerData.Inventory.GetEquippedStats());
			int healthToHeal = Mathf.Max(stats.Health - playerStats.Health, 1);
			playerStats.Health += healthToHeal;
		}
		private void GetReward()
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			playerLevel += 1;
			BlacksmithData blacksmithData = GameData.Data.BlacksmithData;
			HashSet<int> openedRecipes = blacksmithData.OpenedRecipes.ToHashSet();
			List<CraftRecipeSO> allowedRecipes = RecipesInfo.Instance.Recipes.Where(x => x.Recipe.Level <= playerLevel && !openedRecipes.Contains(x.Recipe.Id)).ToList();
			if (allowedRecipes.Count == 0) return;
			int index = Random.Range(0, allowedRecipes.Count);
			blacksmithData.TryOpenRecipe(allowedRecipes[index].Recipe.Id);
		}
		public void BackToAdventure()
		{
			SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
		}
		[ContextMenu("Test")]
		private void sdsdds()
		{
			GameData.Data.TavernData.TryRemoveEnemy(32);
			GameData.Data.TavernData.TryRemoveEnemy(17);
			GameData.Data.TavernData.TryRemoveEnemy(15);
			tavernInit.Init(GameData.Data.TavernData);
		}
		[ContextMenu("Test 2")]
		private void sdsdds2()
		{
			GameData.Data.TavernData.GenerateData();
			tavernInit.Init(GameData.Data.TavernData);
		}
		#endregion methods
	}
}