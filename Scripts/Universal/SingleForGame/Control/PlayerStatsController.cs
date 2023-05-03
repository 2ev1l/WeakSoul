using Data;
using Data.Adventure;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WeakSoul.Adventure;
using WeakSoul.Adventure.Map;
using WeakSoul.Events.Fight;
using WeakSoul.GameMenu.Inventory;

namespace Universal
{
	public class PlayerStatsController : MonoBehaviour
	{
		#region fields & properties
		/// <summary>
		/// <see cref="{T0}"/> itemId;
		/// </summary>
		public static UnityAction<int> OnItemEquipped;
		/// <summary>
		/// <see cref="{T0}"/> itemId;
		/// </summary>
		public static UnityAction<int> OnItemDequipped;
		public static PlayerStatsController Instance { get; private set; }
		private PhysicalStats AdventureDefaultStats
		{
			get => adventureDefaultStats;
			set => adventureDefaultStats = value;
		}
		private PhysicalStats adventureDefaultStats;
		[SerializeField] private InventoryCellController inventoryCellController;
		#endregion fields & properties

		#region methods
		public void Init()
		{
			Instance = this;
			GameData.Data.PlayerData.Stats.Init();
		}
		private void OnEnable()
		{
			GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckCellEquipped;
			GameData.Data.PlayerData.Stats.OnDead += OnDead;
			GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += OnLevelGain;
			AdventureButtons.OnBack += ResetStatsBackWithSaveEffects;
			WeakSoul.Adventure.Backpack.SoulItem.OnWaystoneUsed += AddNegativeEffect;
			SavingUtils.OnBeforeSave += GameData.Data.PlayerData.Stats.StoreLastEquipment;
			Card.OnCardChoosed += OnAdventureCardChoosed;
			GameData.Data.PlayerData.Stats.OnEffectRemoved += OnEffectRemoved;
			EnemyCard.OnEnemyDefeated += OnEnemyDefeated;
		}
		private void OnDisable()
		{
			GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckCellEquipped;
			GameData.Data.PlayerData.Stats.OnDead -= OnDead;
			GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= OnLevelGain;
			AdventureButtons.OnBack -= ResetStatsBackWithSaveEffects;
			WeakSoul.Adventure.Backpack.SoulItem.OnWaystoneUsed -= AddNegativeEffect;
			SavingUtils.OnBeforeSave -= GameData.Data.PlayerData.Stats.StoreLastEquipment;
			Card.OnCardChoosed -= OnAdventureCardChoosed;
			GameData.Data.PlayerData.Stats.OnEffectRemoved -= OnEffectRemoved;
			EnemyCard.OnEnemyDefeated -= OnEnemyDefeated;
		}
		private void OnEnemyDefeated()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			ItemsInventory playerInventory = playerData.Inventory;
			if (playerInventory.ContainItem(340))
			{
				AdventureDefaultStats.Health += 5;
			}
		}
		private void OnEffectRemoved(Effect effect)
		{
			PlayerData playerData = GameData.Data.PlayerData;
			bool IsSoulItem_291 = playerData.Inventory.ContainItem(291);
			int healthIncrease = 4;
			if (IsSoulItem_291)
			{
				playerData.Stats.Health += healthIncrease;
				if (SceneManager.GetActiveScene().name.Equals("Game Menu")) return;
				AdventureDefaultStats.Health += healthIncrease;
			}
		}
		private void OnAdventureCardChoosed(Card card)
		{
			int cardId = card.Data.Id;
			if (!GameData.Data.PlayerData.Inventory.ContainItem(210) || CardsPanel.ChoosedCards.Contains(cardId)) return;
			GameData.Data.PlayerData.Stats.ExperienceLevel.Experience++;
		}
		private void OnLevelGain(int level)
		{
			if (SceneManager.GetActiveScene().name.Equals("Game Menu")) return;
			if (AdventureDefaultStats == null)
			{
				Debug.LogError("Error - Stored stats is null. DATA MAY BE CORRUPTED");
				CollectStats();
			}
			AdventureDefaultStats.IncreaseStatsHidden(LevelsInfo.Instance.GetLevel(level - 1).Reward.Stats);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Clone</returns>
		public PhysicalStats GetDefaultStats()
		{
			if (AdventureDefaultStats == null)
			{
				Debug.LogError("Error - Stored stats is null. DATA MAY BE CORRUPTED");
				CollectStats();
			}
			return AdventureDefaultStats.Clone();
		}
		[ContextMenu("Collect stats")]
		public void CollectStats()
		{
			PhysicalStats stats = new();
			PlayerData playerData = GameData.Data.PlayerData;
			stats.IncreaseStatsHidden(playerData.Stats);
			stats.DecreaseStatsHidden(playerData.Inventory.GetEquippedStats());
			stats.DecreaseStatsHidden(playerData.Stats.GetEffectsStats());
			AdventureDefaultStats = stats;
		}
		private void OnDead()
		{
			GameData.Data.SceneName = "Game Menu";
			PlayerData playerData = GameData.Data.PlayerData;
			if (!SceneManager.GetActiveScene().name.Equals("Game Menu"))
				RemoveInventoryItems();
			ResetStatsBack(false);
			playerData.Wallet.WeakSouls = Mathf.RoundToInt(playerData.Wallet.WeakSouls * 0.7f);
			playerData.Wallet.NormalSouls = Mathf.RoundToInt(playerData.Wallet.NormalSouls * 0.8f);
			playerData.Wallet.StrongSouls = Mathf.RoundToInt(playerData.Wallet.StrongSouls * 0.9f);
			GameData.Data.Days++;
			//print("restored health with equipment = " + GameData.Data.PlayerData.Stats.Health);
			CheckClassOnDead();
			AddNegativeEffect();
			SavingUtils.SaveGameData();
		}
		private void CheckClassOnDead()
		{
			PlayerStats playerStats = GameData.Data.PlayerData.Stats;
			switch (playerStats.Class)
			{
				case PlayerClass.PosthumousHero:
					playerStats.Health += 3;
					playerStats.Damage += 1;
					playerStats.EvasionChance -= 1;
					playerStats.CriticalChance -= 1;
					playerStats.ChangeKarmaBy(3);
					break;
				default: break;
			}
		}
		private void AddNegativeEffect()
		{
			bool f = CustomMath.GetRandomChance(50);
			ChestData data = new(0, GameData.Data.PlayerData.Stats.ExperienceLevel.Level - 3, f ? ChestTier.Terrible : ChestTier.Bad, 0, f ? 1 : 2);
			List<RewardData> rewards = data.GetReward();
			rewards.ForEach(x => x.TryAddReward(out _));
		}
		private void ResetStatsBackWithSaveEffects() => ResetStatsBack(true);
		private void ResetStatsBack(bool saveEffects)
		{
			PlayerData playerData = GameData.Data.PlayerData;
			//copy of destroyable effects
			List<Effect> effects = saveEffects ? (playerData.Stats.Effects.Count() > 0 ? playerData.Stats.Effects.Where(x => x.IsDestroyable).ToList() : new()) : new();

			if (AdventureDefaultStats == null)
			{
				Debug.LogError("Error - Stored stats is null. DATA MAY BE CORRUPTED");
				CollectStats();
			}
			//not destroyable effects remains
			playerData.Stats.TryRemoveAllEffects(false);
			playerData.Stats.ResetStats();
			playerData.Stats.IncreaseStatsHidden(AdventureDefaultStats);
			playerData.Stats.IncreaseStatsHidden(playerData.Inventory.GetEquippedStats());
			playerData.Stats.Stamina = Mathf.Clamp(playerData.Stats.Stamina, 0, 2);
			//applying buffes back for non destroyable effects
			playerData.Stats.AddAllEffectBuffes();
			if (saveEffects)
				playerData.Stats.AddOrStackEffects(effects);
		}
		private void RemoveInventoryItems()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			List<int> items = playerData.Inventory.Items.ToList();
			for (int i = 0; i < 16; ++i)
			{
				if (items[i] == -1 || CustomMath.GetRandomChance(30)) continue;
				if (ItemsInfo.Instance.GetItem(items[i]).IsDestroyable)
					playerData.Inventory.RemoveItem(i);
			}
		}
		private void CheckCellEquipped(int itemId, int newCellId, int oldCellId)
		{
			Cell startCell = inventoryCellController.StartCell;
			if (startCell == null) return;
			if (!ItemsInventory.EquipmentCells.Contains(newCellId) && !ItemsInventory.EquipmentCells.Contains(startCell.Index)) return;

			PhysicalStats newCellStats = new();
			try { newCellStats = ItemsInfo.Instance.GetPhysicalStats(itemId); }
			catch { return; }
			switch (newCellId)
			{
				case 16: UpdateAffectedStats(true, false, newCellStats); OnItemEquipped?.Invoke(itemId); break;
				case 17: UpdateAffectedStats(true, true, newCellStats); OnItemEquipped?.Invoke(itemId); break;
				case 18: UpdateAffectedStats(true, false, newCellStats); OnItemEquipped?.Invoke(itemId); break;
				case 19: UpdateAffectedStats(true, false, newCellStats); OnItemEquipped?.Invoke(itemId); break;
			}
			switch (oldCellId)
			{
				case 16: UpdateAffectedStats(false, false, newCellStats); OnItemDequipped?.Invoke(itemId); break;
				case 17: UpdateAffectedStats(false, true, newCellStats); OnItemDequipped?.Invoke(itemId); break;
				case 18: UpdateAffectedStats(false, false, newCellStats); OnItemDequipped?.Invoke(itemId); break;
				case 19: UpdateAffectedStats(false, false, newCellStats); OnItemDequipped?.Invoke(itemId); break;
			}
		}
		private void UpdateAffectedStats(bool increaseStats, bool isWeapon, PhysicalStats stats) =>
			UpdateAffectedStats(GameData.Data.PlayerData.Stats, increaseStats, isWeapon, stats);
		private PhysicalStats UpdateAffectedStats(PhysicalStats changingStats, bool increaseStats, bool isWeapon, PhysicalStats physicalStats)
		{
			if (increaseStats)
				changingStats.IncreaseStats(physicalStats);
			else
				changingStats.DecreaseStats(physicalStats);
			if (isWeapon)
				changingStats.DamageType = increaseStats ? physicalStats.DamageType : DamageType.Physical;
			return changingStats;
		}

		[ContextMenu("Add damage")]
		private void AddDamage() => GameData.Data.PlayerData.Stats.Damage += damageToAdd;
		[SerializeField] private int damageToAdd;

		[ContextMenu("Add health")]
		private void AddHealth() => GameData.Data.PlayerData.Stats.Health += healthToAdd;
		[SerializeField] private int healthToAdd;

		[ContextMenu("Add def")]
		private void AddDef() => GameData.Data.PlayerData.Stats.Defense += defToAdd;
		[SerializeField] private int defToAdd;

		[ContextMenu("Add res")]
		private void AddRes() => GameData.Data.PlayerData.Stats.Resistance += resToAdd;
		[SerializeField] private int resToAdd;

		[ContextMenu("Add critical scale")]
		private void AddCS() => GameData.Data.PlayerData.Stats.CriticalScale += csToAdd;
		[SerializeField] private int csToAdd;
		[ContextMenu("Add evasion chance")]
		private void AddEC() => GameData.Data.PlayerData.Stats.EvasionChance += ecToAdd;
		[SerializeField] private int ecToAdd;
		[ContextMenu("Add stamina regen")]
		private void AddSR() => GameData.Data.PlayerData.Stats.StaminaRegen += srToAdd;
		[SerializeField] private int srToAdd;

		[ContextMenu("Health to zero")]
		private void HTZ()
		{
			if (AdventureDefaultStats == null)
			{
				Debug.LogError("Error - Stored stats is null. DATA MAY BE CORRUPTED");
				CollectStats();
			}
			GameData.Data.PlayerData.Stats.Health = 0;
		}

		[ContextMenu("Add Soul life")]
		private void AddSoulLife() => GameData.Data.PlayerData.Stats.SoulLife += soulLifeToAdd;
		[SerializeField] private int soulLifeToAdd;

		[ContextMenu("SOUL DEAD")]
		private void SD()
		{
			GameData.Data.PlayerData.Stats.SoulLife = 0;
			HTZ();
		}

		[ContextMenu("Change class to ")]
		private void CCT() => GameData.Data.PlayerData.Stats.SetPlayerClass(classToSet);
		[SerializeField] private PlayerClass classToSet;
		[ContextMenu("Change damage type to")]
		private void CDTT() => GameData.Data.PlayerData.Stats.DamageType = damageTypeToSet;
		[SerializeField] private DamageType damageTypeToSet;

		[ContextMenu("Add experience")]
		private void AddExp() => GameData.Data.PlayerData.Stats.ExperienceLevel.Experience += expToAdd;
		[SerializeField] private int expToAdd;
		#endregion methods
	}
}