using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Shop
{
    public class SellPanelOpener : MonoBehaviour, IPointerClickHandler
    {
        #region fields & properties
        [SerializeField] private InventoryCell inventoryCell;
        #endregion fields & properties

        #region methods
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || inventoryCell.ItemId == -1) return;
            SellPanel.Instance.OpenSellPanel(inventoryCell.Index);
        }
        #endregion methods
    }
}