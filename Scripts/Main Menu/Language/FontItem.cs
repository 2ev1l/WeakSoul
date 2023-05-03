using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class FontItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        [SerializeField] private TextOutline outline;
        private int id;
        public GameObject rootObject => gameObject;
        public int listParam => id;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            outline.UpdateFont = false;
        }
        public void OnListUpdate(int param)
        {
            id = param;
            txt.text = TextData.Instance.Fonts[id].name;
            txt.font = TextData.Instance.Fonts[id];
        }
        #endregion methods
    }
}