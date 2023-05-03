using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class SceneInit : MonoBehaviour
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private void Awake()
        {
            GameData.CanSaveData = true;
        }
        [ContextMenu("Fight tutorial off")]
        private void FTO() => GameData.Data.TutorialData.IsFightCompleted = true;
        [ContextMenu("Tutorial off")]
        private void TTO() => GameData.Data.TutorialData.IsCompleted = true;
        [ContextMenu("Save")]
        private void Save() => SavingUtils.SaveGameData();
        [ContextMenu("Set souls default")]
        private void SSD()
        {
            Wallet w = GameData.Data.PlayerData.Wallet;
            w.WeakSouls = 7;
            w.NormalSouls = 4;
            w.UniqueSouls = 1;
            w.StrongSouls = 0;
            w.LegendarySouls = 0;
        }
		[ContextMenu("Set souls INF")]
		private void SSI()
		{
			Wallet w = GameData.Data.PlayerData.Wallet;
			w.WeakSouls = 99;
			w.NormalSouls = 99;
			w.UniqueSouls = 99;
			w.StrongSouls = 99;
			w.LegendarySouls = 99;
		}
		[ContextMenu("Fpr video")]
		private void FV()
		{
			PlayerData playerData = GameData.Data.PlayerData;
            playerData.Stats.SoulLife = 11;
            playerData.Stats.SkillPoints = 10;
            playerData.Stats.Stamina = 0;
            playerData.Stats.StaminaRegen = 0;
		}
        [ContextMenu("Load 1 cut scene")]
        private void L1CS() => SceneLoader.Instance.LoadCutSceneFade(0, 1);

        [ContextMenu("Defeat boss")]
        private void DefeatBoss() => GameData.Data.AdventureData.TryAddDefeatedBoss(bossIdToDefeat);
        [SerializeField] private int bossIdToDefeat = 0;
        #endregion methods
	}
}