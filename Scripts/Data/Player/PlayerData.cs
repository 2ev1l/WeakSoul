using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
	[System.Serializable]
	public class PlayerData
	{
		#region fields & properties
		public Wallet Wallet => wallet;
		[SerializeField] private Wallet wallet = new Wallet();
		public PlayerStats Stats => stats;
		[SerializeField] private PlayerStats stats = new PlayerStats(false);

		public SkillsInventory Skills => skills;
		[SerializeField] private SkillsInventory skills = new SkillsInventory();
		public ItemsInventory Inventory => inventory;
		[SerializeField] private ItemsInventory inventory = new ItemsInventory();
		#endregion fields & properties

		#region methods
		public PlayerData() { }
		#endregion methods
	}
}