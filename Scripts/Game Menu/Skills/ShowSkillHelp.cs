using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu.Skills
{
    public class ShowSkillHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => SkillHelpUpdater.Instance;
        public string CooldownColor { get; set; } = "C39F68ED";
        public string CooldownValue { get; set; } = "";
        public int SkillId
        {
            get => skillId;
            set => SetSkillId(value);
        }
        private int skillId;
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (SkillId < 0)
                return;
            base.OpenPanel();
            SkillHelpUpdater.Instance.OpenPanel(Vector3.zero, SkillId, CooldownColor, CooldownValue);
        }
        private void SetSkillId(int value)
        {
            if (value == skillId) return;
            if (value < -1)
                throw new System.ArgumentOutOfRangeException("skill id");
            skillId = value;
        }
        #endregion methods
    }
}