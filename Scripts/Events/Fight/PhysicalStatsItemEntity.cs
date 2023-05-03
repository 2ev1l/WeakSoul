using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using WeakSoul.GameMenu;

namespace WeakSoul.Events.Fight
{
    public class PhysicalStatsItemEntity : PhysicalStatsItem
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void SetText(PhysicalStatsItemList itemList, PhysicalStatsType type, Text statParamText, Color defaultColor, Color goodColor, Color badColor)
        {
            statParamText.color = defaultColor;
            statParamText.text = itemList.Stats.GetStatsTextByType(type);
        }
        #endregion methods
    }
}