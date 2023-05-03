using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
	public class ItemHelpUpdater : HelpUpdater
	{
		#region fields & properties
		public static ItemHelpUpdater Instance { get; private set; }
		[SerializeField] private LanguageLoader smallDescriptionText;
		[SerializeField] private LanguageLoader bigDescriptionText;
		[SerializeField] private LanguageLoader itemNameText;
		[SerializeField] private PhysicalStatsItemList itemList;

		[SerializeField] private Image bodyRenderer;
		[SerializeField] private SpriteRenderer rareRenderer;
		[SerializeField] private List<RareUIInfo> rareInfos;
		#endregion fields & properties

		#region methods
		public override void Init()
		{
			base.Init();
			Instance = this;
		}
		public void OpenPanel(Vector3 position, int itemId)
		{
			if (itemId < 0) return;
			base.OpenPanel(position);
			itemList.ItemId = itemId;
			ItemType type = ItemsInfo.Instance.GetItemType(itemId);
			itemNameText.Id = itemId;
			int rare = -1;
			switch (type)
			{
				case ItemType.SoulItem:
					bigDescriptionText.gameObject.SetActive(true);
					bigDescriptionText.Id = itemId;
					SoulItem soulItem = ItemsInfo.Instance.GetSoulItem(itemId);
					smallDescriptionText.gameObject.SetActive(soulItem.BreakChance > 0);
					if (soulItem.BreakChance > 0)
					{
						smallDescriptionText.ChangeValues(45, TextType.GameMenu);
						smallDescriptionText.AddText($" {soulItem.BreakChance}%");
					}
					break;
				case ItemType.Armor:
					rare = ItemsInfo.Instance.GetArmor(itemId).Rare;
					goto default;
				case ItemType.Weapon:
					rare = ItemsInfo.Instance.GetWeapon(itemId).Rare;
					goto default;
				default:
					bigDescriptionText.gameObject.SetActive(false);
					smallDescriptionText.gameObject.SetActive(true);
					smallDescriptionText.ChangeValues(itemId, TextType.ItemsDescription);
					int skillId = ItemsInfo.Instance.GetStatsItem(itemId).SkillId;
					string addText = "";

					if (type == ItemType.Weapon && !ItemsInfo.Instance.GetWeapon(itemId).IsPlayerClassAllowed())
						addText += $"<color=#AB2E26>{LanguageLoader.GetTextByType(TextType.GameMenu, 52)}</color>\n";

					if (skillId == -1)
					{
						smallDescriptionText.AddText(addText);
						break;
					}
					addText += LanguageLoader.GetTextByType(TextType.GameMenu, 40) + $": " +
									LanguageLoader.GetTextByType(TextType.SkillsName, skillId);

					smallDescriptionText.AddText(addText);
					break;
			}
			RareUIInfo rareInfo = rareInfos.Find(x => x.Rare == rare);
			bodyRenderer.material.SetFloat("_FadeAmount", rareInfo.BurnAmount);
			rareRenderer.sprite = rareInfo.Sprite;
			rareRenderer.material.SetVector("_OutlineColor", rareInfo.Color);
		}
		#endregion methods
	}
}