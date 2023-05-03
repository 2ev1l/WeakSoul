using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class DamageZone : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private DamageMiniGame miniGame;
        [SerializeField] private Transform center;
        [SerializeField] private Animator swordAnimator;
        [SerializeField] private Vector4 allowedSpawnRect;
        public bool IsCollided { get; private set; }
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
            ResetValues();
        }
        private void Restart()
        {
            ResetValues();
            RandomPosition();
        }
        private void ResetValues()
        {
            IsCollided = false;
            swordAnimator.speed = 1f / miniGame.timeDeviation;
            swordAnimator.Play("Training-Damage-Sword", -1, CustomMath.GetRandomChance(50) ? 0 : 0.5f);
        }
        private void RandomPosition()
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = Random.Range(allowedSpawnRect.x, allowedSpawnRect.z);
            localPos.y = Random.Range(allowedSpawnRect.y, allowedSpawnRect.w);
            transform.localPosition = localPos;
            CustomAnimation.LookAt2D(transform, transform.position, center.position);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IsCollided = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            IsCollided = false;
        }
        #endregion methods
    }
}