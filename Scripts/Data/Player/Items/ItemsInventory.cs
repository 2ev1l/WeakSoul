using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class ItemsInventory : InventoryData
    {
        #region fields & properties
        public Armor HeadArmor => ItemsInfo.Instance.TryGetArmor(items[HeadCell]);
        public Weapon Weapon => ItemsInfo.Instance.TryGetWeapon(items[WeaponCell]);
        public Armor BodyArmor => ItemsInfo.Instance.TryGetArmor(items[BodyCell]);
        public Armor LegsArmor => ItemsInfo.Instance.TryGetArmor(items[LegsCell]);

        public static int[] EquipmentCells => new int[] { HeadCell, WeaponCell, BodyCell, LegsCell };
        [System.NonSerialized] public static readonly int HeadCell = 16;
        [System.NonSerialized] public static readonly int WeaponCell = 17;
        [System.NonSerialized] public static readonly int BodyCell = 18;
        [System.NonSerialized] public static readonly int LegsCell = 19;

        #endregion fields & properties

        #region methods
        public PhysicalStats GetEquippedStats()
        {
            PhysicalStats totalStats = new();
            List<PhysicalStats> newStats = new();
            if (items[HeadCell] != -1)
                newStats.Add(HeadArmor.Stats);
            if (items[WeaponCell] != -1)
                newStats.Add(Weapon.Stats);
            if (items[BodyCell] != -1)
                newStats.Add(BodyArmor.Stats);
            if (items[LegsCell] != -1)
                newStats.Add(LegsArmor.Stats);
            foreach (PhysicalStats el in newStats)
                totalStats.IncreaseStatsHidden(el);
            return totalStats;
        }
        public ItemsInventory()
        {
            Size = 4;
            items = new List<int>() {
                    3,-1,-1,-1,
                    -1,-1,-1,-1,
                    -1,-1,-1,-1,
                    -1,-1,-1,-1,

                    -1,-1,-1,-1, //equipment
                    -1}; //remove
        }
        #endregion methods
    }
}