using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class ChoosePanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private GameObject choosePanel;
        [SerializeField] private GameObject mainPanel;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            InputController.OnKeyDown += ChangePanel;
        }
        private void OnDisable()
        {
            InputController.OnKeyDown -= ChangePanel;
        }
        private void ChangePanel(KeyCode key)
        {
            if (mainPanel.activeSelf) return;
            bool state = !InventoryPanelInit.Instance.Panel.activeSelf;
            if (state == choosePanel.activeSelf) return;
            choosePanel.SetActive(state);
        }
        #endregion methods
    }
}