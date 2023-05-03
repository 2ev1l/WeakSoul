using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.Adventure
{
    public class SceneInit : SingleSceneInstance
    {
        #region fields & properties
        public static SceneInit Instance { get; private set; }
        [SerializeField] private List<GameObject> tutorialEnabled;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
            GameData.CanSaveData = false;
            CheckTutorial();
        }
        private void CheckTutorial()
        {
            if (GameData.Data.TutorialData.IsCompleted) return;
            tutorialEnabled.ForEach(x=>x.SetActive(true));
        }
        [ContextMenu("For video")]
        private void FV()
        {
            PlayerData playerData = GameData.Data.PlayerData;
            playerData.Inventory.Size = 15;
            playerData.Stats.SetPlayerClass(PlayerClass.Stoic);
            playerData.Wallet.SetSoulsByType(0, SoulType.Weak);
            playerData.Wallet.SetSoulsByType(0, SoulType.Normal);
            playerData.Wallet.SetSoulsByType(0, SoulType.Strong);
            playerData.Wallet.SetSoulsByType(0, SoulType.Unique);
            playerData.Wallet.SetSoulsByType(0, SoulType.Legendary);
            SavingUtils.SaveGameData();
        }
        #endregion methods
    }
}