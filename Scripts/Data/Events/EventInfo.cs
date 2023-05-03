using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using WeakSoul.Adventure.Map;

namespace Data.Events
{
	public class EventInfo : MonoBehaviour
	{
		#region fields & properties
		public static EventInfo Instance { get; private set; }
		public EventData Data => data;
		[SerializeField] private EventData data = new();
		#endregion fields & properties

		#region methods
		public void Init()
		{
			Instance = this;
		}
		private void OnEnable()
		{
			Player.OnSubZoneChanged += data.SetSubZone;
		}
		private void OnDisable()
		{
			Player.OnSubZoneChanged -= data.SetSubZone;
		}
		[ContextMenu("Generate shop data")]
		private void GenerateShopData() => data.LoadEvent(3);
		[ContextMenu("Generate enemies")]
		private void GenerateEnemies() => data.GenerateBattleData();
		[ContextMenu("Load default fight")]
		private void LoadDefaultFight() => data.LoadEvent(1);
		[ContextMenu("Load blacksmith")]
		private void LoadBlacksmith() => data.LoadEvent(6);
		[ContextMenu("Load 0 boss")]
		private void Load0Boss()
		{
			data.SetSubZone(new(SpawnSubZone.Hunger));
			data.LoadEvent(9);
		}
		[ContextMenu("Load 1 boss")]
		private void Load1Boss()
		{
			data.SetSubZone(new(SpawnSubZone.Poverty));
			data.LoadEvent(9);
		}
		#endregion methods
	}
}