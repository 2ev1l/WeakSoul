using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Skills
{
    public class SkillHelpUpdater : HelpUpdater
    {
        #region fields & properties
        public static SkillHelpUpdater Instance { get; private set; }
        [SerializeField] private LanguageLoader nameText;
        [SerializeField] private LanguageLoader descriptionText;
        [SerializeField] private LanguageLoader skillPriceText;
        [SerializeField] private LanguageLoader skillCostText;
        [SerializeField] private LanguageLoader skillCooldownText;
        [SerializeField] private LanguageLoader skillLevelText;

        [SerializeField] private Color badColor;
        [SerializeField] private Color goodColor;
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            base.Init();
            Instance = this;
        }
        public void OpenPanel(Vector3 position, int skillId, string cooldownColor, string cooldownValue)
        {
            if (skillId < 0) return;
            base.OpenPanel(position);
            Skill skill = SkillsInfo.Instance.GetSkill(skillId);
            nameText.Id = skillId;
            descriptionText.Id = skillId;

            string turnsActive = "";
            List<int> turns = new();
            if (skill.SkillBuff.Turns > 0)
                turns.Add(skill.SkillBuff.Turns);
            foreach (var el in skill.SkillBuff.StatsScale)
            {
                if (turns.Contains(el.Turns) || el.Turns == 0) continue;
                turns.Add(el.Turns);
            }
            if (turns.Count == 1)
                turnsActive = turns.First().ToString();
            else
            {
                if (skill.SkillBuff.Turns > 0)
                    turnsActive = $"{skill.SkillBuff.Turns}";
                foreach (var el in skill.SkillBuff.StatsScale)
                {
                    if (el.Turns <= 0) continue;
                    turnsActive += $"\\{el.Turns}";
                }
            }

            string activeTurnsLanguage = LanguageLoader.GetTextByType(TextType.GameMenu, 46);
            activeTurnsLanguage = activeTurnsLanguage.Replace("[X]", turnsActive);
            descriptionText.AddText(turnsActive != "" ? $"\n\n{activeTurnsLanguage}" : "");

            if (cooldownValue == "")
                cooldownValue = $"{skill.Cooldown}";
            bool opened = skill.IsOpened;
            bool canOpen = skill.CanOpenSkill();
            skillCostText.AddText($" <color=#{(GameData.Data.PlayerData.Stats.Stamina >= skill.StaminaPrice ? goodColor : badColor).ToHexString()}>{skill.StaminaPrice}</color>");
            skillCooldownText.AddText($" <color=#{cooldownColor}>{cooldownValue}</color>");
            skillPriceText.AddText($" <color=#{(skill.SPAccess() ? goodColor : badColor).ToHexString()}>{skill.OpenPrice} SP</color>" +
                $"{(opened ? "" : (!canOpen ? "" : (" (" + LanguageLoader.GetTextByType(TextType.GameMenu, 22) + ")")))}");
            skillLevelText.AddText($" <color=#{(skill.LevelAccess() ? goodColor : badColor).ToHexString()}>{skill.Level}</color>");

            skillPriceText.gameObject.SetActive(!opened);
        }
        #endregion methods
    }
}