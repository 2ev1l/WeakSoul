using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class RecipeHelpUpdater : HelpUpdater
    {
        #region fields & properties
        public static RecipeHelpUpdater Instance { get; private set; }
        public CraftUIList ItemList => itemList;
        [SerializeField] private CraftUIList itemList;
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            base.Init();
            Instance = this;
        }
        public void OpenPanel(Vector3 position, int recipeId)
        {
            itemList.RecipeId = recipeId;
            base.OpenPanel(position);
        }
        #endregion methods
    }
}