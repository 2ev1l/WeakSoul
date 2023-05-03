using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class RecipesList : ItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            UpdateListData();
        }
        public override void UpdateListData()
        {
            int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            List<int> recipes = GameData.Data.BlacksmithData.OpenedRecipes.Where(x => RecipesInfo.Instance.GetRecipe(x).Level <= playerLevel).ToList();
            UpdateListDefault(recipes, x => x);
        }
        #endregion methods
    }
}