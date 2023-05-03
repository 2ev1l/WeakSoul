using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class StatScale
	{
		#region fields & properties
		/// <summary>
		/// <see cref="{T0}"/> turns;
		/// </summary>
		public UnityAction<int> OnTurnsChanged;
		public UnityAction<int> OnIncreasedValueChanged;
		public float Multiplier => multiplier;
		[SerializeField] private float multiplier;
		public PhysicalStatsType StatsType => type;
		[SerializeField] private PhysicalStatsType type;
		public ValueIncrease IncreaseType => increaseType;
		[SerializeField] private ValueIncrease increaseType = ValueIncrease.Multiply;
		public int Turns => turns;
		[Min(0)][SerializeField] private int turns = 1;
		public int IncreasedValue
		{
			get => increasedValue;
			set
			{
				increasedValue = value;
				OnIncreasedValueChanged?.Invoke(value);
			}
		}
		private int increasedValue = 0;
		#endregion fields & properties

		#region methods
		public void DecreaseTurns()
		{
			turns -= 1;
			turns = Mathf.Max(turns, 0);
			OnTurnsChanged?.Invoke(turns);
		}
		/// <summary>
		/// Calculate and set to field
		/// </summary>
		/// <returns><see cref="IncreasedValue"/></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public int SetIncreasedValue(PhysicalStats stats)
		{
			IncreasedValue = CalculateIncreasedValue(stats);
			return IncreasedValue;
		}
		public int CalculateIncreasedValue(PhysicalStats stats) => IncreaseType switch
		{
			ValueIncrease.Multiply => CustomMath.GetMultipliedIncrease(stats.GetStatsByType(StatsType), Multiplier),
			ValueIncrease.Additive => Mathf.RoundToInt(Multiplier),
			_ => throw new System.NotImplementedException("Value increase type")
		};
		public StatScale Clone() => new()
		{
			multiplier = multiplier,
			type = type,
			turns = turns,
			increasedValue = increasedValue,
			increaseType = increaseType,
		};
		#endregion methods
	}
}