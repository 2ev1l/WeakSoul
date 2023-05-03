using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
    public class PhysicalStatsItemListStorage : MonoBehaviour
    {
        #region fields & properties
        public static PhysicalStatsItemListStorage Instance { get; private set; }

        [field: SerializeField] public Color DefaultColor { get; private set; }
        [field: SerializeField] public Color BadColor { get; private set; }
        [field: SerializeField] public Color GoodColor { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        #endregion methods
    }
}