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
	[RequireComponent(typeof(CursorChanger))]
	[RequireComponent(typeof(ShowSkillHelp))]
	public class SkillMove : MaterialRaycastChanger, IPointerClickHandler
	{
		#region fields & properties
		[SerializeField] private SkillRender skillRender;
		private ShowSkillHelp Help
		{
			get
			{
				help = help == null ? GetComponent<ShowSkillHelp>() : help;
				return help;
			}
		}
		private ShowSkillHelp help;
		[SerializeField] private ShowTextHelp attackHelpWarning;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
			SkillsPanelInit.OnAttackSkillsChanged += CheckSkillsAllow;
        }
        private void OnDisable()
        {
			SkillsPanelInit.OnAttackSkillsChanged -= CheckSkillsAllow;
        }
        public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left || attackHelpWarning.enabled) return;
			if (SkillsPanelInit.CellController.FreeCell != -1)
				GameData.Data.PlayerData.Skills.SetItem(skillRender.Id, SkillsPanelInit.CellController.FreeCell);
		}
		private void CheckSkillsAllow() => CheckSkillAllow();
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Is skill allowed</returns>
		public bool CheckSkillAllow()
		{
			Skill skill = skillRender.Skill;
			if (skill.SkillType != SkillType.Attack || skill.Id == 9 || skill.Id == 38)
			{
				SetDefaultHelp();
				return true;
			}
			attackHelpWarning.enabled = SkillsPanelInit.Instance.AttackSkillsCount > 1;
			return !attackHelpWarning.enabled;
		}
		public void SetDefaultHelp()
		{
			Help.enabled = true;
			attackHelpWarning.enabled = false;
		}
		#endregion methods
	}
}