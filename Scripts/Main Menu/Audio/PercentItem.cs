using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class PercentItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        private int value;
        public GameObject rootObject => gameObject;
        public int listParam => value;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            value = param;
            txt.text = $"{value}%";
            txt.lineSpacing = value;
        }
        #endregion methods
    }
}