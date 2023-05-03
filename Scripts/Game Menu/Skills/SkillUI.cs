using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu.Skills
{
    [System.Serializable]
    public class SkillUI
    {
        #region fields & properties
        public SpriteRenderer Line => line;
        [SerializeField] private SpriteRenderer line;
        public SkillRender NextSkill => nextSkill;
        [SerializeField] private SkillRender nextSkill;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}