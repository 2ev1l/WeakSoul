using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class Weapon: StatsItem
    {
        #region fields & properties
        [SerializeField] private List<PlayerClass> allowedClasses;
        #endregion fields & properties

        #region methods
        public bool IsPlayerClassAllowed() => allowedClasses.Contains(GameData.Data.PlayerData.Stats.Class);
        #endregion methods
    }
}