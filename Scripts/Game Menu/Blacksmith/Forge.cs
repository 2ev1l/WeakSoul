using Data;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class Forge : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> - itemId;
        /// </summary>
        public static UnityAction<int> OnForgeStart;
        public static Sprite ForgeResultSprite { get; private set; }
        [SerializeField] private Sprite nullSprite;
        #endregion fields & properties

        #region methods
        public void ApplyCraft()
        {
            CraftRecipe currnet = GameData.Data.BlacksmithData.CurrentRecipe;
            CraftRecipeSO crafted = RecipesInfo.Instance.Recipes.Find(x => x.Recipe.IsCraftPossible(currnet));
            int itemId = -1;
            if (crafted == null)
                ForgeResultSprite = nullSprite;
            else
            {
                int recipeId = crafted.Recipe.Id;
                BlacksmithData blacksmithData = GameData.Data.BlacksmithData;
                blacksmithData.TryOpenRecipe(recipeId);
                blacksmithData.CraftsCount += 1;
                ForgeResultSprite = RecipesInfo.Instance.GetRecipeTexture(recipeId);
                itemId = crafted.Recipe.ItemId;
                TryAddItemToInventory(itemId);
            }
            Craft.Instance.OnDisable();
            OnForgeStart?.Invoke(itemId);
            AudioManager.PlayClip(AudioStorage.Instance.HammerSound, Universal.AudioType.Sound);
            StartCoroutine(ResetList());
        }
        private void TryAddItemToInventory(int itemId)
        {
            int freeCell = GameData.Data.PlayerData.Inventory.GetFreeCell();
            if (freeCell == -1) return;
            GameData.Data.PlayerData.Inventory.SetItem(itemId, freeCell);
        }
        private IEnumerator ResetList()
        {
            yield return new WaitForSeconds(1.5f);
            if (Craft.Instance != null)
                Craft.Instance.OnEnable();
        }
        #endregion methods
    }
}