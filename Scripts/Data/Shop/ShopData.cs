using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace Data
{
	[System.Serializable]
	public class ShopData
	{
		#region fields & properties
		public UnityAction<IEnumerable<ShopItem>> OnItemsGenerated;
		public IEnumerable<ShopItem> Items => items;
		[SerializeField] private List<ShopItem> items;
		#endregion fields & properties

		#region methods
		public void GenerateItems(int weapons = 4, int armors = 6, int soulItems = 3, bool includeUniqueItems = false, float randomChance = 40, bool includeCells = true, int playerLevelChange = 0)
		{
			items = new List<ShopItem>();
			items.AddRange(GenerateBaseItems(weapons, armors, soulItems, includeUniqueItems, randomChance, playerLevelChange));
			if (includeCells)
				items.AddRange(GenerateCells());
			OnItemsGenerated?.Invoke(Items);
		}
		private List<ShopItem> GenerateBaseItems(int weapons = 4, int armors = 6, int soulItems = 3, bool includeUniqueItems = false, float randomChance = 40, int playerLevelChange = 0)
		{
			List<ShopItem> shopItems = new();
			shopItems.AddRange(GenerateBaseItems(weapons, ShopItemType.Weapon, ItemsInfo.Instance.GetWeapons(), includeUniqueItems, randomChance / 1.6f, playerLevelChange));
			shopItems.AddRange(GenerateBaseItems(armors, ShopItemType.Armor, ItemsInfo.Instance.GetArmors(), includeUniqueItems, randomChance, playerLevelChange));
			if (TryAddWaystone(out ShopItem wayStone) && soulItems > 0)
			{
				shopItems.Add(wayStone);
				soulItems--;
			}
			shopItems.AddRange(GenerateBaseItems(soulItems, ShopItemType.SoulItem, ItemsInfo.Instance.GetSoulItems(), includeUniqueItems, randomChance / 1.7f, playerLevelChange));
			return shopItems;
		}
		private bool TryAddWaystone(out ShopItem item)
		{
			item = null;
			PlayerData playerData = GameData.Data.PlayerData;
			int playerLevel = playerData.Stats.ExperienceLevel.Level;
			if (playerData.Inventory.ContainItem(6)) return false;
			List<int> waystoneEffects = new() { 197, 198, 199 };
			List<int> allowedEffects = new() { 6 };
			foreach (var el in waystoneEffects)
			{
				if (ItemsInfo.Instance.GetItem(el).Level <= playerLevel)
					allowedEffects.Add(el);
			}
			item = new(allowedEffects[Random.Range(0, allowedEffects.Count)], ShopItemType.SoulItem);
			return true;
		}
		private List<ShopItem> GenerateBaseItems(int size, ShopItemType type, List<Item> items, bool includeUniqueItems = false, float randomChance = 40, int playerLevelChange = 0)
		{
			List<ShopItem> shopItems = new();
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level + playerLevelChange;
			items = items.OrderBy(x => x.Level).ToList();
			if (playerLevel >= 52)
				includeUniqueItems = true;
			for (int i = items.Count - 1; i >= 0; --i)
			{
				if (size <= shopItems.Count) return shopItems;
				Item el = items[i];
				if (playerLevel < el.Level || !el.CanBuy) continue;

				bool canAdd = CustomMath.GetRandomChance(randomChance);
				if (!includeUniqueItems)
					canAdd = canAdd && el.Price.UniqueSouls == 0;
				else if (CustomMath.GetRandomChance(randomChance))
					canAdd = canAdd && el.Price.UniqueSouls > 0;

				if (!canAdd) continue;
				ShopItem item = new(items[i].Id, type);
				shopItems.Add(item);
			}
			return shopItems;
		}
		private List<ShopCell> GenerateCells()
		{
			List<ShopCell> shopItems = new();
			List<ShopCellInfo> cellsInfos = CellsInfo.Instance.Infos.ToList();
			for (int i = 0; i < cellsInfos.Count; ++i)
			{
				ShopCellInfo el = cellsInfos[i];
				ShopCellValue value = null;
				try { value = el.GetLastAllowedValue(); }
				catch { continue; }
				if (value == null) continue;
				shopItems.Add(new ShopCell(i, ShopItemType.Cell, value.Value));
			}
			return shopItems;
		}
		public void RemoveItem(ShopItem item) => items.Remove(item);
		public void RemoveItem(int id, ShopItemType type) => items.Remove(items.Find(x => x.Id == id && x.Type == type));
		public void GenerateTutorialData()
		{
			items = new List<ShopItem>();
			ShopItem item = new(0, ShopItemType.Armor);
			items.Add(item);
			OnItemsGenerated?.Invoke(items);
		}
		#endregion methods
	}
}