using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Universal
{
    public class CustomButton : CursorChanger, IPointerClickHandler
    {
        #region fields & properties
        public UnityEvent OnClickEvent => onClickEvent;
        [SerializeField] private UnityEvent onClickEvent;
        public UnityAction OnClicked;
        #endregion fields & properties

        #region methods
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!IsButtonLeft(eventData)) return;
            base.OnPointerClick(eventData);
            onClickEvent?.Invoke();
            OnClicked?.Invoke();
        }
        public void Quit()
        {
            if (SteamManager.Initialized)
                Steamworks.SteamAPI.Shutdown();
            Application.Quit();
        }
        #endregion methods
    }
}