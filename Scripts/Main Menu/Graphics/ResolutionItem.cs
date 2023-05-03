using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class ResolutionItem : MonoBehaviour, IListUpdater
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
            Resolution resolution = ResolutionItemList.Resolutions[id];
            txt.text = $"{resolution.width}x{resolution.height}";
        }
        #endregion methods
    }
}