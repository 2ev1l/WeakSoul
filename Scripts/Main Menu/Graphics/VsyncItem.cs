using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class VsyncItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        private int id;
        public GameObject rootObject => gameObject;
        public int listParam => id;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            id = param;
            txt.text = id == 0 ? "X" : "Vsync";
        }
        #endregion methods
    }
}