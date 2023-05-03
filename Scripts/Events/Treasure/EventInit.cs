using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Treasure
{
	public class EventInit : SingleSceneInstance
	{
		#region fields & properties
		public static EventInit Instance { get; private set; }
		[SerializeField] private EventStorage storage;
		[SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
		[SerializeField] private Chest chest;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		private void Start()
		{
			Init();
		}
		private void Init()
		{
			Sprite rnd = storage.GetRandomSprite();
			bgSpriteRenderers.ForEach(x => x.sprite = rnd);
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			List<ChestDataSO> allChests = ChestsInfo.Instance.ChestsData.Where(x => x.ChestData.Level <= playerLevel).ToList();
			List<ChestDataSO> allowedChests = allChests.Where(x => x.ChestData.Level > playerLevel - 5).ToList();
			if (allowedChests.Count == 0)
				allowedChests = allChests;
			List<ChestDataSO> smallChests = allowedChests.Where(x => x.ChestData.Tier == ChestTier.Normal).ToList();
			if (smallChests.Count == 0)
				smallChests = allowedChests;
			ChestData choosed = EventInfo.Instance.Data.Event.Id switch
			{
				5 => GetRandomData(allowedChests),
				18 => GetRandomData(smallChests),
				_ => throw new System.NotImplementedException("Event id for chest data")
			};
			chest.Init(choosed);
		}
		private ChestData GetRandomData(List<ChestDataSO> data) => data[Random.Range(0, data.Count)].ChestData.Clone();
		#endregion methods
	}
}