using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
	[System.Serializable]
	public class StatsItem : Item
	{
		#region fields & properties
		public UnityAction<int> OnSkillIdChanged;
		public UnityAction<int> OnRareChanged;
		public int SkillId
		{
			get => skillId;
			set => SetSkillId(value);
		}
		[Min(-1)][SerializeField] private int skillId = -1;
		public int Rare
		{
			get => rare;
			set => SetRare(value);
		}
		[Range(0, 4)][SerializeField] private int rare = 0;

		public PhysicalStats Stats => stats;
		[SerializeField] private PhysicalStats stats = new();
		#endregion fields & properties

		#region methods
		private void SetSkillId(int value)
		{
			if (value < -1)
				throw new System.ArgumentOutOfRangeException("Skill Id");
			skillId = value;
			OnSkillIdChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		private void SetRare(int value)
		{
			if (value < 0)
				throw new System.ArgumentOutOfRangeException("Rare");
			rare = value;
			OnRareChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		protected override int DivideSellPrice(float value, float divideScale)
		{
			if (rare < 4)
				divideScale += (rare / 2f);
			else
				divideScale = 1.3f;
			divideScale = Mathf.Max(divideScale, 1f);
			return base.DivideSellPrice(value, divideScale);
		}
		public StatsItem()
		{
			stats.Health = 0;
			stats.Damage = 0;
			stats.Stamina = 0;
			stats.StaminaRegen = 0;
			stats.CriticalScale = 0;
		}
		#endregion methods
	}
}