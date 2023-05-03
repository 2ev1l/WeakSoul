using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Universal;
using WeakSoul.Adventure;

namespace WeakSoul.GameMenu.Skills
{
    [RequireComponent(typeof(Image))]
    public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region fields & properties
        [SerializeField] private Transform panelScaler;

        [SerializeField] protected float moveSensitivity = -0.765f;
        [SerializeField] protected float scrollSensitivity = 100f;
        [SerializeField] protected Vector2 scaleRange = new Vector2(0.5f, 1f);
        [SerializeField] private Vector4 moveRange = new Vector4(-3.8f, -5.48f, 3.8f, 5.48f);
        [SerializeField] private Vector2 decreaseScale = new(2f, 2f);

        private bool canMove = false;
        protected float PanelScale => panelScaler.localScale.x;
        #endregion fields & properties

        #region methods
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            AllowMove();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            DisableMove();
        }

        private void OnDisable()
        {
            DisableMove();
            ReturnToZero();
        }
        private void AllowMove() => canMove = true;
        private void DisableMove() => canMove = false;
        protected virtual void ReturnToZero()
        {
            try { Camera.main.transform.position = Vector3.zero; }
            catch { };
            panelScaler.localScale = Vector3.one;
        }

        private void Update()
        {
            ScrollCamera();
            if (!canMove) return;
            MoveCamera();
        }
        protected virtual void ScrollCamera()
        {
            if (DeadScreen.IsDeadScreenApplied) return;
            Vector3 scale = panelScaler.localScale;
            Vector3 startScale = scale;
            scale = Vector3.Lerp(scale, (1 + CursorSettings.MouseWheelDirection) * scale, Time.deltaTime * scrollSensitivity);
            scale = Mathf.Clamp(scale.x, scaleRange.x, scaleRange.y) * Vector3.one;
            panelScaler.localScale = scale;
            OnCameraScroll();
            if (scale.x != startScale.x)
                MoveCamera();
        }
        protected virtual void OnCameraScroll() { }
        private void MoveCamera()
        {
            Vector3 newPos = Camera.main.transform.position;
            float panelScale = panelScaler.localScale.x;
            newPos = Vector3.Lerp(newPos, GetNextMovePosition(newPos), Time.deltaTime);
            float decrease = (1 - panelScale) * moveRange.z * 0.75f;
            Vector2 decreaseVec = decreaseScale * decrease;
            newPos.z = 0;
            newPos.x = Mathf.Clamp(newPos.x, (moveRange.x + decreaseVec.x) * 1.1f, (moveRange.z - decreaseVec.x) * 1.1f);
            newPos.y = Mathf.Clamp(newPos.y, (moveRange.y + decreaseVec.y), (moveRange.w - decreaseVec.y));
            Camera.main.transform.position = newPos;
        }
        protected virtual Vector3 GetNextMovePosition(Vector3 oldPos) => oldPos +
            (1440f / SettingsData.Data.GraphicsSettings.Resolution.width) *
            ((SettingsData.Data.GraphicsSettings.Resolution.width / (float)SettingsData.Data.GraphicsSettings.Resolution.height) / 1.78f) *
            ((SettingsData.Data.GraphicsSettings.Vsync ? 144f : SettingsData.Data.GraphicsSettings.RefreshRate) / 144f) *
            moveSensitivity * CursorSettings.MouseDirection;
        protected virtual void OnCameraMove() { }
        #endregion methods
    }
}