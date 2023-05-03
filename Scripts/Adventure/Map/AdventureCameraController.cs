using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Universal;
using WeakSoul.GameMenu.Skills;

namespace WeakSoul.Adventure.Map
{
    public class AdventureCameraController : CameraController
    {
        #region fields & properties
        private static float cameraDefaultSize = 2.16f;
        public static AdventureCameraController Instance { get; private set; }
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            Instance = this;
        }
        protected override void ReturnToZero()
        {
            try
            {
                Camera.main.transform.position = Vector3.zero;
                Camera.main.orthographicSize = cameraDefaultSize;
            }
            catch { };
        }
        protected override void ScrollCamera()
        {
            if (DeadScreen.IsDeadScreenApplied) return;
            Vector3 scale = Camera.main.orthographicSize * Vector3.one;
            scale = Vector3.Lerp(scale, (1 - CursorSettings.MouseWheelDirection) * scale, Time.deltaTime * scrollSensitivity);
            scale = Mathf.Clamp(scale.x, scaleRange.x + cameraDefaultSize, scaleRange.y + cameraDefaultSize) * Vector3.one;
            Camera.main.orthographicSize = scale.x;
        }
        protected override Vector3 GetNextMovePosition(Vector3 oldPos) => oldPos +
            (Camera.main.orthographicSize / cameraDefaultSize) *
            (1440f / SettingsData.Data.GraphicsSettings.Resolution.width) *
            ((SettingsData.Data.GraphicsSettings.Resolution.width / (float)SettingsData.Data.GraphicsSettings.Resolution.height) / 1.78f) *
            ((SettingsData.Data.GraphicsSettings.Vsync ? 144f : SettingsData.Data.GraphicsSettings.RefreshRate) / 144f) *
            moveSensitivity * CursorSettings.MouseDirection;
        #endregion methods
    }
}