using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.City.Tavern
{
    public class TavernInit : SingleSceneInstance
    {
        #region fields & properties
        public static TavernInit Instance { get; private set; }
        [SerializeField] private TavernItemList tavernItemList;
        [Header("Debug")]
        [SerializeField][ReadOnly] private TavernData data;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
            data.OnDataGenerated += UpdateUI;
		}
		private void OnDisable()
		{
            data.OnDataGenerated -= UpdateUI;
		}
		protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        public void Init(TavernData data)
        {
            this.data = data;
            UpdateUI(data.Quests);
        }
        private void UpdateUI(IEnumerable<QuestData> quests)
        {
            tavernItemList.Quests = quests.ToList();
		}
        [ContextMenu("Generate data")]
        private void GenerateData()
        {
            data.GenerateData();
        }
        #endregion methods
    }
}