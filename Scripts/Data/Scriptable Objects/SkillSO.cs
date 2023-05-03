using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObjects/SkillSO")]
    public class SkillSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public Skill Skill { get; private set; } = new();
        #endregion fields & properties

        #region methods
        [ContextMenu("Print formula for my turns counting to enemy")]
        private void P() => Debug.Log($"Turns = 2*MyTurns-1");
        #endregion methods
    }
}