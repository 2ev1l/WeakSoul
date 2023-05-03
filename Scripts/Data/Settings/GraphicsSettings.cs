using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WeakSoul.MainMenu;

namespace Data
{
    [System.Serializable]
    public class GraphicsSettings
    {
        #region fields & properties
        public UnityAction OnSettingsChanged;
        public SimpleResolution Resolution
        {
            get
            {
                if (resolution.width == 0 || resolution.height == 0)
                {
                    resolution.width = Screen.currentResolution.width;
                    resolution.height = Screen.currentResolution.height;
                }
                    
                return resolution;
            }
            set => SetResolution(value);
        }
        [SerializeField] private SimpleResolution resolution;
        public FullScreenMode ScreenMode
        {
            get => screenMode;
            set => SetScreenMode(value);
        }
        [SerializeField] private FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
        public bool Vsync
        {
            get => vsync;
            set => SetVsync(value);
        }
        [SerializeField] private bool vsync = false;
        public int RefreshRate
        {
            get => refreshRate;
            set => SetRefreshRate(value);
        }
        [SerializeField] private int refreshRate = 60;
        #endregion fields & properties

        #region methods
        private void SetResolution(SimpleResolution value)
        {
            resolution = value;
            OnSettingsChanged?.Invoke();
        }
        private void SetScreenMode(FullScreenMode value)
        {
            screenMode = value;
            OnSettingsChanged?.Invoke();
        }
        private void SetVsync(bool value)
        {
            vsync = value;
            OnSettingsChanged?.Invoke();
        }
        private void SetRefreshRate(int value)
        {
            if (value < 1)
                throw new System.ArgumentOutOfRangeException("refresh rate");
            refreshRate = value;
            OnSettingsChanged?.Invoke();
        }
        #endregion methods
    }
}