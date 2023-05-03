using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Skills;

namespace Data.Events
{
	[System.Serializable]
	public class FightBuffes
	{
		#region fields & properties
		public IEnumerable<SkillBuff> AppliedBuffes => appliedBuffes;
		[SerializeField][ReadOnly] private List<SkillBuff> appliedBuffes = new();
		[SerializeField][ReadOnly] private EntityStats stats;
		[SerializeField][ReadOnly] private bool isBuffesAllowed = true;
		#endregion fields & properties

		#region methods
		public void Init(EntityStats stats)
		{
			this.stats = stats;
			isBuffesAllowed = true;
		}
		public void DecreaseBuffes()
		{
			for (int i = appliedBuffes.Count - 1; i >= 0; --i)
			{
				SkillBuff el = null;
				try { el = appliedBuffes[i]; }
				catch { continue; }
				DecreaseSkillBuffTurns(el);
			}
		}
		private void DecreaseSkillBuffTurns(SkillBuff skillBuff)
		{
			if (skillBuff.Turns <= 0)
			{
				RemoveBuff(skillBuff);
				return;
			}
			skillBuff.DecreaseTurns();

			int c = 0;
			while (c < skillBuff.StatsScale.Count())
			{
				StatScale el = skillBuff.GetStatScale(c);
				if (el.Turns <= 0)
				{
					stats.DecreaseStatsByType(el.StatsType, el.IncreasedValue);
					skillBuff.RemoveStatScale(el);
					continue;
				}
				el.DecreaseTurns();
				c++;
			}
		}
		public bool TryAddBuff(SkillBuff skillBuff)
		{
			if (!isBuffesAllowed) return false;
			AddBuff(skillBuff);
			return true;
		}
		private void AddBuff(SkillBuff skillBuff)
		{
			appliedBuffes.Add(skillBuff);
			foreach (StatScale el in skillBuff.StatsScale)
			{
				el.SetIncreasedValue(stats);
				stats.IncreaseStatsByType(el.StatsType, el.IncreasedValue);
			}
		}
		public void RemoveBuff(SkillBuff skillBuff)
		{
			foreach (StatScale el in skillBuff.StatsScale)
				stats.DecreaseStatsByType(el.StatsType, el.IncreasedValue);
			appliedBuffes.Remove(skillBuff);
		}
		public bool TryRemoveBuffScale(SkillBuff skillBuff, StatScale statScale)
		{
			try
			{
				RemoveBuffScale(skillBuff, statScale);
				return true;
			}
			catch { return false; }
		}
		public void RemoveBuffScale(SkillBuff skillBuff, StatScale statScale)
		{
			SkillBuff buff = appliedBuffes.Find(x => x == skillBuff);
			buff.RemoveStatScale(statScale);
			stats.DecreaseStatsByType(statScale.StatsType, statScale.IncreasedValue);
		}
		public void CloseBuffes()
		{
			isBuffesAllowed = false;
			RemoveAll();
		}
		private void RemoveAll()
		{
			while (appliedBuffes.Count > 0)
				RemoveBuff(appliedBuffes[0]);
		}
		public bool IsActivatorApplied(Predicate<SkillBuff> predicate) => appliedBuffes.Exists(predicate);
		#endregion methods
	}
}