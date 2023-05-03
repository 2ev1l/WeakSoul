using Data;
using Data.Adventure;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
	public class KarmaReward : SingleSceneInstance
	{
		#region fields & properties
		public static KarmaReward Instance { get; private set; }
		private static bool doReward;
		[SerializeField] private GameObject karmaPanel;
		[SerializeField] private LanguageLoader karmaLanguage;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			DisablePanel();
			Instance = this;
			CheckInstances(GetType());
			CheckReward();
		}
		private void CheckReward()
		{
			if (!doReward) return;
			doReward = false;
			if (CustomMath.GetRandomChance(50)) return;
			PlayerStats playerStats = GameData.Data.PlayerData.Stats;
			if ((playerStats.Karma <= -50 || playerStats.Karma >= 50) && playerStats.ExperienceLevel.Level > 14)
			{
				GainReward(out bool isRewardNotNull);
				if (isRewardNotNull)
					EnablePanel();
			}
		}
		private void GainReward(out bool isRewardNotNull)
		{
			isRewardNotNull = false;
			int karma = GameData.Data.PlayerData.Stats.Karma;
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			ChestTier tier = karma switch
			{
				int i when i <= -150 => ChestTier.Terrible,
				int i when i <= -50 => ChestTier.Bad,
				int i when i <= 100 => ChestTier.Normal,
				int i when i <= 200 => ChestTier.Good,
				_ => ChestTier.Amazing,
			};
			int level = karma switch
			{
				int i when i <= -150 => playerLevel - 1,
				int i when i <= -50 => playerLevel - 2,
				int i when i <= 100 => playerLevel - 3,
				int i when i <= 200 => playerLevel - 2,
				_ => playerLevel - Random.Range(2, 7),
			};
			level = Mathf.Max(playerLevel, 0);
			int maxLoot = karma switch
			{
				int i when i <= -150 => 2,
				int i when i <= -50 => 1,
				int i when i <= 100 => 1,
				int i when i <= 200 => 1,
				_ => 1,
			};
			ChestData chestData = new(0, level, tier, 0, maxLoot);
			List<RewardData> rewards = chestData.GetReward();
			foreach (var el in rewards)
			{
				if (el.TryAddReward(out RewardData reward))
				{
					isRewardNotNull = true;
				}
			}
		}
		private void EnablePanel()
		{
			karmaPanel.SetActive(true);
			karmaLanguage.AddText($" {GameData.Data.PlayerData.Stats.Karma}");
			if (ColorUtility.TryParseHtmlString(GameData.Data.PlayerData.Stats.Karma >= 0 ? "#58C094" : "#AB403F", out Color newCol))
				karmaLanguage.Text.color = newCol;
		}
		public void DisablePanel()
		{
			karmaPanel.SetActive(false);
		}
		public static void DoReward() => doReward = true;

		[ContextMenu("Add karma")]
		private void SetKarma() => GameData.Data.PlayerData.Stats.ChangeKarmaBy(karmaToAdd);
		[SerializeField] private int karmaToAdd;
		#endregion methods
	}
}