using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeakSoul.MainMenu;

namespace WeakSoul.GameMenu.Inventory
{
    public class InventoryPanelInit : CanvasInit
    {
        #region fields & properties
        public static InventoryPanelInit Instance { get; private set; }
        [field: SerializeField] public GameObject Panel { get; private set; }
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            Instance = this;
        }
        #endregion methods
    }
}