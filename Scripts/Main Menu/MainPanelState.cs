using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using System.Linq;

namespace WeakSoul.MainMenu
{
    public class MainPanelState : StateChange
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            if (panel == null)
                panel = SettingsPanelInit.Instance.transform.GetChild(0).gameObject;
            panel.SetActive(active);
        }
        #endregion methods
    }
}