using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class HealthMiniGame : MiniGame
    {
        #region fields & properties
        public static HealthMiniGame Instance { get; private set; }
        [SerializeField] private HealthMove upperMove;
        [SerializeField] private HealthMove downMove;
        [SerializeField] private HealthZone upperZone;
        [SerializeField] private HealthZone downZone;
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            Instance = this;
        }
        public override void CheckGameResult()
        {
            bool d = CheckZone(downZone);
            bool u = CheckZone(upperZone);
            bool zoneChecked = d || u;

            if (IsZonesChecked())
            {
                CompleteGame();
                return;
            }

            if (!zoneChecked)
            {
                AudioManager.PlayClip(AudioStorage.Instance.ErrorSound, Universal.AudioType.Sound);
                RestartGame();
            }

        }
        private bool CheckZone(HealthZone zone)
        {
            bool a = zone.IsCollided;
            if (a) zone.DisableZone();
            return a;
        }
        private bool IsZonesChecked() => upperZone.IsChecked && downZone.IsChecked;
        #endregion methods
    }
}