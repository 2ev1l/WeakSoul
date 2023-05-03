using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using Data.ScriptableObjects;
using System.Linq;

namespace Data
{
    public class SkillsInfo : MonoBehaviour
    {
        #region fields & properties
        public static SkillsInfo Instance { get; private set; }
        [field: SerializeField] public List<SkillSO> Skills { get; private set; }
        [SerializeField] private List<Sprite> bgTextures;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        public Skill GetSkill(int skillId) => Skills[skillId].Skill;
        public Sprite GetTextureByType(SkillType skillType) => skillType switch
        {
            SkillType.Attack => bgTextures[0],
            SkillType.Block => bgTextures[1],
            SkillType.Evade => bgTextures[2],
            _ => throw new System.NotImplementedException()
        };


        [ContextMenu("Get all")]
        private void Get()
        {
            Skills = Resources.FindObjectsOfTypeAll<SkillSO>().OrderBy(x => x.Skill.Id).ToList();
            foreach (var el in Skills)
            {
                if (Skills.Where(x => x.Skill.Id == el.Skill.Id).Count() > 1)
                    Debug.LogError($"Error id {el.Skill.Id} at {el.name}");
            }
        }

        #endregion methods
    }
}