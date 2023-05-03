using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class DamageMiniGame : MiniGame
    {
        #region fields & properties
        [SerializeField] private DamageZone zone;
        #endregion fields & properties

        #region methods
        public override void CheckGameResult()
        {
            if (zone.IsCollided)
                CompleteGame();
            else
            {
                RestartGame();
                AudioManager.PlayClip(AudioStorage.Instance.ErrorSound, Universal.AudioType.Sound);
            }
        }
        public override void CompleteGame()
        {
            base.CompleteGame();
            OnGameRestart?.Invoke();
        }
        #endregion methods
    }
}