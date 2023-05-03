using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.MainMenu
{
    public class SettingsPanelStateMachine : StateMachine
    {
        #region methods
        private void OnEnable()
        {
            SettingsPanelInit.Instance.GetComponent<OverlayState>().OnStateChanged += delegate { ApplyDefaultState(); };
        }
        private void OnDisable()
        {
            SettingsPanelInit.Instance.GetComponent<OverlayState>().OnStateChanged -= delegate { ApplyDefaultState(); };
        }

        #endregion methods
    }
}