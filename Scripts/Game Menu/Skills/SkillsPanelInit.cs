using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Skills
{
    public class SkillsPanelInit : CanvasInit
    {
        #region fields & properties
        [SerializeField] private int setSkillSize = 6;
        [SerializeField] private int setOpenSkill = 0;
        [SerializeField] private int setSkillToFreeCell = 0;
        public static SkillsPanelInit Instance { get; private set; }
		public static SkillsCellController CellController { get; private set; }
        [SerializeField] private SkillsCellController cellController;
		[SerializeField] private List<Sprite> SkillsLine;
        [field: SerializeField] public Sprite LockedSkill { get; private set; }
        [field: SerializeField] public Material SkillDeffault { get; private set; }
        [field: SerializeField] public Material SkillNotBuyed { get; private set; }
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            Instance = this;
            CellController = cellController;
        }
        private void OnEnable()
        {
            PlayerStatsController.OnItemEquipped += CheckSkillEquipped;
            PlayerStatsController.OnItemDequipped += CheckCellDequipped;
        }
        private void OnDisable()
        {
            PlayerStatsController.OnItemEquipped -= CheckSkillEquipped;
            PlayerStatsController.OnItemDequipped -= CheckCellDequipped;
        }
        private void CheckSkillEquipped(int itemId)
        {
            StatsItem statsItem = ItemsInfo.Instance.GetStatsItem(itemId);
            if (statsItem.SkillId == -1) return;
            GameData.Data.PlayerData.Skills.TryAddTempOpenedSkill(statsItem.SkillId, true);
        }
        private void CheckCellDequipped(int itemId)
        {
            StatsItem statsItem = ItemsInfo.Instance.GetStatsItem(itemId);
            if (statsItem.SkillId == -1) return;
            GameData.Data.PlayerData.Skills.TryRemoveTempOpenedSkill(statsItem.SkillId);
        }
        public Sprite GetLineTexture(SkillType skillType) => skillType switch
        {
            SkillType.Attack => SkillsLine[0],
            SkillType.Block => SkillsLine[1],
            SkillType.Evade => SkillsLine[2],
            _ => throw new System.NotImplementedException()
        };
        [ContextMenu("Set skill size to")]
        private void SS6() => GameData.Data.PlayerData.Skills.Size = setSkillSize;
        [ContextMenu("Set stamina to 10")]
        private void SS10() => GameData.Data.PlayerData.Stats.Stamina = 10;
		[ContextMenu("Open Skill")]
		private void OS() => GameData.Data.PlayerData.Skills.TryAddOpenedSkill(setOpenSkill);
		[ContextMenu("Set Skill to free cell")]
		private void SSTFC()
        {
            int freeCell = GameData.Data.PlayerData.Skills.GetFreeCell();
            if (freeCell < 0 || GameData.Data.PlayerData.Skills.ContainItem(setSkillToFreeCell)) return;
            GameData.Data.PlayerData.Skills.SetItem(setSkillToFreeCell, freeCell);
        }
		#endregion methods
	}
}