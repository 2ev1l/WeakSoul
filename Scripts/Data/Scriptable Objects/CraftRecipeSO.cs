using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CraftRecipeSO", menuName = "ScriptableObjects/CraftRecipeSO")]
    public class CraftRecipeSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public CraftRecipe Recipe { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}