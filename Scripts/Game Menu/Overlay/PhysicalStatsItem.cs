using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class PhysicalStatsItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => statsType;
        private int statsType = 0;
        [SerializeField] private Text statParamText;
        [SerializeField] private LanguageLoader statNameText;
        private PhysicalStatsItemList itemList;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            statsType = param;
            if (itemList == null) return;

            PhysicalStatsType type = (PhysicalStatsType)statsType;
            statNameText.Id = GetIdByPhysicalType(type);
            PhysicalStatsItemListStorage storage = PhysicalStatsItemListStorage.Instance;
            SetText(itemList, type, statParamText, storage.DefaultColor, storage.GoodColor, storage.BadColor);
        }
        protected virtual void SetText(PhysicalStatsItemList itemList, PhysicalStatsType type, Text statParamText, Color defaultColor, Color goodColor, Color badColor)
        {
            if (type == PhysicalStatsType.DamageType)
            {
                statParamText.text = itemList.Stats.GetStatsTextByType(type);
                statParamText.color = defaultColor;
                return;
            }
            statParamText.text = "";
            statParamText.text += itemList.Stats.GetStatsByType(type) > 0 ? " +" : "";
            statParamText.text += itemList.Stats.GetStatsTextByType(type);
            statParamText.color = itemList.Stats.GetStatsByType(type) > 0 ? goodColor : badColor;
        }
        public void SetItemList(PhysicalStatsItemList itemList)
        {
            this.itemList = itemList;
            OnListUpdate(listParam);
        }
        private int GetIdByPhysicalType(PhysicalStatsType type) => type switch
        {
            PhysicalStatsType.Health => 2,
            PhysicalStatsType.Damage => 3,
            PhysicalStatsType.DamageType => 4,
            PhysicalStatsType.Defense => 5,
            PhysicalStatsType.Resistance => 6,
            PhysicalStatsType.Stamina => 7,
            PhysicalStatsType.StaminaRegen => 8,
            PhysicalStatsType.EvasionChance => 9,
            PhysicalStatsType.CriticalChance => 10,
            PhysicalStatsType.CriticalScale => 11,
            _ => throw new System.NotImplementedException()
        };
        #endregion methods
    }
}