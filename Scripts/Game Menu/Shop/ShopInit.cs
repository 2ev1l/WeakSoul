using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Shop
{
    public class ShopInit : SingleSceneInstance
    {
        #region fields & properties
        public static ShopInit Instance { get; private set; }

        [SerializeField] private List<ShopItemUI> items;
        [SerializeField] private bool checkType = true;
        public ShopData Data => data;
        [SerializeField][ReadOnly] protected ShopData data;
        #endregion fields & properties

        #region methods
        protected virtual void Start()
        {
            LoadItems();
        }
        public void Init(ShopData data)
        {
            this.data = data;
            LoadItems();
        }
        protected virtual void OnEnable()
        {
            Data.OnItemsGenerated += LoadItems;
        }
        protected virtual void OnDisable()
        {
            Data.OnItemsGenerated -= LoadItems;
        }
        public void LoadItems() => LoadItems(Data.Items);
        private void LoadItems(IEnumerable<ShopItem> si)
        {
            if (si == null) return;
            List<ShopItem> shopItems = si.ToList();
            foreach (var el in items)
            {
                ShopItem shopItem = null;
                if (checkType)
                    shopItem = shopItems.Find(x => x.Type == el.Type);
                else
                    shopItem = shopItems.Count > 0 ? shopItems.First() : null;
                if (shopItem == null)
                {
                    el.DisableItem();
                    continue;
                }
                el.Load(shopItem);
                shopItems.Remove(shopItem);
            }
        }

        [ContextMenu("Days++")]
        private void D() => GameData.Data.Days++;
        [ContextMenu("Souls++")]
        private void S() => GameData.Data.PlayerData.Wallet.WeakSouls++;

        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}