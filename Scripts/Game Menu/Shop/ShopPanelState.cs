using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using WeakSoul.MainMenu;

namespace WeakSoul.GameMenu.Shop
{
    public class ShopPanelState : MainPanelState
    {
        #region fields & properties
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Image raycastImage; 
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material unActiveMaterial;
        [SerializeField] private MaterialRaycastChanger materialRaycastChanger;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            raycastImage.enabled = !active;
            materialRaycastChanger.enabled = !active;
            spriteRenderer.material = active ? activeMaterial : unActiveMaterial;
        }
        #endregion methods
    }
}