using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class LanguageItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        private int id;
        public GameObject rootObject => gameObject;
        public int listParam => id;
        public static List<string> LanguageNames
        {
            get
            {
                languageNames ??= SavingUtils.GetLanguageNames();
                return languageNames;
            }
        }
        private static List<string> languageNames;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            id = param;
            txt.text = LanguageNames[id];
        }
        #endregion methods
    }
}