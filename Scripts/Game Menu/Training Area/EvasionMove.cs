using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class EvasionMove : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private MiniGame miniGame;
        [SerializeField] private Animator swordAnimator;
        [SerializeField] private Vector3 startLocalPosition;
        public bool IsMoving { get; private set; }
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            miniGame.OnGameRestart += Restart;
            Restart();
        }
        private void OnDisable()
        {
            miniGame.OnGameRestart -= Restart;
        }
        private void Restart()
        {
            StopMoving();
            swordAnimator.enabled = true;
            swordAnimator.speed = 1f / miniGame.timeDeviation;
            swordAnimator.Play("Training-Evasion-Sword", -1, CustomMath.GetRandomChance(50) ? 0 : 0.5f);
        }
        public void StartMoving()
        {
            swordAnimator.enabled = false;
            IsMoving = true;
            MoveToEnd();
        }
        private void MoveToEnd()
        {
            Vector3 direction = -3 * Time.deltaTime * swordAnimator.transform.up;
            Vector3 localPos = transform.position;
            localPos += 6 * direction / miniGame.timeDeviation;
            transform.position = localPos;
            Invoke(nameof(MoveToEnd), Time.deltaTime);
        }
        private void StopMoving()
        {
            CancelInvoke(nameof(MoveToEnd));
            Vector3 localPos = transform.localPosition;
            localPos = startLocalPosition;
            transform.localPosition = localPos;
            IsMoving = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Respawn"))
            {
                miniGame.RestartGame();
                AudioManager.PlayClip(AudioStorage.Instance.ErrorSound, Universal.AudioType.Sound);
                return;
            }
            if (collision.gameObject.CompareTag("Finish"))
            {
                miniGame.CompleteGame();
                miniGame.RestartGame();
            }

        }
        #endregion methods
    }
}