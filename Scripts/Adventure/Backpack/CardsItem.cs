using Data;
using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Universal;

namespace WeakSoul.Adventure.Backpack
{
    public class CardsItem : CursorChanger, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => cardData.Id;
        private CardData cardData;

        [Header("UI")]
        [SerializeField] private SpriteRenderer mainSpriteRenderer;
        [SerializeField] private SpriteRenderer borderSpriteRenderer;
        [SerializeField] private LanguageLoader cardNameLanguage;
        [SerializeField] private LanguageLoader cardDescriptionLanguage;
        [SerializeField] private LanguageLoader karmaLanguage;
        [SerializeField] private ValueSmoothChanger scaler;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            OnEnter += ScaleCardUp;
            OnExit += ScaleCardDown;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OnEnter -= ScaleCardUp;
            OnExit -= ScaleCardDown;
        }
        public void OnListUpdate(int param)
        {
            cardData = CardsInfo.Instance.GetCard(param);
            mainSpriteRenderer.sprite = cardData.Texture;
            cardNameLanguage.Id = cardData.Id;
            cardDescriptionLanguage.Id = cardData.Id;
            UpdateKarma();
        }
        private void UpdateKarma()
        {
            ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
            bool isSoulItem_Karma = inventory.ContainItem(83);
			if (!isSoulItem_Karma)
            {
                karmaLanguage.Id = -1;
                return;
            }
            karmaLanguage.AddText($" {cardData.Karma}");
            if (UnityEngine.ColorUtility.TryParseHtmlString(cardData.Karma >= 0 ? "#58C094" : "#AB403F", out Color newCol))
                karmaLanguage.Text.color = newCol;
        }
        private void ScaleCardUp()
        {
            mainSpriteRenderer.sortingOrder = -10;
            borderSpriteRenderer.sortingOrder = -9;
            StartCoroutine(ScaleCard(transform.localScale.x, 1.2f));
        }
        private void ScaleCardDown()
        {
            mainSpriteRenderer.sortingOrder = -12;
            borderSpriteRenderer.sortingOrder = -11;
            StartCoroutine(ScaleCard(transform.localScale.x, 0.8f));
        }
        private IEnumerator ScaleCard(float start, float end)
        {
            scaler.StartChange(start, end, 0.5f);
            while (!scaler.IsChangeEnded)
            {
                Vector3 localScale = transform.localScale;
                localScale = scaler.Out * Vector3.one;
                transform.localScale = localScale;
                yield return CustomMath.WaitAFrame();
            }
        }
        #endregion methods
    }
}