using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Effects;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class DefenseZone : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private MiniGame miniGame;
        [SerializeField] DefenseMove defenseMove;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Material correctMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Vector2 startLocalPositionX;
        public bool IsCorrect { get; private set; } = false;
        public bool IsCollided { get; private set; } = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            miniGame.OnGameEnd += ResetValues;
            miniGame.OnGameRestart += Restart;
            defenseMove.OnPositionChanged += CorrectPositionWithParent;
            ResetValues();
        }
        private void OnDisable()
        {
            miniGame.OnGameEnd -= ResetValues;
            miniGame.OnGameRestart -= Restart;
            defenseMove.OnPositionChanged -= CorrectPositionWithParent;
            ResetValues();
        }
        private void Restart()
        {
            ResetValues();
            ResetPosition();
        }
        private void ResetValues()
        {
            spriteRenderer.material = defaultMaterial;
            IsCollided = false;
            IsCorrect = false;
            CorrectPositionWithParent(defenseMove.transform.localPosition.y);
        }
        public void DisableZone()
        {
            spriteRenderer.material = defaultMaterial;
            IsCollided = false;
            IsCorrect = true;
        }
        private void CorrectPositionWithParent(float y)
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = y;
            transform.localPosition = localPos;
        }
        private void ResetPosition()
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = Random.Range(startLocalPositionX.x, startLocalPositionX.y);
            transform.localPosition = localPos;
        }
        private void CheckPositionOffset(Transform tr)
        {
            float allowedDeviation = 40f / miniGame.timeDeviation;
            IsCorrect = Mathf.Abs(tr.localPosition.x - transform.localPosition.x) < allowedDeviation;
            spriteRenderer.material = IsCorrect ? correctMaterial : defaultMaterial;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DefenseMove defenseMove))
            {
                CheckPositionOffset(defenseMove.transform);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DefenseMove defenseMove))
            {
                IsCollided = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DefenseMove defenseMove))
            {
                IsCollided = false;
            }
        }
        #endregion methods
    }
}