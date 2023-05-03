using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu
{
    public class ShowRecipeHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => RecipeHelpUpdater.Instance;
        public int RecipeId { get; set; }
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            RecipeHelpUpdater.Instance.OpenPanel(Vector3.zero, RecipeId);
        }
        #endregion methods
    }
}