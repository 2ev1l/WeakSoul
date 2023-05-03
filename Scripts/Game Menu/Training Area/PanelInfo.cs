using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace WeakSoul.GameMenu.TrainingArea
{
	public class PanelInfo : MonoBehaviour
	{
		#region fields & properties
		[SerializeField] private int expToAdd;
		[field: SerializeField] public PhysicalStatsType Type { get; private set; }
		public StatExperienceLevel PlayerStatLevel
		{
			get
			{
				if (playerStatLevel == null)
					Awake();
				return playerStatLevel;
			}
		}
		private StatExperienceLevel playerStatLevel;
		public TrainingLevel CurrentLevelData
		{
			get
			{
				if (currentLevelData == null)
					CheckStats();
				return currentLevelData;
			}
		}
		private TrainingLevel currentLevelData;
		#endregion fields & properties

		#region methods
		private void Awake()
		{
			playerStatLevel = GameData.Data.PlayerData.Stats.TrainingData.GetLevelByType(Type);
		}
		private void OnEnable()
		{
			PlayerStatLevel.OnLevelChanged += CheckStats;
			CheckStats();
		}
		private void OnDisable()
		{
			PlayerStatLevel.OnLevelChanged -= CheckStats;
		}
		private void CheckStats(int level) => CheckStats();
		private void CheckStats()
		{
			currentLevelData = LevelsInfo.Instance.TrainingLevels.GetCurrentLevelByType(Type);
			if (currentLevelData == null && PlayerStatLevel.Level >= PlayerStatLevel.MaxLevel)
				currentLevelData = LevelsInfo.Instance.TrainingLevels.GetLastLevelByType(Type);
		}
		[ContextMenu("Reset levels")]
		private void RL()
		{
			playerStatLevel.Level = 0;
		}
		[ContextMenu("Complete all levels to current player level")]
		private void CALTPL()
		{
			int plevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			int plevelstat = playerStatLevel.Level;
			foreach (var el in LevelsInfo.Instance.TrainingLevels.GetLevelsByType(Type).Where(x => x.Id <= plevel + 1 && x.Id > plevelstat))
			{
				playerStatLevel.Level = el.Id;
				StatsReward reward = currentLevelData?.Reward;
				if (reward != null)
					GameData.Data.PlayerData.Stats.IncreaseStatsHidden(reward.Stats);
			}
		}
		[ContextMenu("Add exp to lvl")]
		private void ADDEXP() => PlayerStatLevel.Experience += expToAdd;
		#endregion methods
	}
}