using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class CriticalChanceMiniGame : MiniGame
    {
        #region fields & properties
        [SerializeField] private CriticalChanceMove zoneL;
        [SerializeField] private CriticalChanceMove zoneR;
        private bool isLChecked;
        #endregion fields & properties

        #region methods
        public override void CheckGameResult()
        {
            bool l = zoneL.IsZoneChecked;
            bool r = zoneR.IsZoneChecked;
            if (!l && !r)
            {
                RestartWithSound();
                return;
            }
            if (!isLChecked)
            {
                if (!l)
                {
                    RestartWithSound();
                    return;
                }
                isLChecked = true;
                zoneL.StopCheckZone();
                zoneR.StartCheckZone();
                return;
            }
            if (r)
            {
                if (!isLChecked) return;
                zoneR.StopCheckZone();
                CompleteGame();
            }
            if (!r && isLChecked)
                RestartWithSound();
        }
        private void RestartWithSound()
        {
            AudioManager.PlayClip(AudioStorage.Instance.ErrorSound, Universal.AudioType.Sound);
            RestartGame();
        }
        public override void RestartGame()
        {
            base.RestartGame();
            isLChecked = false;
            zoneL.StartCheckZone();
        }
        #endregion methods
    }
}