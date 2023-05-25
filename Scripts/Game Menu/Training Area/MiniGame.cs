using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public abstract class MiniGame : MonoBehaviour
    {
        #region fields & properties
        public static UnityAction<PhysicalStatsType> OnStatsGained;
        public UnityAction OnGameEnd;
        public UnityAction OnGameRestart;

        [SerializeField] private PanelInfo panelInfo;
        [SerializeField] private StateMachine stateMachine;
        [SerializeField] private StateChange defaultState;

        public float timeDeviation
        {
            get
            {
                if (panelInfo.CurrentLevelData != null)
                    return panelInfo.CurrentLevelData.TimeDeviation;
                return -1;
            }
        }
        private float timeSpent;
        private bool isGamePlaying = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            panelInfo.PlayerStatLevel.OnLevelChanged += GainStats;
        }
        private void OnDisable()
        {
            panelInfo.PlayerStatLevel.OnLevelChanged -= GainStats;
        }
        private void GainStats(int level) 
        {
            PhysicalStats stats = GameData.Data.PlayerData.Stats;
            StatsReward reward = LevelsInfo.Instance.TrainingLevels.GetPreviousLevelByType(panelInfo.Type).Reward;
            stats.IncreaseStatsHidden(reward.Stats);
            OnStatsGained?.Invoke(panelInfo.Type);
            ClosePanel();
        }
        private void ClosePanel()
        {
            RestartGame();
            stateMachine.TryApplyState(defaultState);
        }
        public virtual void CompleteGame()
        {
            EndGame();
            panelInfo.PlayerStatLevel.Experience++;
            AudioManager.PlayClip(AudioStorage.Instance.CorrectSound, Universal.AudioType.Sound);
        }
        public virtual void RestartGame()
        {
            isGamePlaying = true;
            timeSpent = timeDeviation;
            OnGameRestart?.Invoke();
        }
        public virtual void EndGame()
        {
            isGamePlaying = false;
            timeSpent = 0;
            OnGameEnd?.Invoke();
        }
        public abstract void CheckGameResult();
        private void Update()
        {
            if (isGamePlaying)
                CheckGameTime();
        }
        private void CheckGameTime()
        {
            timeSpent -= Time.deltaTime;

            if (timeSpent <= 0)
                EndGame();
        }
        #endregion methods
    }
}