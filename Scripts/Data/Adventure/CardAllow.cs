using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Data.Adventure
{
	[System.Serializable]
	public class CardAllow
	{
		#region fields & properties
		[SerializeField] private bool useKarma = false;
		[SerializeField] private LogicalOperation logic;
		[SerializeField] private int karma;

		[SerializeField] private List<int> inventoryItems = new();
		#endregion fields & properties

		#region methods
		public bool IsCardAllowed(PlayerData playerData)
		{
			bool karmaAllow = true;
			if (useKarma)
				karmaAllow = CustomMath.GetLogicalResult(playerData.Stats.Karma, logic, karma);
			if (!karmaAllow) return false;

			bool inventoryAllow = true;
			foreach (var el in inventoryItems)
			{
				if (!playerData.Inventory.ContainItem(el))
				{
					inventoryAllow = false;
				}
				else
				{
					inventoryAllow = true;
					break;
				}
			}
			if (!inventoryAllow) return false;
			return true;
		}
		#endregion methods
	}
}
