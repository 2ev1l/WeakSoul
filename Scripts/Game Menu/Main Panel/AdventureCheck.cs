using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.GameMenu
{
    public class AdventureCheck : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private ShowTextHelp help;
        [SerializeField] private CustomButton button;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Skills.OnInventoryChanged += CheckAllowSkills;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckAllowInventory;
            CheckAllow();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Skills.OnInventoryChanged -= CheckAllowSkills;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckAllowInventory;
        }
        private void CheckAllowSkills(int skillId, int newIndex, int oldIndex)
        {
            if (oldIndex != newIndex) return;
            CheckAllow();
        }
        private void CheckAllowInventory(int itemId, int newIndex, int oldIndex)
        {
            if (oldIndex == newIndex) return;
            CheckAllow();
        }
        private void CheckAllow()
        {
            bool skillAllow = false;
            List<int> skills = GameData.Data.PlayerData.Skills.GetFilledItems();
            foreach (int skill in skills)
            {
                if (SkillsInfo.Instance.GetSkill(skill).SkillType == SkillType.Attack)
                    skillAllow = true;
            }
            if (!skillAllow)
                help.Id = 4;
            button.enabled = skillAllow;
            help.enabled = !button.enabled;
        }
        public void LoadAdventure()
        {
            PointsInit.ClearGeneratedData();
            Player.CurrentPointId = 0;
            PlayerStatsController.Instance.CollectStats();
            SavingUtils.TrySaveGameData();
            GameData.CanSaveData = false;
            SceneLoader.Instance.LoadSceneFade("Adventure", 1f);
        }
        #endregion methods
    }
}