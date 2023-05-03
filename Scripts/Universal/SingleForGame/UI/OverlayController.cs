using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WeakSoul.GameMenu;

namespace Universal
{
    public class OverlayController : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private GameObject logo;
        [SerializeField] private GameObject overlay;
        [SerializeField] private NewLevelPanel levelPrefab;
        #endregion fields & properties

        #region methods
        public void Start()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Main Menu": logo.SetActive(true); break;
                default: logo.SetActive(false); break;
            }
            overlay.SetActive(true);
        }
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += InstantiateLevel;
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= InstantiateLevel;
        }
        private void InstantiateLevel(int level)
        {
            NewLevelPanel levelPanel = Instantiate(levelPrefab, Camera.main.transform.position, Quaternion.identity, transform) as NewLevelPanel;
            Vector3 position = levelPanel.transform.localPosition;
            position.z = 0;
            levelPanel.transform.localPosition = position;
            levelPanel.Init(level);
        }
        #endregion methods
    }
}