using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Adventure
{
    public class ZoneWarning : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            Player.OnSubZoneChanged += CheckSubZone;
            panel.SetActive(false);
        }
        private void OnDisable()
        {
            Player.OnSubZoneChanged -= CheckSubZone;
        }
        private void CheckSubZone(SubZoneData zone)
        {
            panel.SetActive(zone.LevelsScale.x > GameData.Data.PlayerData.Stats.ExperienceLevel.Level);
        }
        #endregion methods
    }
}