using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.GameMenu;

namespace WeakSoul.GameMenu.Shop
{
    public class ShopIconOpenClose : OpenCloseAtLevel
{
		#region fields & properties
		[Min(-1)][SerializeField] private int secondOpen = -1;
		#endregion fields & properties

		#region methods
		protected override void CheckOpen()
		{
			if (PlayerLevel >= secondOpen)
			{
				Obj.SetActive(true);
				return;
			}
			base.CheckOpen();
		}
		#endregion methods
	}
}