using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class Item
    {
        #region fields & properties
        public UnityAction OnStatsChanged;
        public Sprite Texture => texture;
        [SerializeField] private Sprite texture;

        public int Id => id;
        [Min(0)] [SerializeField] private int id;
        public int Level => level;
        [Min(0)] [SerializeField] private int level;
        public bool CanBuy => canBuy;
        [SerializeField] private bool canBuy = true;
        public bool IsDestroyable => isDestroyable;
        [SerializeField] private bool isDestroyable = true;
        public Wallet Price => price;
        [SerializeField] private Wallet price = new();
        #endregion fields & properties

        #region methods
        public virtual Wallet GetSellPrice()
        {
            Wallet wallet = new();
            wallet.WeakSouls = DivideSellPrice(price.WeakSouls, 2);
            wallet.NormalSouls = DivideSellPrice(price.NormalSouls, 2);
            wallet.StrongSouls = DivideSellPrice(price.StrongSouls, 2);
            wallet.UniqueSouls = DivideSellPrice(price.UniqueSouls, 2);
            wallet.LegendarySouls = DivideSellPrice(price.LegendarySouls, 2);
            return wallet;
        }
        protected virtual int DivideSellPrice(float value, float divideScale) => Mathf.RoundToInt(value / divideScale);
        public Item Clone()
        {
            Item item = new Item();
            item.texture = texture;
            item.id = id;
            item.level = level;
            Wallet wallet = price.Clone();
            item.price = wallet;
            return item;
        }
        #endregion methods
    }
}