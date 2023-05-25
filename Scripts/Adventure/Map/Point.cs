using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Adventure.Map
{
	public class Point : MonoBehaviour
	{
		#region fields & properties
		public SpriteRenderer SpriteRenderer => spriteRenderer;
		[SerializeField] private SpriteRenderer spriteRenderer;

		[SerializeField][ReadOnly] private List<ZoneData> capturedZones = new();
		public Point Parent
		{
			get => parent;
			set
			{
				parent = value;
				Data.DirectionsInfo.SetParentDirections(this);
			}
		}
		[SerializeField][ReadOnly] private Point parent;
		public PointData Data
		{
			get
			{
				if (!isInitialized)
				{
					isInitialized = true;
					data = new(this);
					PointsInit.Instance.AddPoint(this);
				}
				return data;
			}
		}
		[SerializeField][ReadOnly] protected PointData data;
		protected bool isInitialized = false;
		#endregion fields & properties

		#region methods
		protected virtual void Start()
		{
			StartCoroutine(StartGenerating());
		}
		private IEnumerator StartGenerating()
		{
			while (capturedZones.Count == 0)
				yield return CustomMath.WaitAFrame();
			GenerateZoneData();
			GenerateEventData();
			GenerateDirections();
			RandomizeOpen();
			PointsInit.Instance.AddPointData(this);
		}
		private void GenerateZoneData()
		{
			foreach (var el in capturedZones)
			{
				switch (el.SpawnZone)
				{
					case SpawnZone.Any:
						if (Data.ChoosedZone != null) continue;
						Data.SetZone(el);
						break;
					case SpawnZone.Mountains:
						Data.SetZone(el);
						break;
					case SpawnZone.Boss:
						if (PointsInit.Instance.TryGenerateBoss(el.Value))
						{
							Data.SetZone(el);
							return;
						}
						break;
					case SpawnZone.Water:
						Data.SetZone(el);
						return;
					case SpawnZone.City:
						if (PointsInit.Instance.TryGenerateCity(el.Value))
						{
							Data.SetZone(el);
							return;
						}
						break;
					case SpawnZone.Final:
						if (PointsInit.Instance.TryGenerateFinal())
						{
							Data.SetZone(el);
							return;
						}
						break;
					default: throw new System.NotImplementedException("Spawn Zone");
				}
			}
			if (Data.ChoosedZone == null) throw new System.NotImplementedException("Choosed zone is null");
		}
		private void GenerateEventData()
		{
			List<MapEvent> mapEvents = MapEventsInfo.Instance.GetEvents(Data.ChoosedZone.SpawnZone);
			if (Data.ChoosedZone.SpawnZone == SpawnZone.Water || Data.ChoosedZone.SpawnZone == SpawnZone.Boss)
			{
				Data.SetEventHidden(mapEvents.First());
				return;
			}
			if (data.PointId == 0)
			{
				Data.SetEventHidden(MapEventsInfo.Instance.GetEvent(0));
				return;
			}
			int choosed = GetChoosedEvent(mapEvents);
			if (choosed == -1)
			{
				List<MapEvent> anyEvents = MapEventsInfo.Instance.GetEvents(SpawnZone.Any);
				choosed = GetChoosedEvent(anyEvents);
				if (choosed == -1)
					choosed = 0;
				Data.SetEventHidden(anyEvents[choosed]);
				return;
			}

			Data.SetEventHidden(mapEvents[choosed]);
		}
		private void GenerateDirections()
		{
			if (Data.ChoosedZone.SpawnZone == SpawnZone.Water) return;

			List<Direction> filledDirs = Data.DirectionsInfo.GetFilledDirections();

			List<Point> childs = new();
			List<Direction> freeDirs = Data.DirectionsInfo.GetFreeDirections();
			foreach (Direction el in freeDirs)
			{
				Point child = PointsInit.Instance.InstantiatePoint(Data.DirectionsInfo.GetPositionByDirection(el, transform));
				Data.DirectionsInfo.SetDirection(el, child);
				childs.Add(child);
			}
			childs.ForEach(x => x.Parent = this);
			foreach (Direction el in filledDirs)
			{
				DirectionInfo di = Data.DirectionsInfo.GetDirection(el);
				di.Point.Parent = this;
			}
		}
		private void RandomizeOpen()
		{
			if (data.ChoosedEvent.Id != 19 && data.ChoosedEvent.Id != 9 &&
				CustomMath.GetRandomChance(20) && !PointsInit.IsSoulItem_AllPointsOpen) return;
			Data.Open();
		}
		protected virtual void GenerateUI()
		{
			spriteRenderer.sprite = (Data.IsOpened || PointsInit.IsSoulItem_AllPointsOpen) ? Data.ChoosedEvent.Texture : PointsInit.Instance.IconUnknown;
		}
		private int GetChoosedEvent(List<MapEvent> events)
		{
			float totalChance = 0;
			for (int i = events.Count - 1; i >= 0; --i)
				totalChance += events[i].Probability;
			float[] chances = GetChancesFromEvents(events);
			int choosed = CustomMath.GetRandomFromChancesArray(chances);
			return choosed;
		}
		private float[] GetChancesFromEvents(List<MapEvent> events)
		{
			float[] chances = new float[events.Count];
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			bool isPlayerLevelSmall = playerLevel <= 8;
			bool isPlayerLevelHigh = playerLevel > 30;
			bool isPlayerLevelHigh2 = playerLevel > 35;
			bool isPlayerLevelHigh3 = playerLevel > 45;
			bool isBossNotDefeatedWhenMust = PointsInit.IsBossMustBeBeaten;
			for (int i = 0; i < events.Count; ++i)
			{
				MapEvent @event = events[i];
				float chance = @event.Probability;
				if (@event.Id == 0) //none
				{
					if (isPlayerLevelSmall)
					{
						chances[i] = chance / 2f;
						continue;
					}
					if (isPlayerLevelHigh)
					{
						chances[i] = chance / 1.1f;
						continue;
					}
				}

				if (@event.Id == 1) //fight
				{
					if (isPlayerLevelSmall)
					{
						chances[i] = chance * 2.5f;
						continue;
					}
					if (isBossNotDefeatedWhenMust)
					{
						chances[i] = chance / 10f;
						continue;
					}
					if (isPlayerLevelHigh)
					{
						chances[i] = chance / 2f;
						continue;
					}
				}

				if (@event.Id == 2) //dungeon
				{
					if (isBossNotDefeatedWhenMust)
					{
						chances[i] = chance / 2f;
						continue;
					}
					if (isPlayerLevelHigh)
					{
						chances[i] = chance * 3f;
						continue;
					}
				}

				if (@event.Id == 3) //shop
				{
					if (isPlayerLevelHigh2)
					{
						chances[i] = chance * (isPlayerLevelHigh3 ? 2f : 4f);
						continue;
					}
				}

				if (@event.Id == 6) //blacksmith
				{
					if (isPlayerLevelHigh2)
					{
						chances[i] = chance * (isPlayerLevelHigh3 ? 2f : 1.5f);
						continue;
					}
				}

				if (@event.Id == 10) //mountain fight
				{
					if (isBossNotDefeatedWhenMust)
					{
						chances[i] = chance / 5f;
						continue;
					}
				}

				chances[i] = chance;
			}
			return chances;
		}
		protected virtual void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent(out Zone zone))
			{
				capturedZones.Add(zone.Data);
			}
		}
		#endregion methods
	}
}