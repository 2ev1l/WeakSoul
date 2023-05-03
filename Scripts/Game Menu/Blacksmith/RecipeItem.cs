using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class RecipeItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => recipe.Id;
        private CraftRecipe recipe;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private ShowRecipeHelp showRecipeHelp;
        [SerializeField] private ShowItemHelp showItemHelp;
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            recipe = RecipesInfo.Instance.GetRecipe(param);
            itemSpriteRenderer.sprite = ItemsInfo.Instance.GetItem(recipe.ItemId).Texture;
            showRecipeHelp.RecipeId = recipe.Id;
            showItemHelp.ItemId = recipe.ItemId;
        }
        #endregion methods
    }
}