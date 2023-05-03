using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Effects;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class HealthZone : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] Vector2 localPositionsX;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material changedMaterial;
        public bool IsChecked { get; private set; } = false;
        public bool IsCollided { get; private set; } = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            HealthMiniGame.Instance.OnGameEnd += ResetValues;
            HealthMiniGame.Instance.OnGameRestart += Restart;
            ResetValues();
        }
        private void OnDisable()
        {
            HealthMiniGame.Instance.OnGameEnd -= ResetValues;
            HealthMiniGame.Instance.OnGameRestart -= Restart;
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
            IsChecked = false;
            boxCollider.enabled = true;
        }
        public void DisableZone()
        {
            spriteRenderer.material = changedMaterial;
            boxCollider.enabled = false;
            IsCollided = false;
            IsChecked = true;
        }
        private void ResetPosition()
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = Random.Range(localPositionsX.x, localPositionsX.y);
            transform.localPosition = localPos;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out HealthMove move))
            {
                IsCollided = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out HealthMove move))
            {
                IsCollided = false;
            }
        }
        #endregion methods
    }
}