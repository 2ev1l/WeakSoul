using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Camp
{
	public class EventInit : SingleSceneInstance
	{
		#region fields & properties
		public static EventInit Instance { get; private set; }
		[SerializeField] private EventStorage storage;
		[SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
		[SerializeField] private Camp camp;
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
			camp.Init(RandomizeEvent());
		}
		private CampEvent RandomizeEvent()
		{
			List<CampEvent> campEvents = new();
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			foreach (var el in CampInfo.Instance.CampData)
			{
				CampEvent elce = el.CampEvent;
				if (elce.Level <= playerLevel)
					campEvents.Add(elce);
			}
			List<CampEvent> bigEvents = campEvents.Where(x => x.StatsScale.Count() > 1).ToList();
			List<CampEvent> smallEvents = campEvents.Where(x => x.StatsScale.Count() == 0).ToList();
			if (smallEvents.Count == 0)
				smallEvents = campEvents;
			if (bigEvents.Count == 0)
				bigEvents = campEvents;
			CampEvent choosedEvent = (EventInfo.Instance.Data.Event.Id) switch
			{
				8 => GetRandomEvent(campEvents),
				14 => GetRandomEvent(bigEvents),
				15 => GetRandomEvent(smallEvents),
				_ => throw new System.NotImplementedException("Map event id for Camp Data")
			};

			return choosedEvent;
		}
		private CampEvent GetRandomEvent(List<CampEvent> campEvents) => campEvents[Random.Range(0, campEvents.Count)].Clone();
		#endregion methods
	}
}