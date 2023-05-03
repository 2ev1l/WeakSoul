using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class InventoryData
    {
        #region fields & properties
        public UnityAction<int> OnSizeChanged;
        /// <summary>
        /// <see cref="{T0}"/> itemId;
        /// <see cref="{T1}"/> newCellId;
        /// <see cref="{T2}"/> oldCellId;
        /// </summary>
        public UnityAction<int, int, int> OnInventoryChanged;

        /// <summary>
        /// Doesn't affect on <see cref="items"/>
        /// </summary>
        public int Size
        {
            get => size;
            set => SetSize(value);
        }
        [SerializeField] private int size = -1;
        public IEnumerable<int> Items => items;
        [SerializeField] protected List<int> items = new List<int>();
        #endregion fields & properties

        #region methods
        private void SetSize(int value)
        {
            if (value < -1)
                throw new System.ArgumentOutOfRangeException("Inventory size");
            size = value;
            OnSizeChanged?.Invoke(value);
        }
        /// <summary>
        /// If you want to set "-1" for id, then use <see cref="RemoveItem(int)"/>
        /// </summary>
        /// <param name="id">>=0</param>
        /// <param name="index">>=0</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void SetItem(int id, int index)
        {
            if (id < -1)
                throw new System.ArgumentOutOfRangeException("Inventory item id");
            items[index] = id;
            OnInventoryChanged?.Invoke(id, index, index);
        }
        public void RemoveItem(int index) => SetItem(-1, index);
        public void MoveItem(int oldIndex, int newIndex)
        {
            int newItem = items[newIndex];
            int oldItem = items[oldIndex];
            items[newIndex] = oldItem;
            items[oldIndex] = newItem;
            OnInventoryChanged?.Invoke(oldItem, newIndex, oldIndex);
        }
        public virtual int GetFreeCell()
        {
            for (int i = 0; i < Size; ++i)
                if (items[i] == -1)
                    return i;
            return -1;
        }
        public virtual List<int> GetFilledItems()
        {
            List<int> l = new();
            for (int i = 0; i < size; ++i)
            {
                if (items[i] == -1) continue;
                l.Add(items[i]);
            }
            return l;
        }
        public int GetItem(int index) => items[index];
        public bool ContainItem(int itemId) => items.Contains(itemId);
        public int FindItemIndex(int itemId) => items.FindIndex(x => x == itemId);
        #endregion methods
    }
}