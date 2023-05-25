using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.Adventure.Map;
using WeakSoul.Events.Teleport;
using WeakSoul.GameMenu;

namespace WeakSoul.Adventure.Backpack
{
	public class SoulItem : MonoBehaviour, IListUpdater
	{
		#region fields & properties
		/// <summary>
		/// <see cref="{T0}"/> - itemId;
		/// </summary>
		public static UnityAction<int> OnSoulItemUsed;
        /// <summary>
        /// <see cref="{T0}"/> - itemId;
        /// </summary>
        public static UnityAction<int> OnSoulItemDestroyed;
		public static UnityAction OnWaystoneUsed;
		public GameObject rootObject => gameObject;
		public int listParam => soulItem.Id;
		private Data.SoulItem soulItem;

		[Header("UI")]
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private ShowItemHelp showItemHelp;
		#endregion fields & properties

		#region methods
		public void OnListUpdate(int param)
		{
			soulItem = ItemsInfo.Instance.GetSoulItem(param);
			spriteRenderer.sprite = soulItem.Texture;
			showItemHelp.ItemId = param;
		}
		public void UseSoulItem()
		{
			switch (soulItem.Id)
			{
				case 6: UseWaystone(); break;
				case 79: UseToDefaultFight(); break;
				case 80: UseTeleport(); break;
				case 81: UseTeleport(); break;
				case 197: UseWaystone(); break;
				case 198: UseWaystone(); break;
				case 199: UseWaystone(); break;
				case 200: UseTeleport(); break;
				case 216: UsePetrifiedHeart(); break;
				case 217: UseWealthCard(); break;
				case 218: UseRitualClock(); break;
				case 219: UseToPlayerDirection(Direction.RND, Direction.RND, Direction.RND); break;
				case 220: UseToPlayerDirection(Direction.N); break;
				case 221: UseToPlayerDirection(Direction.S); break;
				case 222: UseToPlayerDirection(Direction.W); break;
				case 223: UseToPlayerDirection(Direction.E); break;
				case 224: UseDevilsHeart(); break;
				case 284: UseDevilsHand(); break;
				case 288: UseExplosiveAmulet(); break;
				case 289: UseDesertCard(); break;
				case 335: UseJewelBox(10); break;
				case 336: UseJewelBox(5); break;
				case 342: UsePadnorasBox(); break;
				default: throw new System.NotImplementedException($"{soulItem.Id} Id for soul item isn't implemented");
			}
			OnSoulItemUsed?.Invoke(soulItem.Id);

            if (TryBreak())
			{
				DoBreakEffect();
				OnSoulItemDestroyed?.Invoke(soulItem.Id);
			}
			else
				DoDefaultEffect();
		}
		private bool TryBreak()
		{
			if (!CustomMath.GetRandomChance(soulItem.BreakChance) || !IsSoulItemAllowedToBreakNow()) return false;
			InventoryData inventory = GameData.Data.PlayerData.Inventory;
			int soulItemIndex = inventory.FindItemIndex(soulItem.Id);
			inventory.RemoveItem(soulItemIndex);
			return true;
		}
		private bool IsSoulItemAllowedToBreakNow() => soulItem.Id switch
		{
			6 => GameData.Data.PlayerData.Stats.ExperienceLevel.Level >= 6,
			_ => true,
		};
		private void ClosePanel() => AdventureButtons.Instance.SetMainState();
		private GameObject SpawnEffect(GameObject effectPrefab)
		{
			GameObject inst = Instantiate(effectPrefab, transform.position, Quaternion.identity);
			Vector3 pos = inst.transform.localPosition;
			pos.z = 0;
			inst.transform.localPosition = pos;
			inst.SetActive(true);
			return inst;
		}
		private void DoBreakEffect() => SpawnEffect(BackpackPanel.Instance.BreakEffectPrefab);
		private void DoDefaultEffect() => SpawnEffect(BackpackPanel.Instance.SoulItemDefaultEffectPrefab);

		[ContextMenu("Use fight")]
		private void UseToDefaultFight()
		{
			EventInfo.Instance.Data.LoadEvent(1);
		}
		[ContextMenu("Use teleport")]
		private void UseTeleport()
		{
			List<PointData> allowedPoints = PointsInit.GeneratedPointsData.Where(x => x.ChoosedEvent.Id == 0).ToList();
			int randomPoint = allowedPoints[Random.Range(0, allowedPoints.Count)].PointId;
			Player.CurrentPointId = randomPoint;
		}
		private void UseWaystone()
		{
			AdventureButtons.Instance.LoadGameMenu();
			OnWaystoneUsed?.Invoke();
		}
		private void UsePetrifiedHeart()
		{
			PlayerStats stats = GameData.Data.PlayerData.Stats;
			stats.Health = CustomMath.Multiply(stats.Health, 105);
			stats.CriticalChance -= 3;
		}
		private void UseWealthCard()
		{
			bool getLoot = CustomMath.GetRandomChance(30);
			if (!getLoot) return;
			bool getUniqueSoul = CustomMath.GetRandomChance(30);
			Wallet playerWallet = GameData.Data.PlayerData.Wallet;
			SoulType choosedSoul = getUniqueSoul ? SoulType.Unique : SoulType.Strong;
			playerWallet.SetSoulsByType(playerWallet.GetSoulsByType(choosedSoul) + 1, choosedSoul);
		}
		private void UseRitualClock()
		{
			GameData.Data.TimePlayed += 60;
		}
		private void UseToPlayerDirection(params Direction[] directions)
		{
			Player.Instance.MoveToDirections(directions);
		}
		private void UseDevilsHeart()
		{
			List<int> bossPoints = PointsInit.GeneratedPointsData.Where(x => x.ChoosedEvent.Id == 9).Select(x => x.PointId).ToList();
			Player.CurrentPointId = bossPoints[Random.Range(0, bossPoints.Count)];
		}
		private void UseDevilsHand()
		{
			List<PointData> fightPoints = PointsInit.GeneratedPointsData.Where(x => x.ChoosedEvent.Id == 1).ToList();
			int pointsToDelete = CustomMath.Multiply(fightPoints.Count, 15);
			SetPointsToDefault(fightPoints, pointsToDelete);
			GameData.Data.PlayerData.Stats.ExperienceLevel.Experience += Random.Range(20, 40);
		}
		private void UseExplosiveAmulet()
		{
			PlayerData playerData = GameData.Data.PlayerData;
			int healthToDecrease = CustomMath.Multiply(playerData.Stats.Health, 20);
			healthToDecrease = Mathf.Max(healthToDecrease, 20);
			playerData.Stats.Damage = CustomMath.Multiply(playerData.Stats.Damage, 110);
			playerData.Stats.Health -= healthToDecrease;
		}
		private void UseDesertCard()
		{
			List<PointData> notNullPoints = PointsInit.GeneratedPointsData.Where(x => x.ChoosedEvent.Id > 0 && x.ChoosedEvent.Id < 9).ToList();
			int pointsToDelete = CustomMath.Multiply(notNullPoints.Count, 15);
			SetPointsToDefault(notNullPoints, pointsToDelete);
		}
		private void UseJewelBox(float chance)
		{
			if (!CustomMath.GetRandomChance(chance)) return;
			Wallet playerWallet = GameData.Data.PlayerData.Wallet;
			playerWallet.LegendarySouls += 1;
		}
		private void UsePadnorasBox()
		{
			List<Item> items = ItemsInfo.Instance.GetSoulItems();
			int rnd = Random.Range(0, items.Count);
			int itemId = items[rnd].Id;
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			int invIndex = inventory.GetFreeCell();
			if (invIndex == -1) return;
			inventory.SetItem(itemId, invIndex);
		}

		/// <summary>
		/// Warning, List will be modified -> without removed points
		/// </summary>
		/// <param name="points"></param>
		/// <param name="count"></param>
		private void SetPointsToDefault(List<PointData> points, int count)
		{
			count = Mathf.Min(count, points.Count);
			for (int i = 0; i < count; ++i)
			{
				PointData rnd = points[Random.Range(0, points.Count)];
				points.Remove(rnd);
				rnd.SetEvent(MapEventsInfo.Instance.GetEvent(0));
			}
			Player.Instance.MoveToDirections();
		}
		#endregion methods
	}
}