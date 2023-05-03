using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.Events.Fight
{
    public class TutorialInit : TutorialPanel
    {
        #region fields & properties
        [SerializeField] private GameObject playerSkillsPanel;
        [SerializeField] private GameObject nextTurnButton;
        [SerializeField] private FightCard playerCard;
        [SerializeField] private FightCard enemyCard;
        [SerializeField] private Transform spawnCanvas;
        [SerializeField] private VideoPlayer videoPlayerPrefab;
        [SerializeField] private VideoClip defenseClip;
        [SerializeField] private VideoClip deathClip;
        [SerializeField] private VideoClip escapeClip;
        [SerializeField][ReadOnly] private VideoPlayer spawnedPlayer;
        protected override int Progress => TutorialData.FightProgress;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            if (GameData.Data.TutorialData.IsFightCompleted) return;
            InputController.OnKeyDown += CheckKeyDown;
        }
        private void OnDisable()
        {
            InputController.OnKeyDown -= CheckKeyDown;
        }
        protected new IEnumerator Start()
        {
            CheckTutorial();
            yield return CustomMath.WaitAFrame();
            if (GameData.Data.TutorialData.IsFightCompleted) yield break;
            nextTurnButton.SetActive(false);
            playerSkillsPanel.SetActive(false);
        }
        private void CheckKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.Escape && spawnedPlayer != null)
                Destroy(spawnedPlayer.gameObject);
        }
        protected override void CheckTutorial()
        {
            if (GameData.Data.TutorialData.IsFightCompleted) return;
            TutorialData.FightProgress = 0;
            CheckTutorialStep(0);
        }
        public override void CheckTutorialStep()
        {
            switch (Progress)
            {
                case 0:
                    ShowPanel(15);
                    IncreaseProgress();
                    break;
                case 1:
                    spawnedPlayer = InitializePlayer(videoPlayerPrefab, spawnCanvas, escapeClip);
                    ShowPanel(16);
                    IncreaseProgress();
                    break;
                case 2:
                    if (spawnedPlayer != null)
                        Destroy(spawnedPlayer.gameObject);
                    spawnedPlayer = InitializePlayer(videoPlayerPrefab, spawnCanvas, defenseClip);
                    ShowPanel(17);
                    IncreaseProgress();
                    break;
                case 3:
                    if (spawnedPlayer != null)
                        Destroy(spawnedPlayer.gameObject);
                    spawnedPlayer = InitializePlayer(videoPlayerPrefab, spawnCanvas, deathClip);

                    ShowPanel(19);
                    IncreaseProgress();
                    break;
                case 4:
                    if (spawnedPlayer != null)
                        Destroy(spawnedPlayer.gameObject);
                    ShowPanel(20);
                    IncreaseProgress();
                    break; //prepare for attack
                case 5:
                    ShowPanel(21);
                    IncreaseProgress();
                    enemyCard.Stats.GetUnscaledDamageSelf(enemyCard.Stats.Health / 2, DamageType.Physical);
                    playerCard.Stats.GetUnscaledDamageSelf(1, DamageType.Physical);
                    break; //hit player & enemy
                case 6:
                    GameData.Data.TutorialData.IsFightCompleted = true;
                    nextTurnButton.SetActive(true);
                    playerSkillsPanel.SetActive(true);
                    goto default;
                default: HidePanel(); break;
            }
        }
        private void IncreaseProgress() => TutorialData.FightProgress++;
        #endregion methods
    }
}