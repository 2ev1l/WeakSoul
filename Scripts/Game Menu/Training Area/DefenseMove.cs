using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class DefenseMove : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> Y local component
        /// </summary>
        public UnityAction<float> OnPositionChanged;

        [SerializeField] private MiniGame miniGame;
        [SerializeField] private DefenseZone zone;
        [SerializeField] private float startPositionX;
        [SerializeField] private Vector2 startLocalPositionY;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            miniGame.OnGameEnd += StopMoving;
            miniGame.OnGameRestart += Restart;
            StopMoving();
        }
        private void OnDisable()
        {
            miniGame.OnGameEnd -= StopMoving;
            miniGame.OnGameRestart -= Restart;
            StopMoving();
        }
        private void Restart()
        {
            StopMoving();
            ResetPosition();
            MoveToEnd();
        }
        private void ResetPosition()
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = Random.Range(startLocalPositionY.x, startLocalPositionY.y);
            transform.localPosition = localPos;
            OnPositionChanged?.Invoke(localPos.y);
        }
        private void MoveToEnd()
        {
            float step = (-startPositionX) * Time.deltaTime;
            Vector3 localPos = transform.localPosition;
            localPos.x += step / miniGame.timeDeviation;
            transform.localPosition = localPos;
            Invoke(nameof(MoveToEnd), Time.deltaTime);
        }
        public void Stop()
        {
            CancelInvoke(nameof(MoveToEnd));
        }
        private void StopMoving()
        {
            CancelInvoke(nameof(MoveToEnd));
            Vector3 localPos = transform.localPosition;
            localPos.x = startPositionX;
            transform.localPosition = localPos;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                StopMoving();
            }
        }
        #endregion methods
    }
}