using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class SkillsInventory : InventoryData
	{
		#region fields & properties
		[SerializeField] private List<int> openedSkills = new();
		public IEnumerable<int> TempOpenedSkills => tempOpenedSkills;
		[SerializeField] private List<int> tempOpenedSkills = new();
		#endregion fields & properties

		#region methods
		public bool IsSkillOpened(int skillId) => openedSkills.Contains(skillId) || IsSkillTempOpened(skillId);
		public bool IsSkillTempOpened(int skillId) => tempOpenedSkills.Contains(skillId);
		public bool TryAddOpenedSkill(int skillId)
		{
			if (IsSkillOpened(skillId)) return false;
			openedSkills.Add(skillId);
			return true;
		}
		public bool TryAddTempOpenedSkill(int skillId, bool force = false)
		{
			if (IsSkillOpened(skillId) && !force) return false;
			tempOpenedSkills.Add(skillId);
			return true;
		}
		public bool TryRemoveOpenedSkill(int skillId)
		{
			if (openedSkills.Contains(skillId)) return false;
			openedSkills.Remove(skillId);
			return true;
		}
		public void RemoveAllTempOpenedSkills()
		{
			while (tempOpenedSkills.Count > 0)
			{
				int skillId = tempOpenedSkills.First();
				tempOpenedSkills.RemoveAt(0);
				TryRemoveSkill(skillId);
			}
		}
		public bool TryRemoveTempOpenedSkill(int skillId)
		{
			if (openedSkills.Contains(skillId)) return false;
			tempOpenedSkills.Remove(skillId);
			return TryRemoveSkill(skillId);
		}
		public bool TryRemoveSkill(int skillId)
		{
			int index = items.IndexOf(skillId);
			if (index == -1 || IsSkillOpened(skillId)) return false;
			RemoveItem(index);
			return true;
		}
		public void ResetItems() => items = new List<int>()
			{
				-1, -1, -1, -1, -1, -1,
				-1 //remove
            };
		public SkillsInventory()
		{
			ResetItems();
			Size = 2;
			openedSkills = new List<int>();
		}
		public SkillsInventory Clone()
		{
			SkillsInventory skills = new();
			List<int> openedSkills = new();
			List<int> tempOpenedSkills = new();
			List<int> items = new();
			this.openedSkills.ForEach(x => openedSkills.Add(x));
			this.tempOpenedSkills.ForEach(x => tempOpenedSkills.Add(x));
			this.items.ForEach(x => items.Add(x));
			skills.Size = Size;
			skills.openedSkills = openedSkills;
			skills.tempOpenedSkills = tempOpenedSkills;
			skills.items = items;
			return skills;
		}
		#endregion methods
	}
}