using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class Skill
    {
        #region fields & properties
        public static UnityAction<Skill> OnSkillOpened;
        public Sprite Texture => texture;
        [SerializeField] private Sprite texture;

        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public int Level => level;
        [Min(0)][SerializeField] private int level;
        public int OpenPrice => openPrice;
        [Min(0)][SerializeField] private int openPrice;
        public int StaminaPrice => staminaPrice;
		[Min(0)][SerializeField] private int staminaPrice;
        public int Cooldown => cooldown;
		[Min(-1)][SerializeField] private int cooldown;

        public SkillType SkillType => skillType;
        [SerializeField] private SkillType skillType;
        public SkillBuff SkillBuff => skillBuff;
        [SerializeField] private SkillBuff skillBuff;
        public SkillBuff SkillEnemyBuff => skillEnemyBuff;
        [SerializeField] private SkillBuff skillEnemyBuff;
        public SkillVFXAnimation VFXAnimation => vfxAnimation;
        [SerializeField] private SkillVFXAnimation vfxAnimation;
        public bool IsOpened => GameData.Data.PlayerData.Skills.IsSkillOpened(id);
        public bool IsTempOpened => GameData.Data.PlayerData.Skills.IsSkillTempOpened(id);
        #endregion fields & properties

        #region methods
        public bool TryOpenSkill()
        {
            if (!CanOpenSkill()) return false;
            OpenSkill();
            return true;
        }
        public bool CanOpenSkill() => (!IsOpened && SPAccess() && LevelAccess());
        public bool LevelAccess() => GameData.Data.PlayerData.Stats.ExperienceLevel.Level >= level;
        public bool SPAccess() => GameData.Data.PlayerData.Stats.SkillPoints >= openPrice;
        private void OpenSkill()
        {
            GameData.Data.PlayerData.Skills.TryAddOpenedSkill(id);
            GameData.Data.PlayerData.Stats.SkillPoints -= openPrice;
            OnSkillOpened?.Invoke(this);
        }

        public Skill() { }
        public Skill(int id)
        {
            this.id = id;
        }
        public Skill Clone() => new()
        {
            id = id,
            level = level,
            texture = texture,
            skillType = skillType,
            cooldown = cooldown,
            openPrice = openPrice,
            skillBuff = skillBuff.Clone(),
            skillEnemyBuff = skillEnemyBuff.Clone(),
            staminaPrice = staminaPrice,
            vfxAnimation = vfxAnimation.Clone()
        };

        #endregion methods
    }
}