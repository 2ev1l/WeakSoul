using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class CraftRecipe
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> orderCell;
        /// </summary>
        public UnityAction<int> OnOrderChanged;
        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public int ItemId => itemId;
        [Min(0)][SerializeField] private int itemId;
        public int Level => level;
        [Min(0)][SerializeField] private int level;
        [SerializeField] private CraftSet[] order = new CraftSet[length];
        private static readonly int length = 5;
        #endregion fields & properties

        #region methods
        public bool IsCraftPossible(CraftRecipe recipe)
        {
            bool isOpened = GameData.Data.BlacksmithData.OpenedRecipes.Contains(id);
            if (!isOpened || Level > GameData.Data.PlayerData.Stats.ExperienceLevel.Level) return false;

            for (int i = 0; i < length; ++i)
            {
                bool isTypeAllowed = recipe.order[i].Thing == order[i].Thing;
                bool isIdAllowed = recipe.order[i].Id == order[i].Id;
                if (!isTypeAllowed || !isIdAllowed)
                    return false;
            }
            return true;
        }
        public void SetItem(int id, CraftingThing thing, int orderCell)
        {
            order[orderCell].SetValues(id, thing);
            OnOrderChanged?.Invoke(orderCell);
        }
        public CraftSet GetItem(int orderCell) => order[orderCell];
        /// <summary>
        /// O(n)
        /// </summary>
        /// <returns></returns>
        public List<CraftSet> GetItems() => order.ToList();
        //public void ChangeID(int id) => this.id = id;
        #endregion methods
    }
}