using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class Armor: StatsItem
    {
        #region fields & properties
        public ArmorType ArmorType => armorType;
        [SerializeField] private ArmorType armorType = ArmorType.Head;
        #endregion fields & properties

        #region methods
        #endregion methods
    }
}