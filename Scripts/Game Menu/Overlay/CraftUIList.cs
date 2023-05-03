using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class CraftUIList : ItemList
    {
        #region fields & properties
        public List<CraftSet> Items { get; protected set; }
        public int RecipeId
        {
            get => recipeId;
            set
            {
                recipeId = value;
                UpdateListData();
            }
        }
        private int recipeId;
        #endregion fields & properties

        #region methods
        public override void UpdateListData()
        {
            Items = RecipesInfo.Instance.GetRecipe(RecipeId).GetItems();
            UpdateListDefault(Items, x => Items.IndexOf(x));
        }
        #endregion methods
    }
}