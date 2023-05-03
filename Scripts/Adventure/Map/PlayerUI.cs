using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeakSoul.Adventure.Map
{
    public class PlayerUI : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private SpriteRenderer spriteRenderer;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckWeapon;
            Player.OnPointChanged += CheckPoint;
            Player.OnStartMoving += ResetOldPoint;
            UpdateSprite();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckWeapon;
            Player.OnPointChanged -= CheckPoint;
            Player.OnStartMoving -= ResetOldPoint;
        }
        private void CheckWeapon(int itemId, int newCellId, int oldCellId)
        {
            if (newCellId == ItemsInventory.WeaponCell || oldCellId == ItemsInventory.WeaponCell)
                UpdateSprite();
        }
        private void CheckPoint(int oldPointId, int newPointId)
        {
            Point _new = PointsInit.Instance.GetPoint(newPointId);
            _new.SpriteRenderer.material = PointsInit.Instance.IconChoosedMaterial;
            if (_new.Data.ChoosedEvent.Id != 0)
                spriteRenderer.material = PointsInit.Instance.IconChoosedMaterial;
        }
        private void ResetOldPoint(int oldPointId)
        {
            Point _old = PointsInit.Instance.GetPoint(oldPointId);
            _old.SpriteRenderer.material = PointsInit.Instance.IconDefaultMaterial;
            spriteRenderer.material = PointsInit.Instance.IconDefaultMaterial;
        }
        private void UpdateSprite()
        {
            try { spriteRenderer.sprite = GameData.Data.PlayerData.Inventory.Weapon.Texture; }
            catch { spriteRenderer.sprite = ItemsInfo.Instance.Weapons.Find(x => x != null).Weapon.Texture; }
        }
        #endregion methods
    }
}