using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    public class ItemsInfo : MonoBehaviour
    {
        #region fields & properties
        public static ItemsInfo Instance { get; private set; }
        [SerializeField] private int idToAdd;

        [field: SerializeField] public List<WeaponSO> Weapons { get; private set; } = new();
        [field: SerializeField] public List<ArmorSO> Armors { get; private set; } = new();
        [field: SerializeField] public List<SoulItemSO> SoulItems { get; private set; } = new();
        public List<Item> Items { get; private set; } = new();
        public List<StatsItem> StatsItems { get; private set; } = new();
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            List<Item> list = new();
            foreach (var el in Weapons.Where(x => x != null))
                list.Add(el.Weapon);
            foreach (var el in Armors.Where(x => x != null))
                list.Add(el.Armor);
            foreach (var el in SoulItems.Where(x => x != null))
                list.Add(el.SoulItem);
            Items = list.OrderBy(x => x.Id).ToList();

            List<StatsItem> list2 = new();
            for (int i = 0; i < Items.Count; i++)
                list2.Add(null);
            foreach (var el in Weapons.Where(x => x != null))
                list2[el.Weapon.Id] = el.Weapon;
            foreach (var el in Armors.Where(x => x != null))
                list2[el.Armor.Id] = el.Armor;
            StatsItems = list2;

            foreach (var el in Items)
            {
                if (Items.Where(x => x.Id == el.Id).Count() > 1)
                {
                    Debug.LogError($"Error id {el.Id} at {el.Texture}");
                }
            }
        }
        public Weapon GetWeapon(int itemId) => Weapons[itemId].Weapon;
        public Weapon TryGetWeapon(int itemId)
        {
            WeaponSO i = null;
            try { i = Weapons[itemId]; }
            catch { return null; }
            if (i == null) return null;
            return i.Weapon;
        }
        public Armor GetArmor(int itemId) => Armors[itemId].Armor;
        public Armor TryGetArmor(int itemId)
        {
            ArmorSO i = null;
            try { i = Armors[itemId]; }
            catch { return null; }
            if (i == null) return null;
            return i.Armor;
        }
        public SoulItem GetSoulItem(int itemId) => SoulItems[itemId].SoulItem;
        public SoulItem TryGetSoulItem(int itemId)
        {
            SoulItemSO i = null;
            try { i = SoulItems[itemId]; }
            catch { return null; }
            if (i == null) return null;
            return i.SoulItem;
        }
        public Item GetItem(int itemId) => Items[itemId];
        public StatsItem GetStatsItem(int itemId) => StatsItems[itemId];
        public ItemType GetItemType(int itemId)
        {
            if (TryGetArmor(itemId) != null)
                return ItemType.Armor;
            if (TryGetWeapon(itemId) != null)
                return ItemType.Weapon;
            return ItemType.SoulItem;
        }
        public PhysicalStats GetPhysicalStats(int itemId) => GetStatsItem(itemId).Stats;
        public List<Item> GetWeapons()
        {
            List<Item> n = new();
            foreach (var el in Weapons.Where(x => x != null))
                n.Add(el.Weapon);
            return n;
        }
        public List<Item> GetArmors()
        {
            List<Item> n = new();
            foreach (var el in Armors.Where(x => x != null))
                n.Add(el.Armor);
            return n;
        }
        public List<Item> GetSoulItems()
        {
            List<Item> n = new();
            foreach (var el in SoulItems.Where(x => x != null))
                n.Add(el.SoulItem);
            return n;
        }
        public List<PhysicalStatsType> GetEnabledStatsList(int itemId)
        {
            ItemType type = GetItemType(itemId);
            return type switch
            {
                ItemType i when i == ItemType.Armor || i == ItemType.Weapon => GetPhysicalStats(itemId).GetEnabledStatsListInherit(type),
                ItemType.SoulItem => new List<PhysicalStatsType>(),
                _ => throw new System.NotImplementedException()
            };
        }

        [ContextMenu("Add item")]
        private void AddItem() => AddItem(idToAdd);
		[ContextMenu("Add items to last increase")]
		private void AddItemsTI() 
        {
            ItemsInventory ii = GameData.Data.PlayerData.Inventory;
            while (ii.GetFreeCell() != -1)
            {
                AddItem(idToAdd);
                idToAdd++;
            }
        }
		private void AddItem(int id)
		{
			int freeCell = GameData.Data.PlayerData.Inventory.GetFreeCell();
			if (freeCell == -1)
			{
				print("no free cells");
				return;
			}
			GameData.Data.PlayerData.Inventory.SetItem(id, freeCell);
		}
		[ContextMenu("Get all")]
        private void Get()
        {
            Weapons = (Resources.FindObjectsOfTypeAll<WeaponSO>().OrderBy(x => x.Weapon.Id)).ToList();
            Armors = (Resources.FindObjectsOfTypeAll<ArmorSO>().OrderBy(x => x.Armor.Id)).ToList();
            SoulItems = (Resources.FindObjectsOfTypeAll<SoulItemSO>().OrderBy(x => x.SoulItem.Id)).ToList();
            Sort();
        }
        private void Sort()
        {
            int wMax = Weapons.Max(x => x.Weapon.Id);
            int aMax = Armors.Max(x => x.Armor.Id);
            int sMax = SoulItems.Max(x => x.SoulItem.Id);
            int maxIndex = Mathf.Max(wMax, aMax, sMax);
            int countIndex = 0;

            List<WeaponSO> w = new();
            for (int i = 0; i <= maxIndex; ++i)
            {
                WeaponSO el = null;
                try { el = Weapons[countIndex]; } catch { }
                if (el == null || el.Weapon.Id != i)
                {
                    w.Add(null);
                    continue;
                }
                w.Add(el);
                countIndex++;
            }

            countIndex = 0;
            List<ArmorSO> a = new();
            for (int i = 0; i <= maxIndex; ++i)
            {
                ArmorSO el = null;
                try { el = Armors[countIndex]; } catch { }
                if (el == null || el.Armor.Id != i)
                {
                    a.Add(null);
                    continue;
                }
                a.Add(el);
                countIndex++;
            }

            countIndex = 0;
            List<SoulItemSO> s = new();
            for (int i = 0; i <= maxIndex; ++i)
            {
                SoulItemSO el = null;
                try { el = SoulItems[countIndex]; } catch { }
                if (el == null || el.SoulItem.Id != i)
                {
                    s.Add(null);
                    continue;
                }
                s.Add(el);
                countIndex++;
            }
            Weapons = w;
            Armors = a;
            SoulItems = s;
        }

        [ContextMenu("Get all cells")]
        private void GAC() => GameData.Data.PlayerData.Inventory.Size = 16;
        [ContextMenu("Get fight test artifacts")]
        private void GFTA()
        {
            GAC();
            AddItem(8);
			AddItem(74);
			AddItem(90);
		}
		[ContextMenu("Get cards test artifacts")]
        private void GCTA()
        {
			GAC();
			AddItem(13);
			AddItem(14);
			AddItem(82);
		}
		#endregion methods
	}
}