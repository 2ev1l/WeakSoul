using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class EvasionMiniGame : MiniGame
    {
        #region fields & properties
        [SerializeField] private EvasionMove move;
        #endregion fields & properties

        #region methods
        public override void CheckGameResult()
        {
            if (move.IsMoving)
            {
                RestartGame();
                return;
            }
            move.StartMoving();
        }
        #endregion methods
    }
}