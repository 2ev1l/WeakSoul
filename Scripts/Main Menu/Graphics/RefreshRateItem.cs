using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class RefreshRateItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        private int rate;
        public GameObject rootObject => gameObject;
        public int listParam => rate;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            rate = param;
            txt.text = $"{rate} FPS";
        }
        #endregion methods
    }
}