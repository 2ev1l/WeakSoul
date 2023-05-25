using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Universal;
using Universal.Effects;

namespace WeakSoul.Events.Fight
{
    public abstract class FightCardUI : MonoBehaviour
    {
        #region fields & properties
        public FightCard Card => card;
        protected LanguageLoader StaminaLanguage => staminaLanguage;
        public abstract Sprite Texture { get; }
        [Header("Information")]
        [SerializeField] private FightCard card;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private LanguageLoader staminaLanguage;
        [SerializeField] private PhysicalStatsItemListEntity statsList;
        [SerializeField] private CursorChanger mouseEventRaycast;
        [SerializeField] private GameObject choosedCardShadow;

        [Header("Animation")]
        [SerializeField] private CanvasAlphaChanger canvasAlphaChanger;
        [SerializeField] private List<GameObject> burnObjects;
        [SerializeField] private List<Text> burnTexsts;
        [SerializeField] private StatText statText;

        [Header("HP Bar")]
        [SerializeField] private SpriteRenderer hpBar;
        [SerializeField] private SpriteRenderer hpBarOld;
        [SerializeField] private ValueSmoothChanger hpBarValueChanger;
        [SerializeField] private ValueSmoothChanger hpBarOldValueChanger;

        [Header("VFX")]
        [SerializeField] private FightSkillsController skillsController;
        [SerializeField] private SkillVFXAnimation evasionVFX;
        [SerializeField] private SkillVFXAnimation physicalDefenseVFX;
        [SerializeField] private AudioClip defenseSound;
        [SerializeField] private AudioClip evasionSound;

        [Header("Debug")]
        [SerializeField][ReadOnly] private bool isInitialized = false;
        [SerializeField][ReadOnly] private int maxHp;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            card.OnCardInit += UpdateUI;
            if (!isInitialized)
                return;
            OnEnableInitialized();
        }
        private void OnDisable()
        {
            card.OnCardInit -= UpdateUI;
            if (!isInitialized)
                return;
            OnDisableInitialized();
        }
        protected virtual void OnEnableInitialized()
        {
            card.Stats.OnStaminaChanged += UpdateStamina;
            mouseEventRaycast.OnEnter += IncreaseCanvasAlpha;
            mouseEventRaycast.OnExit += DecreaseCanvasAlpha;
            card.Stats.OnDead += OnDead;
            TurnController.Instance.OnTurnEnd += CheckTurn;
            TurnController.Instance.OnTurnEnd += UpdateStamina;
            card.Stats.OnEvasion += OnEvasion;
            card.Stats.OnDamageBlocked += OnDamageBlocked;
            card.Stats.OnDamageGained += OnDamageGained;
            card.Stats.OnHealthChanged += OnHpChanged;
            UpdateStamina(card.Stats.Stamina);
            DecreaseCanvasAlpha();
        }
        protected virtual void OnDisableInitialized()
        {
            card.Stats.OnStaminaChanged -= UpdateStamina;
            mouseEventRaycast.OnEnter -= IncreaseCanvasAlpha;
            mouseEventRaycast.OnExit -= DecreaseCanvasAlpha;
            card.Stats.OnDead -= OnDead;
            TurnController.Instance.OnTurnEnd -= CheckTurn;
            TurnController.Instance.OnTurnEnd -= UpdateStamina;
            card.Stats.OnEvasion -= OnEvasion;
            card.Stats.OnDamageBlocked -= OnDamageBlocked;
            card.Stats.OnDamageGained -= OnDamageGained;
            card.Stats.OnHealthChanged -= OnHpChanged;
        }
        private void OnDamageGained(int value)
        {
            StatText.SpawnPrefab(statText, skillsController.AnimationTransform, skillsController.GetSkillAnimationPosition(FightPosition.Owner), PhysicalStatsType.Health, -value);
        }
        private void OnHpChanged(int value)
        {
            if (card.Stats.Health > maxHp)
                maxHp = card.Stats.Health;
            StartCoroutine(HPBarAnimation(hpBarValueChanger, hpBar));
            StartCoroutine(HPBarAnimation(hpBarOldValueChanger, hpBarOld));
        }
        private IEnumerator HPBarAnimation(ValueSmoothChanger vsc, SpriteRenderer bar)
        {
            float finalValue = 1f - ((float)card.Stats.Health / maxHp);
            finalValue = Mathf.Clamp01(finalValue);
            vsc.StartChange(bar.material.GetFloat("_ClipUvLeft"), finalValue, 1f);
            while (!vsc.IsChangeEnded)
            {
                bar.material.SetFloat("_ClipUvLeft", vsc.Out);
                yield return CustomMath.WaitAFrame();
            }
            bar.material.SetFloat("_ClipUvLeft", vsc.Out);
        }
        private void OnDamageBlocked(DamageType damageType)
        {
            List<SkillVFX> vfxs = physicalDefenseVFX.VFXs.ToList();
            vfxs.ForEach(x => x.SetPosition(skillsController.GetSkillAnimationPosition(x.FightPosition)));
            vfxs.ForEach(x => { if (x.ExposedName == "DefenseValue") x.SetParamValue(card.Stats.Defense); });
            StartCoroutine(VFXAnimation.Animate(skillsController.EffectPrefab, skillsController.AnimationTransform, vfxs));
            Invoke(nameof(PlayDefenseSound), 1f);
        }
        private void PlayDefenseSound() => AudioManager.PlayClip(defenseSound, Universal.AudioType.Sound);
        private void OnEvasion()
        {
            List<SkillVFX> vfxs = evasionVFX.VFXs.ToList();
            vfxs.ForEach(x => x.SetPosition(skillsController.GetSkillAnimationPosition(x.FightPosition)));
            StartCoroutine(VFXAnimation.Animate(skillsController.EffectPrefab, skillsController.AnimationTransform, vfxs));
            Invoke(nameof(PlayEvasionSound), 1f);
        }
        private void PlayEvasionSound() => AudioManager.PlayClip(evasionSound, Universal.AudioType.Sound);
        private void CheckTurn(FightCard card)
        {
            bool isMyTurn = card != Card;
            choosedCardShadow.SetActive(isMyTurn);
        }
        private void OnDead()
        {
            mouseEventRaycast.enabled = false;
            canvasAlphaChanger.StartChange(canvasAlphaChanger.CanvasGroup.alpha, 0f, 0.3f, true);
            StartCoroutine(BurnCard());
        }
        private IEnumerator BurnCard()
        {
            AudioManager.PlayClip(AudioStorage.Instance.BurnSound, Universal.AudioType.Sound);
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            List<Burn> burnEffects = new();
            burnObjects.ForEach(x => burnEffects.Add(x.AddComponent<Burn>()));
            burnEffects.ForEach(x => x.StartAnimation(MaterialsInfo.Instance.Fire_Blue, 0.8f));
            vsc.StartChange(1, 0, 0.8f);
            while (!burnEffects.Last().IsAnimationEnded && !vsc.IsChangeEnded)
            {
                burnTexsts.ForEach(x =>
                {
                    Color col = x.color;
                    col.a = vsc.Out;
                    x.color = col;
                });
                yield return CustomMath.WaitAFrame();
            }
            AfterDeadAnimation();
        }
        protected virtual void AfterDeadAnimation()
        {
            gameObject.SetActive(false);
        }
        private void UpdateStamina(FightCard card) => UpdateStaminaOnTurn(this.card.Stats.Stamina, card);
        private void UpdateStamina(int stamina) => UpdateStaminaOnTurn(stamina, TurnController.Instance.OldTurnCard);
        private void UpdateStaminaOnTurn(int stamina, FightCard card)
        {
            bool isMyTurn = card != this.card;
            bool isStaminaReachedMax = this.card.Stats.Stamina >= this.card.Stats.MaxStamina;
            StaminaLanguage.AddText($" {stamina}{(isStaminaReachedMax ? "" : (isMyTurn ? "" : $" (+{Card.Stats.StaminaRegen})"))}");
        }
        private void IncreaseCanvasAlpha()
        {
            canvasAlphaChanger.StartChange(canvasAlphaChanger.CanvasGroup.alpha, 1, 0.3f);
        }
        private void DecreaseCanvasAlpha()
        {
            canvasAlphaChanger.StartChange(canvasAlphaChanger.CanvasGroup.alpha, 0.1f, 0.3f);
        }
        protected virtual void UpdateUI()
        {
            if (!isInitialized)
            {
                OnDisable();
                isInitialized = true;
                OnEnable();
                maxHp = card.Stats.Health;
            }
            statsList.EnitityStats = card.Stats;
            spriteRenderer.sprite = Texture;
        }
        #endregion methods
    }
}