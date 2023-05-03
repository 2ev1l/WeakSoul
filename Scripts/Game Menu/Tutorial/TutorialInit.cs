using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Universal;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu
{
	public class TutorialInit : TutorialPanel
	{
		#region fields & properties
		[SerializeField] private GameObject classesPanel;
		[SerializeField] private GameObject shopIcon;
		[SerializeField] private GameObject adventureIcon;
		[SerializeField] private GameObject raycastBlock;
		private GameObject EquipIcon
		{
			get
			{
				if (equipIcon == null)
					equipIcon = GameObject.Find("Icon-Help Mark-Small_Equip");
				return equipIcon;
			}
		}
		[SerializeField][ReadOnly] private GameObject equipIcon;
		private GameObject SkillsIcon
		{
			get
			{
				if (skillsIcon == null)
					skillsIcon = GameObject.Find("Icon-Help Mark-Small_Skills");
				return skillsIcon;
			}
		}
		[SerializeField][ReadOnly] private GameObject skillsIcon;
		[SerializeField] private List<GameObject> tutorialTexts;

		[SerializeField] private VideoPlayer playerPrefab;
		[SerializeField] private VideoClip equipmentClip;
		[SerializeField] private VideoClip skillsClip;
		[SerializeField] private Transform spawnCanvas;
		[SerializeField][ReadOnly] private VideoPlayer currentPlayer;

		protected override int Progress => TutorialData.Progress;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			if (GameData.Data.TutorialData.IsCompleted) return;
			InputController.OnKeyDown += CheckKeyDown;
			ClassChoose.OnClassChoosed += CheckClassChoose;
		}
		private void OnDisable()
		{
			InputController.OnKeyDown -= CheckKeyDown;
			ClassChoose.OnClassChoosed -= CheckClassChoose;
		}
		private void CheckClassChoose(ClassChoose classChoose)
		{
			CheckTutorialStep();
		}
		private void CheckKeyDown(KeyCode keyCode)
		{
			if ((keyCode == KeyCode.Escape || keyCode == KeyCode.E || keyCode == KeyCode.S) && currentPlayer != null)
				Destroy(currentPlayer.gameObject);
			if (keyCode == KeyCode.E)
			{
				StartCoroutine(Check5StepAfterFrame());
			}
			if (keyCode == KeyCode.S)
			{
				CheckTutorialStep(6);
			}
		}
		private IEnumerator Check5StepAfterFrame()
		{
			yield return CustomMath.WaitAFrame();
			if (InventoryPanelInit.Instance.Panel.activeSelf) yield break;
			CheckTutorialStep(5);
			CheckTutorialStep(5);
		}
		protected override void CheckTutorial()
		{
			if (GameData.Data.TutorialData.IsCompleted)
			{
				CheckFlags();
				if (GameData.Data.Days != 0) return;
				GameData.Data.Days = 1;
				return;
			}
			shopIcon.SetActive(false);
			adventureIcon.SetActive(false);
			EquipIcon.SetActive(false);
			SkillsIcon.SetActive(false);
			tutorialTexts.ForEach(x => x.SetActive(true));
			if (TryCheckTutorialStep(7)) return;
			ShowPanel(0);
		}
		private void CheckFlags()
		{
			AdventureData adventureData = GameData.Data.AdventureData;
			PlayerData playerData = GameData.Data.PlayerData;
			int playerLevel = playerData.Stats.ExperienceLevel.Level;
			if (adventureData.IsBossAllowedForPlayer(out int bossId) && !adventureData.IsBossDefeated(bossId))
			{
				switch (bossId)
				{
					case 0:
						if (TryAddFlagAndShowPanel(TutorialFlag.Boss0Info, 23)) return;
						break;
					case 1:
						if (TryAddFlagAndShowPanel(TutorialFlag.Boss1Info, 25)) return;
						break;
					case 2:
						if (TryAddFlagAndShowPanel(TutorialFlag.Boss2Info, 31)) return;
						break;
					case 3: break;
					case 4: break;
					default: throw new System.NotImplementedException($"ATText Boss id {bossId}");
				}
			}
			
			if (playerLevel > 16 && adventureData.IsBossDefeated(0) && TryAddFlagAndShowPanel(TutorialFlag.Boss0Completed, 27)) return;
			if (playerLevel > 41 && adventureData.IsBossDefeated(2) && TryAddFlagAndShowPanel(TutorialFlag.Boss2Completed, 32)) return;
			if (playerLevel > 45 && TryAddFlagAndShowPanel(TutorialFlag.ShopRestore, 33)) return;

			if (adventureData.IsBossNotDefeatedWhenMust(out bossId))
			{
				switch (bossId)
				{
					case 0:
						if (TryAddFlagAndShowPanel(TutorialFlag.Boss0Warning, 24)) return;
						break;
					case 1:
						if (TryAddFlagAndShowPanel(TutorialFlag.Boss1Warning, 26)) return;
						break;
					case 2: break;
					case 3: break;
					case 4: break;
					default: throw new System.NotImplementedException($"DTText Boss id {bossId}");
				}
			}
			if (playerLevel >= 4 && TryAddFlagAndShowPanel(TutorialFlag.TrainingAreaInfo, 28)) return;
			if (playerLevel >= 16 && TryAddFlagAndShowPanel(TutorialFlag.BlacksmithInfo, 29)) return;
			if (playerLevel >= 32 && TryAddFlagAndShowPanel(TutorialFlag.WitchInfo, 30)) return;
		}
		private bool TryAddFlagAndShowPanel(TutorialFlag flag, int textId)
		{
			TutorialData tutorialData = GameData.Data.TutorialData;
			if (tutorialData.TryAddNewFlag(flag))
			{
				ShowPanel(textId, "", true);
				return true;
			}
			return false;
		}
		public override void CheckTutorialStep()
		{
			if (GameData.Data.TutorialData.IsCompleted)
			{
				HidePanel();
				return;
			}
			switch (Progress)
			{
				case 0:
					ShowPanel(1);
					IncreaseProgress();
					break; //first day => go to shop
				case 1:
					IncreaseProgress();
					shopIcon.SetActive(true);
					goto default; //shop open
				case 2:
					ShowPanel(2);
					IncreaseProgress();
					GameData.Data.PlayerData.Wallet.SetSoulsByType(1, SoulType.Weak);
					break; //souls give
				case 3:
					IncreaseProgress();
					goto default; //hide
				case 4:
					int mainTextId4 = 3;
					string additionalText4 = $"\n\n{LanguageLoader.GetTextByType(TextType.Help, 6)}";
					if (!GameData.Data.PlayerData.Inventory.ContainItem(3))
					{
						mainTextId4 = 18;
						additionalText4 = "";
						if (GameData.Data.PlayerData.Wallet.WeakSouls >= 1)
						{
							additionalText4 = $"\n\n{LanguageLoader.GetTextByType(TextType.Tutorial, 14)}";
							GameData.Data.PlayerData.Wallet.SetSoulsByType(0, SoulType.Weak);
						}
						additionalText4 += $"\n\n{LanguageLoader.GetTextByType(TextType.Help, 6)}";
						GameData.Data.PlayerData.Inventory.SetItem(3, GameData.Data.PlayerData.Inventory.GetFreeCell());
					}
					else if (GameData.Data.PlayerData.Wallet.WeakSouls >= 1)
					{
						mainTextId4 = 14;
						additionalText4 = $"\n\n{LanguageLoader.GetTextByType(TextType.Tutorial, 3)}\n\n{LanguageLoader.GetTextByType(TextType.Help, 6)}";
						GameData.Data.PlayerData.Wallet.SetSoulsByType(0, SoulType.Weak);
					}
					ShowPanel(mainTextId4, additionalText4);
					EquipIcon.SetActive(true);
					currentPlayer = InitializePlayer(playerPrefab, spawnCanvas, equipmentClip);
					IncreaseProgress();
					break;//need equip
				case 5:
					if (currentPlayer != null)
						Destroy(currentPlayer.gameObject);
					if (TextPanel.activeSelf) goto default;
					int mainTextId5 = 4;
					string additionalText5 = $"\n\n{LanguageLoader.GetTextByType(TextType.Help, 7)}";
					ShowPanel(mainTextId5, additionalText5);
					IncreaseProgress();
					SkillsIcon.SetActive(true);
					currentPlayer = InitializePlayer(playerPrefab, spawnCanvas, skillsClip);
					break; //skills open
				case 6:
					if (currentPlayer != null)
						Destroy(currentPlayer.gameObject);
					if (!GameData.Data.PlayerData.Skills.IsSkillOpened(0))//attack
					{
						if (GameData.Data.PlayerData.Skills.IsSkillOpened(2))//evade
						{
							if (GameData.Data.PlayerData.Stats.SkillPoints == 0)
							{
								ShowPanel(12, $"\n\n{LanguageLoader.GetTextByType(TextType.Tutorial, 13)}");
								GameData.Data.PlayerData.Stats.SkillPoints = 1;
								return;
							}
							ShowPanel(12);
							return;
						}
						ShowPanel(11);
						return;
					}
					if (TextPanel.activeSelf && !GameData.Data.PlayerData.Skills.IsSkillOpened(0)) goto default;
					ShowPanel(5);
					IncreaseProgress();
					adventureIcon.SetActive(true);
					break; //to adventure
				case 7:
					if (TextPanel.activeSelf) goto default;
					ShowPanel(6, "\n\n" + LanguageLoader.GetTextByType(TextType.Tutorial, GameData.Data.PlayerData.Stats.Karma >= 0 ? 10 : 9));
					IncreaseProgress();
					break; //get from adventure (alive)
				case 8:
					ShowPanel(7);
					IncreaseProgress();
					break;//goal
				case 9:
					ShowPanel(22);
					IncreaseProgress();
					break;//class choose text
				case 10:
					classesPanel.SetActive(true);
					IncreaseProgress();
					goto default; // class choose
				case 11:
					ShowPanel(8);
					IncreaseProgress();
					break;//tip
				case 12:
					GameData.Data.TutorialData.IsCompleted = true;
					GameData.Data.PlayerData.Inventory.SetItem(6, GameData.Data.PlayerData.Inventory.GetFreeCell());
					SceneLoader.Instance.LoadSceneFade("Game Menu", 2f);
					goto default;//end
				default: HidePanel(); break;
			}
		}
		public void ShowPanel(int languageId, string additionalText = "", bool enableRaycastBlock = false)
		{
			raycastBlock.SetActive(enableRaycastBlock);
			base.ShowPanel(languageId, additionalText);
		}
		public override void HidePanel()
		{
			raycastBlock.SetActive(false);
			base.HidePanel();
		}
		private void IncreaseProgress() => TutorialData.Progress++;
		#endregion methods
	}
}