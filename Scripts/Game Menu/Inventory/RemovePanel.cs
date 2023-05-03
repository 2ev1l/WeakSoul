using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using System.Linq;

namespace WeakSoul.GameMenu.Inventory
{
    public class RemovePanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        [SerializeField] private LanguageLoader itemName;
        [SerializeField] private InventoryCellController cellController;
        private int itemId = -1;
        private int lastCellId = -1;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            cellController.OnRemoveCatched += OpenRemovePanel;
            CheckPanelOnRestart();
        }
        private void OnDisable()
        {
            cellController.OnRemoveCatched -= OpenRemovePanel;
            DiscardDelete();
        }
        private void CheckPanelOnRestart()
        {
            List<int> inventory = GameData.Data.PlayerData.Inventory.Items.ToList();
            int size = GameData.Data.PlayerData.Inventory.Size;
            if (inventory[20] != -1)
            {
                int index = -1;
                for (int i = 0; i < size; ++i)
                    if (inventory[i] == -1)
                    {
                        index = i;
                        break;
                    }
                if (index > -1)
                    OpenRemovePanel(inventory[20], index);
            }
        }
        public void ApplyDelete()
        {
            GameData.Data.PlayerData.Inventory.RemoveItem(20);
            panel.SetActive(false);
            lastCellId = -1;
        }
        public void DiscardDelete()
        {
            panel.SetActive(false);
            if (lastCellId > -1)
                GameData.Data.PlayerData.Inventory.MoveItem(20, lastCellId);
            lastCellId = -1;
        }
        private void OpenRemovePanel(int itemId, int lastCellId)
        {
            this.itemId = itemId;
            this.lastCellId = lastCellId;
            itemName.Id = itemId;
            panel.SetActive(true);
        }
        #endregion methods
    }
}