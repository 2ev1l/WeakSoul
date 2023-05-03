using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class CraftSet
	{
		#region fields & properties
		public CraftingThing Thing => thing;
		[SerializeField] private CraftingThing thing = CraftingThing.None;
		public int Id => id;
		[Min(0)][SerializeField] private int id;
		#endregion fields & properties

		#region methods
		public void SetValues(int id, CraftingThing thing)
		{
			this.thing = thing;
			this.id = id;
		}
		public CraftSet Clone() => new()
		{
			id = id,
			thing = thing,
		};
		#endregion methods
	}
}