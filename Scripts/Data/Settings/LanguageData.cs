using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class LanguageData
    {
        #region fields & properties
        [field: SerializeField] [field: TextArea(0, 30)] public string[] MenuData { get; private set; }
        [field: SerializeField] [field: TextArea(0, 30)] public string[] CutSceneData { get; private set; }
        [field: SerializeField] [field: TextArea(0, 30)] public string[] GameMenuData { get; private set; }
        [field: SerializeField] [field: TextArea(0, 30)] public string[] AdventureData { get; private set; }
        [field: SerializeField] [field: TextArea(0, 30)] public string[] EventsData { get; private set; }
        [field: SerializeField] [field: TextArea(5, 30)] public string[] TutorialData { get; private set; }

        
        [field: SerializeField] public List<ItemTextData> ItemsData { get; private set; }
        [field: SerializeField] public List<ItemTextData> SkillsData { get; private set; }
        [field: SerializeField] public List<ItemTextData> EffectsData { get; private set; }
        [field: SerializeField] public List<ItemTextData> CardGroupsData { get; private set; }
        [field: SerializeField] public List<ItemTextData> CardsData { get; private set; }
        [field: SerializeField] public List<ItemTextData> EnemiesData { get; private set; }
        [field: SerializeField] [field: TextArea(0, 5)] public string[] HelpData { get; private set; }
        #endregion fields & properties
    }
}