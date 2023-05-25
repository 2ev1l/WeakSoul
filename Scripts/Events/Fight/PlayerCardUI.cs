using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.Events.Fight
{
	public class PlayerCardUI : FightCardUI
	{
		#region fields & properties
		[SerializeField][ReadOnly] private int escapeChance = 10;
		public override Sprite Texture => playerSprite;
		[Header("Player Card UI")]
		[SerializeField] private Sprite playerSprite;
		[SerializeField] private Image skillRaycastBlock;
		[SerializeField] private GameObject buttonNextTurn;

		public LanguageLoader EscapeButtonLanguage
		{
			get
			{
				escapeButtonLanguage = escapeButtonLanguage == null ? GameObject.FindObjectsOfType<LanguageLoader>(true).Where(x => x.gameObject.name.Equals("Escape")).First() : escapeButtonLanguage;
				return escapeButtonLanguage;
			}
		}
		private LanguageLoader escapeButtonLanguage;
		private static readonly int turnsToActivateEscapeButton = 10;

		[Header("Equipment")]
		[SerializeField] private GameObject unequippedHead;
		[SerializeField] private GameObject unequippedBody;
		[SerializeField] private GameObject unequippedLegs;
		[SerializeField] private GameObject unequippedSword;

		[SerializeField] private AudioClip turnStartSound;
		private bool isFightEnd = false;
		#endregion fields & properties

		#region methods
		protected override void OnEnableInitialized()
		{
			base.OnEnableInitialized();
			TurnController.Instance.OnTurnEnd += CheckTurnEnd;
			FightCard.OnFightEnd += OnFightEnd;
			SkillUse.OnSkillAnimationChanged += CheckSkillUse;
			SkillUse.OnSkillAnimationChanged += CheckEscape;
			TurnController.Instance.OnTurnEnd += CheckEscape;
			CheckEquipment();
		}
		protected override void OnDisableInitialized()
		{
			base.OnDisableInitialized();
			TurnController.Instance.OnTurnEnd -= CheckTurnEnd;
			FightCard.OnFightEnd -= OnFightEnd;
			SkillUse.OnSkillAnimationChanged -= CheckSkillUse;
			SkillUse.OnSkillAnimationChanged += CheckEscape;
			TurnController.Instance.OnTurnEnd -= CheckEscape;
		}
		private void CheckEscape(bool isSkillAnimation) => CheckEscape(isSkillAnimation ? Card : TurnController.Instance.OldTurnCard);
		private void CheckEscape(FightCard card)
		{
			bool isMyTurn = card != Card;
			EscapeButtonLanguage.gameObject.SetActive(TurnController.Instance.CurrentTurn > turnsToActivateEscapeButton && isMyTurn && !isFightEnd);
			if (!isMyTurn) return;
			bool isSoulItem_EscapeChance = GameData.Data.PlayerData.Inventory.ContainItem(91);
			escapeChance += isSoulItem_EscapeChance ? Random.Range(1, 2) : 1;
			escapeChance = Mathf.Clamp(escapeChance, 0, 100);
			EscapeButtonLanguage.AddText($" ({escapeChance}%)");
		}
		public void TryEscape()
		{
			if (CustomMath.GetRandomChance(escapeChance))
			{
				Escape();
				PlayerCard.OnPlayerEscaped?.Invoke();
            }
			else
			{
				int damage = Mathf.Max(Mathf.RoundToInt(Card.Stats.Health * 0.2f), 1);
				Card.Stats.GetUnscaledDamageSelf(damage, Card.OwnEnemy.Stats.DamageType);
				TurnController.Instance.NextTurn();
			}
		}
		private void Escape()
		{
			SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
		}
		private void CheckEquipment()
		{
			ItemsInventory itemsInventory = GameData.Data.PlayerData.Inventory;
			unequippedHead.SetActive(itemsInventory.HeadArmor == null);
			unequippedBody.SetActive(itemsInventory.BodyArmor == null);
			unequippedLegs.SetActive(itemsInventory.LegsArmor == null);
			unequippedSword.SetActive(itemsInventory.Weapon == null);
		}
		private void CheckTurnEnd(FightCard card)
		{
			bool isMyTurn = card != Card;
			SetActivePlayerUI(isMyTurn && !isFightEnd);
			if (isMyTurn)
				AudioManager.PlayClip(turnStartSound, Universal.AudioType.Sound);
		}
		private void CheckSkillUse(bool isAnimation)
		{
			if (TurnController.Instance.CurrentTurnCard != Card) return;
			SetActivePlayerUI(!isAnimation && !isFightEnd);
		}
		private void OnFightEnd(bool isLastEnemy)
		{
			isFightEnd = true;
			CheckEscape(Card);
			SetActivePlayerUI(false);
		}
		private void SetActivePlayerUI(bool active)
		{
			skillRaycastBlock.enabled = !active;
			buttonNextTurn.SetActive(active);
		}
		#endregion methods
	}
}