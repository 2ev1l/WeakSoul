using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Universal;

namespace WeakSoul.GameMenu.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Cell : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> itemId; 
        /// <see cref="{T1}"/> newCellId;
        /// <see cref="{T2}"/> oldCellId;
        /// </summary>
        protected abstract UnityAction<int, int, int> OnInventoryChanged { get; set; }
        public abstract int ItemId { get; }
        public abstract CellController CellController { get; }
        public abstract Sprite Texture { get; }
        public int Index => index;
        [SerializeField] private int index;
        public bool AllowMove => allowMove;
        [SerializeField] private bool allowMove = true;
        [SerializeField] private SpriteRenderer itemRenderer;
        #endregion fields & properties

        #region methods
        public void SetItem(int itemId, int newCellId, int oldCellId)
        {
            if (newCellId != index && oldCellId != index) return;
            RenderCell();
            if (itemId == -1) return;
            CellController.OnCellEquipped?.Invoke(index);
        }
        protected virtual void OnEnable()
        {
            OnInventoryChanged += SetItem;
            SetItem(ItemId, index, index);
        }
        protected virtual void OnDisable()
        {
            OnInventoryChanged -= SetItem;
        }
        protected virtual void RenderCell()
        {
            int itemId = ItemId;
            if (itemId == -1)
            {
                itemRenderer.enabled = false;
                return;
            }
            itemRenderer.enabled = true;
            itemRenderer.sprite = Texture;
        }
        #endregion methods
    }
}