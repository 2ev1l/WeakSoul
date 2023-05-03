using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace WeakSoul.GameMenu.Inventory
{
    public class CellController : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> cell;
        /// </summary>
        public UnityAction<Cell> OnCellSelected;
        /// <summary>
        /// <see cref="{T0}"/> cell;
        /// </summary>
        public UnityAction<Cell> OnCellDeselected;
        /// <summary>
        /// <see cref="{T0}"/> cellIndex;
        /// </summary>
        public UnityAction<int> OnCellEquipped;
        /// <summary>
        /// <see cref="{T0}"/> cellIndex; <see cref="{T1}"/> itemId; <see cref="{T2}"/> startCellIndex;
        /// </summary>
        public UnityAction<int, int, int> OnCellPreviewStart;
        public UnityAction OnCellPreviewEnd;

        public bool CellMove { get; private set; } = false;
        public int CatchedCellIndex { get; set; } = 0;
        public Cell StartCell { get; private set; } = null;
        private Transform movingTransform;

        protected GameObject[] Cells => cells;
        [SerializeField] private GameObject[] cells;
        [field: SerializeField] public Material CorrectCellMaterial { get; private set; }
        [field: SerializeField] public Material ErrorCellMaterial { get; private set; }
        [field: SerializeField] public Material DefaultCellMaterial { get; private set; }
        [field: SerializeField] public Material StartedCellMaterial { get; private set; }
        #endregion fields & properties

        #region methods
        public virtual void Init() { }
        protected virtual void OnEnable()
        {
            OnCellSelected += ActivateMove;
            OnCellDeselected += DropMove;
        }
        protected virtual void OnDisable()
        {
            OnCellSelected -= ActivateMove;
            OnCellDeselected -= DropMove;
            CatchedCellIndex = -1;
        }
        protected virtual void UpdateCells(int inventorySize)
        {
            for (int i = 0; i < cells.Length; ++i)
                cells[i].SetActive(i < inventorySize);
        }
        protected virtual void ActivateMove(Cell cell)
        {
            CatchedCellIndex = -1;
            StartCell = cell;
            movingTransform = StartCell.transform.GetChild(0);
            CellMove = true;
        }
        protected virtual void DropMove(Cell cell)
        {
            CellMove = false;
            movingTransform.position = cell.transform.position;
            OnCellEquipped?.Invoke(CatchedCellIndex);
        }
        private void Update()
        {
            if (!CellMove) return;
            Vector3 pos = movingTransform.position;
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            movingTransform.position = pos;
        }
        #endregion methods
    }
}