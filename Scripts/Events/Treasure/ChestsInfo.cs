using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Treasure
{
	public class ChestsInfo : SingleSceneInstance
	{
		#region fields & properties
		public static ChestsInfo Instance { get; private set; }
		public IEnumerable<ChestDataSO> ChestsData => chestsData;
		[SerializeField] private List<ChestDataSO> chestsData;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		public ChestData GetChest(int chestId) => chestsData[chestId].ChestData;

		[ContextMenu("Get all")]
		private void GetAll()
		{
			chestsData = new();
			chestsData = Resources.FindObjectsOfTypeAll<ChestDataSO>().OrderBy(x => x.ChestData.Id).ToList();
			foreach (var el in chestsData)
			{
				if (chestsData.Where(x => x.ChestData.Id == el.ChestData.Id).Count() > 2)
				{
					Debug.LogError($"Error {el.ChestData.Id} Id at {el.name}");
				}
			}
		}

		#endregion methods
	}
}