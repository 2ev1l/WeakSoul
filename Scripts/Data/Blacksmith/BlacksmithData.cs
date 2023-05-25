using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class BlacksmithData
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> recipe id;
        /// </summary>
        public UnityAction<int> OnRecipeOpened;
        public UnityAction<int> OnCraftsCountChanged;

        public IEnumerable<int> OpenedRecipes => openedRecipes;
        [SerializeField] private List<int> openedRecipes = new() { 0, 3 };
        public CraftRecipe CurrentRecipe => currentRecipe;
        [SerializeField] private CraftRecipe currentRecipe = new();
        public int CraftsCount
        {
            get => craftsCount;
            set
            {
                craftsCount = value;
                OnCraftsCountChanged?.Invoke(craftsCount);
            }
        }
        [SerializeField] private int craftsCount;
        #endregion fields & properties

        #region methods
        public bool TryOpenRecipe(int recipeId)
        {
            if(openedRecipes.Contains(recipeId)) return false;
            openedRecipes.Add(recipeId);
            OnRecipeOpened?.Invoke(recipeId);
            return true;
        }
        public BlacksmithData()
        {
            openedRecipes = new() { 0, 3 };
        }
        #endregion methods
    }
}