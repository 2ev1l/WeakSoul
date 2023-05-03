using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.Adventure
{
    public class DeadScreen : MonoBehaviour
    {
        #region fields & properties
        public static UnityAction<bool> OnDeadScreenChanged;
        [SerializeField] private CanvasGroup canvasGroup;
        public GameObject deadDefaultPanel;
        public GameObject deadCompletelyPanel;
        public static bool IsDeadScreenApplied
        {
            get => isDeadScreenApplied;
            set
            {
                isDeadScreenApplied = value;
                OnDeadScreenChanged?.Invoke(value);
            }
        }
        private static bool isDeadScreenApplied;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.OnDeadCompletely += DoDeathComplete;
            GameData.Data.PlayerData.Stats.OnDead += DoDeathDefault;
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.OnDeadCompletely -= DoDeathComplete;
            GameData.Data.PlayerData.Stats.OnDead -= DoDeathDefault;
        }
        public void Start()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!IsDeadScreenApplied) return;

			if (sceneName == "Main Menu")
            {
                CloseAll();
                SavingUtils.ResetTotalProgress(true);
            }
            if (sceneName == "Game Menu" || sceneName == "Cut Scene")
            {
                CloseAll();
            }
        }
        private void DoDeathDefault()
        {
            if (deadCompletelyPanel.activeSelf) return;
            IsDeadScreenApplied = true;
            ResetCanvasAlpha();
            deadDefaultPanel.SetActive(true);
            StartCoroutine(CanvasAlphaChange());
        }
        private void DoDeathComplete()
        {
            R();
            IsDeadScreenApplied = true;
            ResetCanvasAlpha();
            deadCompletelyPanel.SetActive(true);
            StartCoroutine(CanvasAlphaChange());
        }
        private IEnumerator CanvasAlphaChange()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 1, 2);
            while (!vsc.IsChangeEnded)
            {
                canvasGroup.alpha = vsc.Out;
                yield return CustomMath.WaitAFrame();
            }
            canvasGroup.alpha = 1;
            yield return CustomMath.WaitAFrame();
            Destroy(vsc);
        }
        private void ResetCanvasAlpha() => canvasGroup.alpha = 0;
        public void Rebirth()
        {
            if (GameData.Data.AdventureData.TryLoadBossCutScene()) return;
            SceneLoader.Instance.LoadSceneFade("Game Menu", 1f);
        }
        public void ResetAll()
        {
            SceneLoader.Instance.LoadSceneFade("Main Menu", 1f);
        }
        private void CloseAll()
        {
            deadCompletelyPanel.SetActive(false);
            deadDefaultPanel.SetActive(false);
            ResetCanvasAlpha();
            IsDeadScreenApplied = false;
        }
        private void R() => SavingUtils.ResetTotalProgress(false);
        #endregion methods
    }
}