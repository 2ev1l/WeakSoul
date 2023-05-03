using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Skills
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SkillRender : MonoBehaviour
	{
		#region fields & properties
		public int Id => id;
		[SerializeField] private int id;
		[SerializeField] private SkillBuy skillBuy;
		[SerializeField] private SkillMove skillMove;
		[SerializeField] private ShowSkillHelp help;
		[SerializeField] private CursorChanger cursorChanger;
		[SerializeField] private Image raycastImage;
		[SerializeField] private List<SkillUI> directions;
		public SpriteRenderer SpriteRenderer
		{
			get
			{
				spriteRenderer = spriteRenderer == null ? GetComponent<SpriteRenderer>() : spriteRenderer;
				return spriteRenderer;
			}
		}
		private SpriteRenderer spriteRenderer;
		private Sprite MainSprite
		{
			get
			{
				mainSprite = mainSprite == null ? SkillsInfo.Instance.GetTextureByType(Skill.SkillType) : mainSprite;
				return mainSprite;
			}
		}
		private Sprite mainSprite;
		public Skill Skill
		{
			get
			{
				skill ??= SkillsInfo.Instance.GetSkill(id);
				return skill;
			}
		}
		private Skill skill;
		public bool IsTempOpened => Skill.IsTempOpened && !Skill.IsOpened;
		public bool IsSkillEquipped => GameData.Data.PlayerData.Skills.Items.Contains(id);
		public bool IsInitializedByParent
		{
			get
			{
				TrySaveDefaultState();
				return isInitializedByParent;
			}
			set
			{
				TrySaveDefaultState();
				isInitializedByParent = value;
			}
		}
		[SerializeField] private bool isInitializedByParent = false;
		[Universal.ReadOnly][SerializeField] private bool defaultInitialized;
		[Universal.ReadOnly][SerializeField] private bool defaultInitialization;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			GameData.Data.PlayerData.Skills.OnInventoryChanged += CheckMovedSkill;
			SavingUtils.OnDataReset -= ResetInitialization;
			TryRender();
		}
		private void OnDisable()
		{
			GameData.Data.PlayerData.Skills.OnInventoryChanged -= CheckMovedSkill;
			SavingUtils.OnDataReset += ResetInitialization;
		}
		private void OnDestroy()
		{
			SavingUtils.OnDataReset -= ResetInitialization;
		}
		private void Start()
		{
			help.SkillId = id;
		}
		private void ResetInitialization()
		{
			IsInitializedByParent = defaultInitialization;
		}
		private void TrySaveDefaultState()
		{
			if (defaultInitialized) return;
			defaultInitialized = true;
			defaultInitialization = isInitializedByParent;
		}
		private void CheckMovedSkill(int itemId, int newCellId, int oldCellId)
		{
			if (newCellId != oldCellId && newCellId != 6) return;
			TryRender();
		}
		public void TryRender()
		{
			EnablePartialUI();
			bool equipped = IsSkillEquipped;
			bool isOpened = Skill.IsOpened;
			bool levelAccess = Skill.LevelAccess();
			bool hasFreeCell = GameData.Data.PlayerData.Skills.GetFreeCell() > -1;
			if (isOpened)
				directions.ForEach(x =>
				{
					if (!x.NextSkill.IsInitializedByParent)
					{
						if (!IsTempOpened)
							x.NextSkill.IsInitializedByParent = true;
						x.NextSkill.TryRender();
						x.Line.sprite = SkillsPanelInit.Instance.GetLineTexture(Skill.SkillType);
					}
					x.Line.gameObject.SetActive(!IsTempOpened);
				});
			else
				directions.ForEach(x =>
				{
					if (!x.NextSkill.IsInitializedByParent)
					{
						bool ito = x.NextSkill.IsTempOpened;
						bool itoo = ito && (IsTempOpened || isOpened);
						x.NextSkill.TryRender();
					}
					x.Line.gameObject.SetActive(false);
				});

			if (!IsInitializedByParent && !IsTempOpened && !isOpened)
			{
				DisableUI();
				return;
			}
			skillMove.SpriteRenderer.sprite = isOpened ? (equipped ? null : Skill.Texture) : SkillsPanelInit.Instance.LockedSkill;
			
			skillMove.enabled = isOpened && !equipped && levelAccess && hasFreeCell;

			skillBuy.enabled = !isOpened && !IsTempOpened;
			cursorChanger.enabled = (skillMove.SpriteRenderer.sprite != null && hasFreeCell && skillMove.enabled) || (skillBuy.enabled && skillBuy.CanBuy);
			help.enabled = skillMove.SpriteRenderer.sprite != null;
			raycastImage.enabled = help.enabled;
			if (!skillMove.CheckSkillAllow())
			{
				if (skillMove.SpriteRenderer.sprite == SkillsPanelInit.Instance.LockedSkill)
					skillMove.SetDefaultHelp();
				else
					cursorChanger.enabled = false;
			}

			if (!skillBuy.enabled)
				skillBuy.OnDisable();
		}
		private void DisableUI()
		{
			SpriteRenderer.sprite = null;
			skillMove.SpriteRenderer.sprite = null;
			skillMove.enabled = false;
			skillBuy.enabled = false;
			cursorChanger.enabled = false;
			help.enabled = false;
			raycastImage.enabled = false;
			skillBuy.OnDisable();
		}
		private void EnablePartialUI()
		{
			SpriteRenderer.sprite = MainSprite;
		}
		#endregion methods
	}
}