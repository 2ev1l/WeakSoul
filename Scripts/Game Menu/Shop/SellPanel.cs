using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Inventory;

namespace WeakSoul.GameMenu.Shop
{
    public class SellPanel : SingleSceneInstance
    {
        #region fields & properties
        public static SellPanel Instance { get; private set; }

        [SerializeField] private GameObject panel;
        [SerializeField] private LanguageLoader itemName;
        private int itemId => GameData.Data.PlayerData.Inventory.GetItem(cellId);
        private int cellId = -1;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            InputController.OnKeyDown += OnKeyDown;
            DiscardSell();
        }
        private void OnDisable()
        {
            InputController.OnKeyDown -= OnKeyDown;
            DiscardSell();
        }
        private void OnKeyDown(KeyCode keyCode) => DiscardSell();
        public void ApplySell()
        {
            Wallet itemSellPrice = ItemsInfo.Instance.GetItem(itemId).GetSellPrice();
            GameData.Data.PlayerData.Inventory.RemoveItem(cellId);
            GameData.Data.PlayerData.Wallet.IncreaseValues(itemSellPrice);
            cellId = -1;
            panel.SetActive(false);
            AudioManager.PlayClip(AudioStorage.Instance.BuySound, Universal.AudioType.Sound);
            GameObject.FindObjectsOfType<CellItem>(false).ToList().ForEach(x => x.CheckCursorChanger());
        }
        public void DiscardSell()
        {
            cellId = -1;
            panel.SetActive(false);
        }
        public void OpenSellPanel(int cellId)
        {
            this.cellId = cellId;
            itemName.Id = itemId;
            panel.SetActive(true);
        }
        #endregion methods
    }
}