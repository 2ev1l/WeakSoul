using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.MainMenu
{
    public class GraphicsTempSettings : MonoBehaviour
    {
        #region fields & properties
        public static GraphicsSettings Settings { get; private set; } = new GraphicsSettings();
        #endregion fields & properties

        #region methods
        public void ApplySettings()
        {
            SettingsData.Data.GraphicsSettings = Settings;
        }
        #endregion methods
    }
}