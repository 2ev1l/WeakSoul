using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public abstract class ChooseItem : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] protected CustomButton customButton;
        #endregion fields & properties

        #region methods
        protected virtual void OnEnable()
        {
            customButton.OnClicked += AddToCraft;
        }
        protected virtual void OnDisable()
        {
            customButton.OnClicked -= AddToCraft;
        }
        protected virtual void AddToCraft()
        {
            CraftItem.ChoosedCellIndex = -1;
            Craft.Instance.ChoosePanel.SetActive(false);
            Craft.Instance.MainPanel.SetActive(true);
        }
        #endregion methods
    }
}