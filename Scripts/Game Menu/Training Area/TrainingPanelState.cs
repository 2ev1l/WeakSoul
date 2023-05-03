using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class TrainingPanelState : ShopPanelState
    {
        #region fields & properties
        [SerializeField] private StateMachine childStateMachine;
        [SerializeField] private StateChange childDefaultState;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;
            childStateMachine.TryApplyState(childDefaultState);
        }
        #endregion methods
    }
}