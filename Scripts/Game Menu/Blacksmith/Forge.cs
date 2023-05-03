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
        public static UnityAction OnForgeStart;
        public static Sprite ForgeResultSprite { get; private set; }
        [SerializeField] private Sprite nullSprite;
        #endregion fields & properties

        #region methods
        public void ApplyCraft()
        {
            CraftRecipe currnet = GameData.Data.BlacksmithData.CurrentRecipe;
            CraftRecipeSO crafted = RecipesInfo.Instance.Recipes.Find(x => x.Recipe.IsCraftPossible(currnet));
            if (crafted == null)
                ForgeResultSprite = nullSprite;
            else
            {
                GameData.Data.BlacksmithData.TryOpenRecipe(crafted.Recipe.Id);
                ForgeResultSprite = RecipesInfo.Instance.GetRecipeTexture(crafted.Recipe.Id);
                TryAddItemToInventory(crafted.Recipe.ItemId);
            }
            Craft.Instance.OnDisable();
            OnForgeStart?.Invoke();
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