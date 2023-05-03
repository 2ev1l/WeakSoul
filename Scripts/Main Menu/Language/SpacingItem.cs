using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class SpacingItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        [SerializeField] private Text txt;
        private float value;
        public GameObject rootObject => gameObject;
        public int listParam => Mathf.RoundToInt(value * 10f);
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            value = param / 10f;
            txt.text = value.ToString("F2");
            txt.lineSpacing = value;
        }
        #endregion methods
    }
}