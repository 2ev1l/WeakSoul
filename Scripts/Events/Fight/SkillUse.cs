using Data;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.GameMenu.Skills;

namespace WeakSoul.Events.Fight
{
	public class SkillUse : MonoBehaviour
	{
		#region fields & properties
		public UnityAction OnCooldownChanged;
		public static UnityAction<bool> OnSkillAnimationChanged;
		public static bool IsSkillAnimation
		{
			get => isSkillAnimation;
			set
			{
				isSkillAnimation = value;
				OnSkillAnimationChanged?.Invoke(isSkillAnimation);
			}
		}
		private static bool isSkillAnimation;

		[Header("Functional")]
		[SerializeField] private SkillsCell cell;
		[SerializeField] private FightSkillsController controller;
		[SerializeField] private CustomButton customButton;
		[SerializeField] private bool isEnemySkill;

		[Header("UI")]
		[SerializeField] private SpriteRenderer closeUI;
		[SerializeField] private ShowSkillHelp skillHelp;
		[SerializeField] private Material usedChooseMaterial;
		[SerializeField] private Material defaultChooseMaterial;
		[SerializeField] private MaterialRaycastChanger materialRaycastChanger;

		public Skill Skill => skill;
		[Header("Debug")]
		[SerializeField][ReadOnly] private Skill skill = new(-1);
		public bool IsInitialized => isInitialized;
		[SerializeField][ReadOnly] private bool isInitialized;
		private int CurrentCooldown
		{
			get => currentCooldown;
			set
			{
				currentCooldown = value;
				OnCooldownChanged?.Invoke();
			}
		}
		[SerializeField][ReadOnly] private int currentCooldown = 0;
		public bool CanUse => canUse;
		[SerializeField][ReadOnly] private bool canUse;

		private static readonly string badColor = "AB2E26";
		private static readonly string goodColor = "26AB66";
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			TryInit();
			OnEnableInitialized();
		}
		private void OnDisable()
		{
			OnDisableInitialized();
		}
		protected virtual void OnEnableInitialized()
		{
			customButton.OnClicked += UseThisSkill;
			controller.Card.Stats.OnStaminaChanged += CheckUse;
			TurnController.Instance.OnTurnEnd += DecreaseCooldown;
			OnCooldownChanged += CheckUse;
			OnSkillAnimationChanged += CheckUse;
			CheckUse();
		}
		protected virtual void OnDisableInitialized()
		{
			customButton.OnClicked -= UseThisSkill;
			controller.Card.Stats.OnStaminaChanged -= CheckUse;
			TurnController.Instance.OnTurnEnd -= DecreaseCooldown;
			OnCooldownChanged -= CheckUse;
			OnSkillAnimationChanged -= CheckUse;
		}
		public void TryInit()
		{
			if (!controller.Card.IsInitialized || isInitialized || cell.ItemId == -1) return;
			Init();
		}
		private void Init()
		{
			isInitialized = true;
			skill = SkillsInfo.Instance.GetSkill(cell.ItemId).Clone();
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			if (playerLevel > 16 && !isEnemySkill)
			{
				CurrentCooldown = Random.Range(0, skill.Cooldown + 2);
			}
		}
		private void DecreaseCooldown(FightCard card) => DecreaseCooldown();
		private void DecreaseCooldown() => CurrentCooldown = Mathf.Max(0, CurrentCooldown - 1);
		private void CheckUse(bool isSkillAnimation) => CheckUse();
		private void CheckUse() => CheckUse(controller.Card.Stats.Stamina, CurrentCooldown);
		private void CheckUse(int stamina) => CheckUse(stamina, CurrentCooldown);
		private void CheckUse(int stamina, int currentCooldown)
		{
			bool turnUse = controller.Card == TurnController.Instance.CurrentTurnCard;
			bool cooldownUse = currentCooldown <= 0;
			bool staminaUse = stamina >= skill.StaminaPrice;
			bool idUse = skill.Id > -1;
			canUse = cooldownUse && staminaUse && turnUse && idUse && !IsSkillAnimation && isInitialized;
			customButton.enabled = canUse;
			closeUI.enabled = !canUse;
			skillHelp.CooldownColor = cooldownUse ? goodColor : badColor;
			skillHelp.CooldownValue = $"{currentCooldown}/{skill.Cooldown}";
			bool panelState = skillHelp.PanelState;
			skillHelp.HidePanel();
			if (panelState)
				skillHelp.OpenPanel();
			materialRaycastChanger.SetChangedMaterial(canUse ? defaultChooseMaterial : usedChooseMaterial);
		}
		private void UseThisSkill()
		{
			if (!canUse) return;
			StartCoroutine(UseSkill());
		}
		public IEnumerator UseSkill()
		{
			IsSkillAnimation = true;
			CurrentCooldown = skill.Cooldown;
			yield return controller.UseSkill(skill);
			yield return CustomMath.WaitAFrame();
			IsSkillAnimation = false;
		}
		#endregion methods
	}
}