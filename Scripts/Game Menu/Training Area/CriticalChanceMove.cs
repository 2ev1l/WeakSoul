using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class CriticalChanceMove : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private MiniGame miniGame;
        [SerializeField] private SpriteRenderer zoneSpriteRenderer;
        [SerializeField] private SpriteRenderer thisSpriteRenderer;
        [SerializeField] private Vector2 zoneScaleRange;
        [SerializeField] private float defaultScale;
        [SerializeField] private Material zoneGoodMaterial;
        [SerializeField] private Material zoneBadMaterial;
        public bool IsZoneChecked { get; private set; } = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            miniGame.OnGameRestart += RandomizeAndReset;
            miniGame.OnGameEnd += ResetValues;
            ResetValues();
        }
        private void OnDisable()
        {
            miniGame.OnGameRestart -= RandomizeAndReset;
            miniGame.OnGameEnd -= ResetValues;
            ResetValues();
        }
        public void StartCheckZone()
        {
            CheckZone();
        }
        private void RandomizeAndReset()
        {
            ResetValues();
            RandomizeZone();
        }
        private void RandomizeZone()
        {
            zoneSpriteRenderer.transform.localScale = Vector3.one * Random.Range(zoneScaleRange.x, zoneScaleRange.y);
        }
        private void ResetValues()
        {
            StopCheckZone();
            IsZoneChecked = false;
            CheckMaterial();
            zoneSpriteRenderer.transform.localScale = Vector3.one * zoneScaleRange.x;
            transform.localScale = Vector3.one * defaultScale;
        }
        private void CheckZone()
        {
            float scaleDecrease = defaultScale * Time.deltaTime / miniGame.timeDeviation * 2;
            transform.localScale -= Vector3.one * scaleDecrease;
            if (transform.localScale.x <= 0)
            {
                StopCheckZone();
                transform.localScale = Vector3.zero;
            }
            bool isz = IsZoneChecked;
            IsZoneChecked = (Mathf.Abs(zoneSpriteRenderer.transform.localScale.x - transform.localScale.x) < 40f / miniGame.timeDeviation);
            if (isz != IsZoneChecked)
                CheckMaterial();
            Invoke(nameof(CheckZone), Time.deltaTime);
        }
        private void CheckMaterial()
        {
            thisSpriteRenderer.material = IsZoneChecked ? zoneGoodMaterial : zoneBadMaterial;
        }
        public void StopCheckZone()
        {
            CancelInvoke(nameof(CheckZone));
        }
        #endregion methods
    }
}