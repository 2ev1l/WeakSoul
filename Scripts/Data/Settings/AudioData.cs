using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class AudioData
    {
        #region fields & properties
        public UnityAction<float> OnVolumeChanged;

        public float Volume
        {
            get => _volume;
            set => SetVolume(value);
        }
        [SerializeField] private float _volume = 0.5f;
        #endregion fields & properties

        #region methods
        private void SetVolume(float value)
        {
            if (value < 0 || value > 1)
                throw new System.ArgumentOutOfRangeException("volume");
            _volume = value;
            OnVolumeChanged?.Invoke(value);
        }
        #endregion methods
    }
}