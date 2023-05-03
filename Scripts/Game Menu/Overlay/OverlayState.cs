using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.GameMenu
{
    public class OverlayState : StateChange
    {
        #region fields & properties
        public UnityAction<bool> OnStateChanged;
        [field: SerializeField] public List<string> AllowedSceneNames { get; private set; }
        [field: SerializeField] public KeyCode ActivateKey { get; private set; }
        [SerializeField] private List<GameObject> mainPanels;
        [SerializeField] private GameObject overlayHelpButton;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            if (!active)
                mainPanels.ForEach(x => x.SetActive(false));
            else if (overlayHelpButton != null && overlayHelpButton.activeSelf)
                mainPanels.ForEach(x => x.SetActive(!x.activeSelf));
            OnStateChanged?.Invoke(mainPanels[0].activeSelf);
        }
        public bool IsSceneAllowed() => AllowedSceneNames.Contains(SceneManager.GetActiveScene().name) || AllowedSceneNames.Contains("Any");
        private void OnEnable()
        {
            SceneLoader.OnStartLoading += delegate { SetActive(false); };
            SceneLoader.OnSceneLoaded += CheckOverlayButton;
        }
        private void OnDisable()
        {
            SceneLoader.OnStartLoading -= delegate { SetActive(false); };
            SceneLoader.OnSceneLoaded -= CheckOverlayButton;
        }
        private void Start()
        {
            CheckOverlayButton();
        }
        private void CheckOverlayButton()
        {
            if (overlayHelpButton != null)
                overlayHelpButton.SetActive(IsSceneAllowed());
        }
        #endregion methods
    }
}