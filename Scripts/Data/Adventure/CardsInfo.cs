using Data.Adventure;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
	public class CardsInfo : SingleSceneInstance
	{
		#region fields & properties
		public static CardsInfo Instance { get; private set; }
		public IEnumerable<CardGroupSO> CardGroups => cardGroups;
		[SerializeField] private List<CardGroupSO> cardGroups;
		public IEnumerable<CardSO> Cards => cards;
		[SerializeField] private List<CardSO> cards;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		public CardGroup GetGroup(int groupId) => cardGroups[groupId].CardGroup;
		public CardData GetCard(int cardId) => cards[cardId].CardData;

		[ContextMenu("Get all")]
		private void GetAll()
		{
			cardGroups = new();
			cards = new();
			cardGroups = Resources.FindObjectsOfTypeAll<CardGroupSO>().OrderBy(x => x.CardGroup.Id).ToList();
			cards = Resources.FindObjectsOfTypeAll<CardSO>().OrderBy(x => x.CardData.Id).ToList();

			foreach (var el in cardGroups)
				if (cardGroups.Where(x => x.CardGroup.Id == el.CardGroup.Id).Count() > 1)
					Debug.LogError($"Error id {el.CardGroup.Id} at {el.name}");
			foreach (var el in cards)
			{
				if (cards.Where(x => x.CardData.Id == el.CardData.Id).Count() > 1)
					Debug.LogError($"Error id {el.CardData.Id} at {el.name}");
				if (el.CardData.EventChance.Chance > 0 && el.CardData.EventChance.Id == 0)
					Debug.LogError($"Error event chance {el.CardData.EventChance.Chance} & id {el.CardData.EventChance.Id} at {el.name}");
				if (el.CardData.EventChance.Chance < 0.01f && el.CardData.EventChance.Id > 0)
					Debug.LogError($"Error event chance {el.CardData.EventChance.Chance} & id {el.CardData.EventChance.Id} at {el.name}");

				if (el.CardData.CardGroupsChance.Find(x => x.Chance < 0.01f && x.Id > 0) != null)
					Debug.LogError($"Error card group chance {el.CardData.EventChance.Chance} & id {el.CardData.EventChance.Id} at {el.name}");

				foreach (var reward in el.CardData.Rewards)
				{
					if (reward.Count == 0)
						Debug.LogError($"Error reward count at {el.name}");
					if (reward.Chance < 0.01f)
						Debug.LogError($"Error reward chance at {el.name}");
					if (TextData.Instance != null)
					{
						try { LanguageLoader.GetRewardTextByType(reward.Type, reward.Id); }
						catch { Debug.LogError($"Error text in {el.name} - {reward.Type} x {reward.Id}"); }
					}
				}
			}
		}
#if false
        [ContextMenu("Create")]
        private void CreateAll()
        {
            string path = "Assets/Resources/Scriptable Object/Cards/Card ";
            for (int i = 481; i < 607; ++i)
            {
                CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();
                cardSO.CardData.ChangeID(i);
                string newPath = $"{path}{i}.asset";
				AssetDatabase.CreateAsset(cardSO, newPath);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		#endregion methods
	}
}