using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class SoulItem : Item
    {
        #region fields & properties
        public bool CanUse => canUse;
        [SerializeField] private bool canUse;
        public float BreakChance => breakChance;
        [Range(0, 100)][SerializeField] private float breakChance;
		#endregion fields & properties

		#region methods
		protected override int DivideSellPrice(float value, float divideScale)
		{
			divideScale *= 1.3f;
			divideScale = Mathf.Max(divideScale, 1f);
			return base.DivideSellPrice(value, divideScale);
		}
		#endregion methods
	}
}