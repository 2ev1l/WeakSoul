using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeakSoul.GameMenu.Inventory
{
    public class EquipmentUI : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject legs;
        [SerializeField] private GameObject weapon;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckCellEquipped;
            CheckCellEquipped(16);
            CheckCellEquipped(17);
            CheckCellEquipped(18);
            CheckCellEquipped(19);
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckCellEquipped;
        }
        private void CheckCellEquipped(int cellId) => CheckCellEquipped(GameData.Data.PlayerData.Inventory.GetItem(cellId), cellId, cellId);
        private void CheckCellEquipped(int itemId, int newCellId, int oldCellId)
        {
            if (!ItemsInventory.EquipmentCells.Contains(oldCellId) && !ItemsInventory.EquipmentCells.Contains(newCellId)) return;
            ItemsInventory playerInv = GameData.Data.PlayerData.Inventory;
            head.SetActive(playerInv.HeadArmor != null);
            body.SetActive(playerInv.BodyArmor != null);
            legs.SetActive(playerInv.LegsArmor != null);
            weapon.SetActive(playerInv.Weapon != null);
        }
        #endregion methods
    }
}