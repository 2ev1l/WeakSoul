using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.MainMenu;

namespace WeakSoul.City
{
	public class ZoneState : StateChange
	{
		#region fields & properties
		[SerializeField] private GameObject panel;
		public SpawnSubZone SubZone => subZone;
		[SerializeField] private SpawnSubZone subZone;
		#endregion fields & properties

		#region methods
		public override void SetActive(bool active)
		{
			panel.SetActive(active);
		}
		#endregion methods
	}
}