using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Universal
{
    public abstract class ShowHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields
        public bool PanelState => helpUpdater.State;
        protected abstract HelpUpdater helpUpdater { get; }
        #endregion fields

        #region methods
        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenPanel();
        }
        public virtual void OpenPanel()
        {
            helpUpdater.OpenPanel(Vector3.zero);
        }
        public void OnPointerExit(PointerEventData eventData) => HidePanel();
        protected virtual void OnDisable()
        {
            HidePanel();
        }
        public void HidePanel()
        {
            helpUpdater.HidePanel();
        }
        #endregion methods
    }
}