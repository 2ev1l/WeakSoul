using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.GameMenu.Witch
{
    public class WitchInit : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private List<ShopItemUI> items;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            LoadItems();
        }
        private void OnEnable()
        {
            GameData.Data.OnDaysChanged += ReloadShop;
            GameData.Data.WitchData.OnItemsGenerated += LoadItems;
        }
        private void OnDisable()
        {
            GameData.Data.OnDaysChanged -= ReloadShop;
            GameData.Data.WitchData.OnItemsGenerated -= LoadItems;
        }
        public void ReloadShop(int days) => ReloadShop();
        [ContextMenu("Generate data")]
        private void ReloadShop()=> GameData.Data.WitchData.GenerateItems();
		private void LoadItems() => LoadItems(GameData.Data.WitchData.Items);
        private void LoadItems(IEnumerable<WitchItem> wi)
        {
            if (wi == null) return;
            List<WitchItem> witchItems = wi.ToList();
            foreach (var el in items)
            {
                WitchItem witchItem = witchItems.Find(x => x.Type == el.Type);
                if (witchItem == null)
                {
                    el.DisableItem();
                    continue;
                }
                el.Load(witchItem);
                witchItems.Remove(witchItem);
            }
        }
        #endregion methods
    }
}