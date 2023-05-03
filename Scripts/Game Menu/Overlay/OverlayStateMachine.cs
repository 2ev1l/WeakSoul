using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;
using WeakSoul.Adventure;

namespace WeakSoul.GameMenu
{
    public class OverlayStateMachine : StateMachine
    {
        #region fields & properties
        private bool isDeadScreenApplied = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            DeadScreen.OnDeadScreenChanged += CheckDeadScreen;
            InputController.OnKeyDown += ActivateStateByKey;
        }
        private void OnDisable()
        {
            DeadScreen.OnDeadScreenChanged -= CheckDeadScreen;
            InputController.OnKeyDown -= ActivateStateByKey;
        }
        private void CheckDeadScreen(bool state)
        {
            isDeadScreenApplied = state;
            ApplyDefaultState();
        }
        public void ActivateStateByKey(KeyCode keyCode)
        {
            if (SceneLoader.IsBlackScreenFade() || isDeadScreenApplied) return;

            foreach (OverlayState state in states.Cast<OverlayState>())
            {
                if (keyCode != state.ActivateKey) continue;
                if (!state.IsSceneAllowed())
                {
                    ApplyDefaultState();
                    continue;
                }
                ApplyState(state);
                return;
            }
        }
        #endregion methods
    }
}