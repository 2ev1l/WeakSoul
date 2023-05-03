using Data;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Blacksmith
{
    public class BuyPanelInit : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private List<BuyRecipe> recipes;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            SetRecipes();
        }
        private void SetRecipes()
        {
            int maxRecipes = recipes.Count;
            List<int> recipesId = new();
            int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			HashSet<int> openedRecipes = GameData.Data.BlacksmithData.OpenedRecipes.ToHashSet();
			List<CraftRecipeSO> allowedRecipes = RecipesInfo.Instance.Recipes.Where(x => x.Recipe.Level <= playerLevel && !openedRecipes.Contains(x.Recipe.Id)).ToList();
            for (int i = recipes.Count - 1; i >= 0; --i)
            {
                if (CustomMath.GetRandomChance(30 + Mathf.Min(playerLevel * 2, 40))) continue;
                maxRecipes--;
                recipes[i].transform.parent.gameObject.SetActive(false);
            }
            for (int i = allowedRecipes.Count - 1; i >= 0; --i)
            {
                int recipeId = allowedRecipes[i].Recipe.Id;
                if (CustomMath.GetRandomChance(30)) continue;
                recipesId.Add(recipeId);
                if (recipesId.Count == maxRecipes) break;
            }
            foreach (var el in recipes)
            {
                if (recipesId.Count == 0)
                {
                    el.transform.parent.gameObject.SetActive(false);
                    continue;
                }
                if (!el.transform.parent.gameObject.activeSelf) continue;
                el.Init(recipesId.First());
                recipesId.RemoveAt(0);
            }
        }
        #endregion methods
    }
}