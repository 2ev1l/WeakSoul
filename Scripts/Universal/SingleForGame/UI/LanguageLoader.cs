using Data;
using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Universal
{
	public class LanguageLoader : MonoBehaviour
	{
		#region fields & properties
		public Text Text
		{
			get
			{
				txt = txt == null ? GetComponent<Text>() : txt;
				return txt;
			}
		}
		private Text txt;
		public int Id
		{
			get => id;
			set => SetId(value);
		}
		[SerializeField] private int id;
		public TextType TextType
		{
			get => textType;
			set => SetTextType(value);
		}
		[SerializeField] private TextType textType;
		private string additionalText;
		#endregion fields & properties

		#region methods
		public void Start()
		{
			Load();
		}
		public void Load()
		{
			if (id == -1)
			{
				Text.text = "";
				return;
			}
			string text = GetTextByType(textType, id);
			TryReplaceText(text, textType);

			Text.text = text + additionalText;
			if (TryGetComponent(out TextOutline textOutline))
				textOutline.SetAll();
		}
		/// <summary>
		/// Old added text will be deleted
		/// </summary>
		/// <param name="text"></param>
		public void AddText(string text)
		{
			additionalText = "";
			additionalText = text;
			Load();
		}
		/// <summary>
		/// Removes color with tags <see cref="{}"/>
		/// </summary>
		public void RemoveTextXML()
		{
			string text = Text.text;
			int startIndex = text.IndexOf("<");
			int endIndex = text.IndexOf(">");
			while (startIndex > -1)
			{
				text = text.Remove(startIndex, endIndex - startIndex + 1);
				startIndex = text.IndexOf("<");
				endIndex = text.IndexOf(">");
			}
			Text.text = text;
		}
		private static void TryReplaceText(string text, TextType textType)
		{
			if (text.Length > 0 && text[..1].Equals("="))
			{
				try
				{
					int id = System.Convert.ToInt32(text[1..text.Length]);
					text = GetTextByType(textType, id);
				}
				catch { }
			}
		}
		public static string GetTextByType(TextType textType, int id)
		{
			string text = GetText(textType, id);
			TryReplaceText(text, textType);
			return text;
		}
		private static string GetText(TextType textType, int id)
		{
			LanguageData data = TextData.Instance.LoadedData;
			return (textType) switch
			{
				TextType.MainMenu => data.MenuData[id],
				TextType.Help => data.HelpData[id],
				TextType.CutScene => data.CutSceneData[id],
				TextType.GameMenu => data.GameMenuData[id],
				TextType.Adventure => data.AdventureData[id],
				TextType.Event => data.EventsData[id],
				TextType.Tutorial => data.TutorialData[id],

				TextType.ItemsName => data.ItemsData[id].Name,
				TextType.ItemsDescription => data.ItemsData[id].Description,
				TextType.SkillsName => data.SkillsData[id].Name,
				TextType.SkillsDescription => data.SkillsData[id].Description,
				TextType.EffectsName => data.EffectsData[id].Name,
				TextType.EffectsDescription => data.EffectsData[id].Description,

				TextType.CardGroupName => data.CardGroupsData[id].Name,
				TextType.CardGroupDescription => data.CardGroupsData[id].Description,
				TextType.CardName => data.CardsData[id].Name,
				TextType.CardDescription => data.CardsData[id].Description,
				TextType.EnemyName => data.EnemiesData[id].Name,
				TextType.EnemyDescription => data.EnemiesData[id].Description,
				_ => throw new System.NotImplementedException("Text Type"),
			};
		}
		public static string GetRewardTextByType(RewardType type, int id) => (type) switch
		{
			RewardType.Item => LanguageLoader.GetTextByType(TextType.ItemsName, id),
			RewardType.Effect => LanguageLoader.GetTextByType(TextType.EffectsName, id),
			RewardType.Soul => LanguageLoader.GetSoulTextByType((SoulType)id),
			RewardType.Random => "??",
			_ => throw new System.NotImplementedException()
		};
		public static string GetSoulTextByType(SoulType soulType) => soulType switch
		{
			SoulType.Weak => GetTextByType(TextType.Adventure, 6),
			SoulType.Normal => GetTextByType(TextType.Adventure, 7),
			SoulType.Strong => GetTextByType(TextType.Adventure, 8),
			SoulType.Unique => GetTextByType(TextType.Adventure, 9),
			SoulType.Legendary => GetTextByType(TextType.Adventure, 10),
			_ => throw new System.NotImplementedException("Soul Type Text")
		};
		public static int GetPlayerClassTextIdByType(PlayerClass playerClass) => playerClass switch
		{
			PlayerClass.Omnivorous => 47,
			PlayerClass.Impartial => 48,
			PlayerClass.HaterOfEvil => 49,
			PlayerClass.Stoic => 50,
			PlayerClass.PosthumousHero => 51,
			_ => throw new System.NotImplementedException("Player Class Text")
		};
		public static int GetPlayerClassDescriptionIdByType(PlayerClass playerClass) => playerClass switch
		{
			PlayerClass.Omnivorous => 53,
			PlayerClass.Impartial => 54,
			PlayerClass.HaterOfEvil => 55,
			PlayerClass.Stoic => 56,
			PlayerClass.PosthumousHero => 57,
			_ => throw new System.NotImplementedException("Player Class Description")
		};
		private void SetId(int value)
		{
			if (id < -1)
				throw new System.ArgumentOutOfRangeException("id");
			if (id == value) return;
			id = value;
			Load();
		}
		private void SetTextType(TextType value)
		{
			if (textType == value) return;
			textType = value;
			Load();
		}
		public void ChangeValues(int id, TextType textType)
		{
			this.textType = textType;
			SetId(id);
		}
		#endregion methods
	}
}