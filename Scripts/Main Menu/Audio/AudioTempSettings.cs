using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.MainMenu
{
    public class AudioTempSettings : MonoBehaviour
    {
        #region fields & properties
        public static Data.AudioSettings Settings { get; } = new Data.AudioSettings();
        #endregion fields & properties

        #region methods
        public void ApplySettings()
        {
            SettingsData.Data.AudioSettings.SoundData.Volume = Settings.SoundData.Volume;
            SettingsData.Data.AudioSettings.MusicData.Volume = Settings.MusicData.Volume;
            SettingsData.Data.AudioSettings.AudioData.Volume = Settings.AudioData.Volume;
        }
        #endregion methods
    }
}