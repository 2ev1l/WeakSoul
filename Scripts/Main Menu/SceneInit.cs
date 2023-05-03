using Data;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.MainMenu
{
    public class SceneInit : SingleSceneInstance
    {
        #region fields & properties
        public static SceneInit Instance { get; private set; }
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private MainPanelStateMachine mainStateMachine;
        [SerializeField] private GameObject continueButton;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            SettingsPanelInit.Instance.BackButton.OnClicked += ActivateMainPanel;
            SettingsPanelInit.Instance.GetComponent<OverlayState>().OnStateChanged += OnSettingsPanelChanged;
        }
        private void OnDisable()
        {
            SettingsPanelInit.Instance.BackButton.OnClicked -= ActivateMainPanel;
            if (SettingsPanelInit.Instance != null)
                SettingsPanelInit.Instance.GetComponent<OverlayState>().OnStateChanged -= OnSettingsPanelChanged;
        }
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            continueButton.SetActive(GameData.Data.TutorialData.IsCompleted);
        }
        private void OnSettingsPanelChanged(bool isActive)
        {
            if (!isActive)
                ActivateMainPanel();
            else
                ActivateSettingsPanel();
        }
        private void ActivateMainPanel() => mainStateMachine.ApplyDefaultState();
        private void ActivateSettingsPanel() => mainStateMachine.TryApplyState(2);

        public void StartNewGame()
        {
            SavingUtils.ResetTotalProgress();
            SceneLoader.Instance.LoadCutSceneFade(1f, 0);
        }
        public void ContinueGame()
        {
            SceneLoader.Instance.LoadSceneFade(GameData.Data.SceneName, 1f);
        }
        #endregion methods
    }
}