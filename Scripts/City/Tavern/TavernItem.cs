using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.City.Tavern
{
	public class TavernItem : BuyCustomItem, IListUpdater
	{
		#region fields & properties
		public GameObject rootObject => gameObject;
		public int listParam => TavernItemList.Instance.Quests.IndexOf(data);
		[SerializeField] private Text questInfoText;
		[SerializeField][ReadOnly] private QuestData data;
		#endregion fields & properties

		#region methods
		public void OnListUpdate(int param)
		{
			data = TavernItemList.Instance.Quests[param];
			BuyPrice = data.BuyPrice;
			SellPrice = data.WalletReward;

			questInfoText.text = data.GetEnemiesText();
			if (data.IsCompleted)
			{
				Color col = Color.black;
				ColorUtility.TryParseHtmlString("#ECC82E", out col);
				questInfoText.color = col;
				questInfoText.text = $"{LanguageLoader.GetTextByType(TextType.Adventure, 14)}\n";
			}
			questInfoText.text += $"\n<color=#{(data.KarmaReward > 0 ? "71C131" : "FF0000")}>{LanguageLoader.GetTextByType(TextType.GameMenu, 41)} {(data.KarmaReward > 0 ? "+" : "")}{data.KarmaReward}</color>";
			if (data.IsTaken)
			{

				DisableUI();
				return;
			}
		}
		protected override void Buy()
		{
			data.TakeQuest();
			base.Buy();
		}
		#endregion methods

	}
}