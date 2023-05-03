using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class AudioSettings
    {
        #region fields & properties
        [SerializeField] private AudioData _soundData = new AudioData();
        [SerializeField] private AudioData _musicData = new AudioData();
        [SerializeField] private AudioData _audioData = new AudioData();
        public AudioData SoundData => _soundData;
        public AudioData MusicData => _musicData;
        public AudioData AudioData => _audioData;
        #endregion fields & properties
    }
}