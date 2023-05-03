using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Image))]
    public abstract class CellItem : MaterialRaycastChanger, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        #region fields & properties
        protected CellController CellController => Cell.CellController;
        protected Cell Cell => cell;
        [SerializeField] private Cell cell;
        private SpriteRenderer CellRenderer
        {
            get
            {
                cellRenderer = cellRenderer == null ? cell.GetComponent<SpriteRenderer>() : cellRenderer;
                return cellRenderer;
            }
        }
        private SpriteRenderer cellRenderer;
        private CursorChanger CursorChanger
        {
            get
            {
                cursorChanger = cursorChanger == null ? GetComponent<CursorChanger>() : cursorChanger;
                return cursorChanger;
            }
        }
        private CursorChanger cursorChanger;
        private Image Image
        {
            get
            {
                image = image == null ? GetComponent<Image>() : image;
                return image;
            }
        }
        private Image image;
        #endregion fields & properties

        #region methods
        protected virtual void OnEnable()
        {
            CheckCursorChanger();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (CellController.CellMove && CellController.StartCell != cell)
            {
                if (cell.ItemId == -1)
                {
                    CellController.CatchedCellIndex = cell.Index;
                    CellRenderer.material = IsCellAllowed(CellController.StartCell) ? CellController.CorrectCellMaterial : CellController.ErrorCellMaterial;
                    CellController.OnCellPreviewStart?.Invoke(cell.Index, CellController.StartCell.ItemId, CellController.StartCell.Index);
                    return;
                }
                else
                {
                    CellController.CatchedCellIndex = -1;
                    CellRenderer.material = CellController.ErrorCellMaterial;
                }
                return;
            }
            base.OnPointerEnter(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (CellController.CellMove || Cell.ItemId == -1 || eventData.button != PointerEventData.InputButton.Left || !Cell.AllowMove) return;
            CellController.OnCellSelected?.Invoke(cell);
            CellRenderer.material = CellController.StartedCellMaterial;
            Image.raycastTarget = false;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (CellController.StartCell != cell)
            {
                CellRenderer.material = CellController.DefaultCellMaterial;
                CellController.CatchedCellIndex = -1;
                CellController.OnCellPreviewEnd?.Invoke();
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (CellController.StartCell != cell || eventData.button != PointerEventData.InputButton.Left || !Cell.AllowMove) return;
            base.OnPointerExit(eventData);
            ReturnPosition();
            TryMoveItem();
            DeselectCell();
            CheckCursorChanger();
        }
        private void TryMoveItem()
        {
            int lastIndex = CellController.CatchedCellIndex;
            if (lastIndex == -1)
                return;
            if (IsCellAllowed())
                MoveItem();
        }
        private bool IsCellAllowed() => IsCellAllowed(cell);
        protected virtual bool IsCellAllowed(Cell cell)
        {
            return true;
        }
        protected abstract void MoveItem();
        private void ReturnPosition()
        {
            transform.position = cell.transform.position;
            CellRenderer.material = CellController.DefaultCellMaterial;
            CheckCursorChanger();
        }
        private void DeselectCell()
        {
            if (CellController.StartCell == cell)
            {
                CellController.OnCellDeselected?.Invoke(cell);
                Image.raycastTarget = true;
            }
            CheckCursorChanger();
        }
        private void OnDisable()
        {
            ReturnPosition();
            DeselectCell();
        }
        public void CheckCursorChanger()
        {
            if (CursorChanger == null) return;
            CursorChanger.enabled = cell.ItemId != -1;
        }
        #endregion methods
    }
}