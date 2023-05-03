using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.MainMenu
{
    [RequireComponent(typeof(OverlayState))]
    public class SettingsPanelInit : CanvasInit
    {
        #region fields & properties
        public static SettingsPanelInit Instance { get; private set; }

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject bgLogo;
        [field: SerializeField] public CustomButton MenuBackButton { get; private set; }
        [field: SerializeField] public CustomButton BackButton { get; private set; }
        private static readonly List<string> allowedMenuButtonScenes = new() { "Game Menu", "Cut Scene" };
        #endregion fields & properties

        #region methods

        public override void Init()
        {
            Instance = this;
        }

        public override void Start()
        {
            base.Start();
            string sceneName = SceneManager.GetActiveScene().name;
            MenuBackButton.gameObject.SetActive(allowedMenuButtonScenes.Contains(sceneName));
            bgLogo.SetActive(sceneName != "Main Menu");
        }
        public void LoadMenu()
        {
            SceneLoader.Instance.LoadSceneFade("Main Menu", 1f);
        }
        #endregion methods
    }
}