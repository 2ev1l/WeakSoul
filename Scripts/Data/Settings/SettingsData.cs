using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class SettingsData
    {
        #region fields & properties
        [System.NonSerialized] public static readonly string SaveName = "settings";
        [System.NonSerialized] public static readonly string SaveExtension = ".json";
        [System.NonSerialized] private static SettingsData _data;
        public static SettingsData Data
        {
            get => _data;
            set => SetData(value);
        }

        public UnityAction<GraphicsSettings> OnGraphicsChanged;

        [SerializeField] private LanguageSettings _languageSettings = new LanguageSettings();
        [SerializeField] private GraphicsSettings _graphicsSettings = new GraphicsSettings();
        [SerializeField] private AudioSettings _audioSettings = new AudioSettings();

        public LanguageSettings LanguageSettings
        {
            get => _languageSettings;
        }
        public GraphicsSettings GraphicsSettings
        {
            get => _graphicsSettings;
            set => SetGraphics(value);
        }
        public AudioSettings AudioSettings
        {
            get => _audioSettings;
        }
        #endregion fields & properties

        #region methods
        private void SetGraphics(GraphicsSettings value)
        {
            _graphicsSettings = value;
            OnGraphicsChanged?.Invoke(value);
        }
        private static void SetData(SettingsData value) => _data = value ?? new SettingsData();
        #endregion methods
    }
}