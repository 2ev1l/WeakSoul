using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Skills
{
	[RequireComponent(typeof(Image))]
	public class SkillBuy : MonoBehaviour, IPointerClickHandler
	{
		#region fields & properties
		[SerializeField] private SkillRender skillRender;
		public bool CanBuy => canBuy;
		[SerializeField][ReadOnly] private bool canBuy;
		#endregion fields & properties

		#region methods
		private void CheckAllow(int sp) => CheckAllow();
		private void CheckAllow()
		{
			canBuy = skillRender.Skill.CanOpenSkill();
			skillRender.SpriteRenderer.material = !canBuy ? SkillsPanelInit.Instance.SkillDeffault : SkillsPanelInit.Instance.SkillNotBuyed;
			enabled = canBuy;
		}
		public void Buy()
		{
			if (!skillRender.Skill.TryOpenSkill())
				throw new System.ArithmeticException("skill price");
			CheckAllow();
			skillRender.TryRender();
		}
		public void OnEnable()
		{
			GameData.Data.PlayerData.Stats.OnSkillPointsChanged += CheckAllow;
			CheckAllow();
		}
		public void OnDisable()
		{
			GameData.Data.PlayerData.Stats.OnSkillPointsChanged -= CheckAllow;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left) return;
			Buy();
			this.enabled = false;
		}
		#endregion methods
	}
}