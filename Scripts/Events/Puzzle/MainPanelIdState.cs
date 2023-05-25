using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.MainMenu;

namespace WeakSoul.Events.Puzzle
{
    public class MainPanelIdState : StateChange
    {
        #region fields & properties
        public int Id => id;
        [SerializeField] private int id = 0;
        [SerializeField] private GameObject panel;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            panel.SetActive(active);
        }
        #endregion methods
    }
}