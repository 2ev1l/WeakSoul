using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class EffectsOverlayItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => effectId;
        private int effectId;
        private Effect effect;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Text timer;
        [SerializeField] private ShowEffectHelp showEffectHelp;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            OnDisable();
            effectId = param;
            effect = GameData.Data.PlayerData.Stats.GetEffect(param);
            showEffectHelp.Effect = effect;
            OnEnable();
            spriteRenderer.sprite = EffectsInfo.Instance.GetEffect(effectId).Sprite;
		}
		private void OnEnable()
        {
            if (effect == null) return;
            effect.OnDurationChanged += CheckTimer;
            CheckTimer(effect.Duration);
        }
        private void OnDisable()
        {
            if (effect == null) return;
            effect.OnDurationChanged -= CheckTimer;
        }
        private void CheckTimer(int time)
        {
            int s = time % 60;
            int m = time / 60;
            timer.text = $"{m:00}:{s:00}";
        }
        #endregion methods

    }
}