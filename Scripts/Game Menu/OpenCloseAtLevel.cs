using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
    public class OpenCloseAtLevel : OpenAtLevel
    {
		#region fields & properties
		[Min(-1)][SerializeField] private int closesAtLevel = -1;
		#endregion fields & properties

		#region methods
		protected override void CheckOpen()
		{
			if (closesAtLevel != -1 && PlayerLevel >= closesAtLevel)
			{
				Obj.SetActive(false);
				return;
			}
			base.CheckOpen();
		}
		#endregion methods
	}
}