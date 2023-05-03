using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.City.Tavern
{
	public class TavernItemList : ItemList
	{
		#region fields & properties
		public static TavernItemList Instance { get; private set; }
		public List<QuestData> Quests
		{
			get => quests;
			set
			{
				quests = value;
				UpdateListData();
			}
		}
		[SerializeField] private List<QuestData> quests;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			base.Awake();
			Instance = this;
		}
		public override void UpdateListData()
		{
			Instance = this;
			Clear();
			UpdateListDefault(quests, x => quests.IndexOf(x));
		}
		#endregion methods
	}
}