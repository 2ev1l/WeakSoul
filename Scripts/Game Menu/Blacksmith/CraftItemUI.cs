using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class CraftItemUI : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => cellIndex;
        protected int cellIndex = -1;
        protected CraftSet craftSet;
        [SerializeField] protected SpriteRenderer itemSpriteRenderer;
        #endregion fields & properties

        #region methods
        public virtual void OnListUpdate(int param)
        {
            cellIndex = param;
            craftSet = GetCraftSet();
            itemSpriteRenderer.sprite = craftSet.Thing switch
            {
                CraftingThing.None => null,
                CraftingThing.Item => ItemsInfo.Instance.GetItem(craftSet.Id).Texture,
                CraftingThing.Soul => SoulsInfo.Instance.GetInfo((SoulType)craftSet.Id).Sprite2x,
                _ => throw new System.NotImplementedException()
            };
            Color col = itemSpriteRenderer.color;
            col.a = 1;
            itemSpriteRenderer.color = col;
        }
        protected virtual CraftSet GetCraftSet() => RecipeHelpUpdater.Instance.ItemList.Items[cellIndex];
        #endregion methods
    }
}