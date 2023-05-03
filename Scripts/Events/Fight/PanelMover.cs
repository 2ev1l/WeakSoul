using Data;
using Data.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class PanelMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region fields & properties
        private static List<PanelData> FixedPanels;
        [SerializeField] private int panelId;
        [SerializeField] private GameObject panel;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            LoadPostion();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!CheckButtonEnable(eventData)) return;
            SavePosition();
            MovePosition();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!CheckButtonEnable(eventData)) return;
            CancelInvoke(nameof(MovePosition));
            SavePosition();
        }
        private void LoadPostion()
        {
            FixedPanels ??= new();
            int index = FixedPanels.FindIndex(x => x.PanelId == panelId);
            if (index == -1) return;
            panel.transform.position = FixedPanels[index].Position;
        }
        private void SavePosition()
        {
            FixedPanels ??= new();
            int index = FixedPanels.FindIndex(x => x.PanelId == panelId);
            PanelData newPos = new (panel.transform.position, panelId);
            if (index == -1)
            {
                FixedPanels.Add(newPos);
                return;
            }
            else FixedPanels.RemoveAt(index);
            FixedPanels.Add(newPos);
        }
        private void MovePosition()
        {
            Vector3 newPos = panel.transform.position;
            newPos = Vector3.Lerp(newPos, GetNextMovePosition(newPos), Time.deltaTime);
            panel.transform.position = newPos;
            Invoke(nameof(MovePosition), Time.deltaTime);
        }
        private Vector3 GetNextMovePosition(Vector3 oldPos) => oldPos +
            (Camera.main.orthographicSize / 2.16f) *
            (1440f / SettingsData.Data.GraphicsSettings.Resolution.width) *
            ((SettingsData.Data.GraphicsSettings.Resolution.width / (float)SettingsData.Data.GraphicsSettings.Resolution.height) / 1.78f) *
            ((SettingsData.Data.GraphicsSettings.Vsync ? 144f : SettingsData.Data.GraphicsSettings.RefreshRate) / 144f) *
            CursorSettings.MouseDirection;
        private bool CheckButtonEnable(PointerEventData eventData) => eventData.button == PointerEventData.InputButton.Left;
        #endregion methods
    }
}