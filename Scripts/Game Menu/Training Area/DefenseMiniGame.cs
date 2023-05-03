using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class DefenseMiniGame : MiniGame
    {
        #region fields & properties
        [SerializeField] private DefenseZone leftZone;
        [SerializeField] private DefenseMove leftMove;
        [SerializeField] private DefenseZone rightZone;
        [SerializeField] private DefenseMove rightMove;
        private bool IsLeftChecked;
        private bool IsRightChecked;
        #endregion fields & properties

        #region methods
        public override void RestartGame()
        {
            base.RestartGame();
            IsLeftChecked = false;
            IsRightChecked = false;
        }
        public override void CheckGameResult()
        {
            bool l = leftZone.IsCollided;
            bool r = rightZone.IsCollided;

            if (!IsLeftChecked && l)
            {
                ApplyLeftSide();
                return;
            }
            if (!IsRightChecked && r)
            {
                ApplyRightSide();
                return;
            }
            AudioManager.PlayClip(AudioStorage.Instance.ErrorSound, Universal.AudioType.Sound);
            RestartGame();
        }
        private void ApplyLeftSide()
        {
            IsLeftChecked = true;
            leftMove.Stop();
            CheckGameComplete();
        }
        private void ApplyRightSide()
        {
            IsRightChecked = true;
            rightMove.Stop();
            CheckGameComplete();
        }
        private void CheckGameComplete()
        {
            if (IsLeftChecked && IsRightChecked)
            {
                if (leftZone.IsCorrect && rightZone.IsCorrect)
                    CompleteGame();
                else
                    RestartGame();
            }
        }
        #endregion methods
    }
}