using Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class AudioManager : MonoBehaviour
    {
        #region fields & properties
        public static AudioManager Instance { get; private set; }
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambienceSource;
        private readonly static List<string> menuMusicScenes = new() { "Main Menu", "Game Menu" };
        private static float musicScale = 1f;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            SettingsData.Data.AudioSettings.MusicData.OnVolumeChanged += UpdateCurrentMusicVolume;
            SettingsData.Data.AudioSettings.AudioData.OnVolumeChanged += UpdateCurrentMusicVolume;
            SettingsData.Data.AudioSettings.MusicData.OnVolumeChanged += UpdateAmbientVolume;
            SettingsData.Data.AudioSettings.AudioData.OnVolumeChanged += UpdateAmbientVolume;
        }
        private void OnDisable()
        {
            SettingsData.Data.AudioSettings.MusicData.OnVolumeChanged -= UpdateCurrentMusicVolume;
            SettingsData.Data.AudioSettings.AudioData.OnVolumeChanged -= UpdateCurrentMusicVolume;
            SettingsData.Data.AudioSettings.MusicData.OnVolumeChanged -= UpdateAmbientVolume;
            SettingsData.Data.AudioSettings.AudioData.OnVolumeChanged -= UpdateAmbientVolume;
        }
        public void Start()
        {
            PlayMusicDependOnScene();
        }
        private void PlayMusicDependOnScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.Equals("Cut Scene")) return;

            if (menuMusicScenes.Contains(sceneName))
                PlayMusic(AudioStorage.Instance.MenuMusic);
            else
                PlayMusic(AudioStorage.Instance.GameMusic);
        }
        public static void PlayClip(AudioClip clip, AudioType type)
        {
            float volume = 1f * GetValueByType(type) * GetValueByType(AudioType.Audio);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }
        public static void AddAmbient(AudioClip clip, bool force = false, float defaultMusicScale = 0.8f)
        {
            if (force || Instance.ambienceSource.clip != clip)
            {
                Instance.ambienceSource.clip = clip;
            }
            musicScale = defaultMusicScale;
            Instance.UpdateCurrentMusicVolume();
            Instance.UpdateAmbientVolume();
            Instance.ambienceSource.Play();
        }
        public static void RemoveAmbient()
        {
            musicScale = 1f;
            Instance.ambienceSource.clip = null;
            Instance.ambienceSource.Stop();
            Instance.UpdateCurrentMusicVolume();
        }
        public static void PlayMusic(AudioClip clip, [Optional] bool force)
        {
            if (force || Instance.musicSource.clip != clip)
            {
                Instance.musicSource.clip = clip;
                Instance.UpdateCurrentMusicVolume();
                Instance.musicSource.Play();
            }
        }
        private void UpdateCurrentMusicVolume(float musicVolume) => UpdateCurrentMusicVolume();
        private void UpdateAmbientVolume(float musicVolume) => UpdateAmbientVolume();
        public void UpdateCurrentMusicVolume() => UpdateVolume(musicSource, AudioType.Music);
        public void UpdateAmbientVolume() => ambienceSource.volume = 0.5f * GetValueByType(AudioType.Music) * GetValueByType(AudioType.Audio);

        public static void UpdateVolume(AudioSource audioSource, AudioType type) => audioSource.volume = musicScale * GetValueByType(type) * GetValueByType(AudioType.Audio);
        private static float GetValueByType(AudioType type) => (type) switch
        {
            AudioType.Music => SettingsData.Data.AudioSettings.MusicData.Volume,
            AudioType.Sound => SettingsData.Data.AudioSettings.SoundData.Volume,
            AudioType.Audio => SettingsData.Data.AudioSettings.AudioData.Volume,
            _ => throw new System.NotImplementedException()
        };
        #endregion methods
    }
}