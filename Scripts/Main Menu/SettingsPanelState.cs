using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.MainMenu
{
    public class SettingsPanelState : StateChange
    {
        #region fields & properties
        [SerializeField] private GameObject panel;
        [SerializeField] private Image image;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material changedMaterial;
        [SerializeField] private MaterialRaycastChanger materialRaycastChanger;
        [SerializeField] private List<ItemList> itemLists;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            panel.SetActive(active);
            image.raycastTarget = !active;
            materialRaycastChanger.enabled = !active;
            spriteRenderer.material = active ? changedMaterial : defaultMaterial;
            if (active && itemLists.Count > 0)
                itemLists.ForEach(x => x.UpdateListData());
        }
        #endregion methods
    }
}