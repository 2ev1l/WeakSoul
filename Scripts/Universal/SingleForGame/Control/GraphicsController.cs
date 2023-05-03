using Data;
using UnityEngine;

namespace Universal
{
    public class GraphicsController : MonoBehaviour
    {
        #region methods
        private void Start()
        {
            OnGraphicsChanged();
        }
        private void OnEnable()
        {
            SettingsData.Data.OnGraphicsChanged += delegate { OnGraphicsChanged(); };
            SettingsData.Data.GraphicsSettings.OnSettingsChanged += delegate { OnGraphicsChanged(); };
        }
        private void OnDisable()
        {
            SettingsData.Data.OnGraphicsChanged -= delegate { OnGraphicsChanged(); };
            SettingsData.Data.GraphicsSettings.OnSettingsChanged -= delegate { OnGraphicsChanged(); };
        }
        private void OnGraphicsChanged()
        {
            GraphicsSettings graphicsSettings = SettingsData.Data.GraphicsSettings;
            Screen.SetResolution(graphicsSettings.Resolution.width, graphicsSettings.Resolution.height, graphicsSettings.ScreenMode);
            SetGraphicsExceptResolution();
        }
        private void SetGraphicsExceptResolution()
        {
            GraphicsSettings graphicsSettings = SettingsData.Data.GraphicsSettings;
            Application.targetFrameRate = graphicsSettings.RefreshRate;
            QualitySettings.vSyncCount = graphicsSettings.Vsync ? 1 : 0;
        }
        #endregion methods
    }
}