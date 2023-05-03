using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.Events.Blacksmith
{
    public class BuyRecipe : BuyCustomItem
    {
        #region fields & properties
        [SerializeField] private ShowItemHelp showItemHelp;
        [SerializeField] private SpriteRenderer recipeSprite;
        [SerializeField][ReadOnly] private int recipeId;
        #endregion fields & properties

        #region methods
        public void Init(int recipeId)
        {
            this.recipeId = recipeId;
            CraftRecipe recipe = RecipesInfo.Instance.GetRecipe(recipeId);
            recipeSprite.sprite = ItemsInfo.Instance.GetItem(recipe.ItemId).Texture;
            SetPriceForRecipe(recipe);
            showItemHelp.ItemId = recipe.ItemId;
        }
        private void SetPriceForRecipe(CraftRecipe recipe)
        {
            int itemId = recipe.ItemId;
            Item item = ItemsInfo.Instance.GetItem(itemId);
            Wallet finalPrice = item.GetSellPrice();
            List<CraftSet> craftSets = recipe.GetItems();
            foreach (var el in craftSets)
            {
                if (el.Thing == CraftingThing.Item)
                {
                    if (CustomMath.GetRandomChance(50)) continue;

                    Item elItem = ItemsInfo.Instance.GetItem(el.Id);
                    Wallet itemSellPrice = elItem.GetSellPrice();
                    itemSellPrice.WeakSouls = CustomMath.Multiply(itemSellPrice.WeakSouls, 70);
                    itemSellPrice.NormalSouls = CustomMath.Multiply(itemSellPrice.NormalSouls, 70);
                    itemSellPrice.StrongSouls = CustomMath.Multiply(itemSellPrice.StrongSouls, 70);
                    itemSellPrice.UniqueSouls = CustomMath.Multiply(itemSellPrice.UniqueSouls, 70);
                    finalPrice.IncreaseValues(itemSellPrice);
                }
                else if (el.Thing == CraftingThing.Soul)
                {
                    if (CustomMath.GetRandomChance(50)) continue;
                    SoulType soul = (SoulType)el.Id;
                    finalPrice.SetSoulsByType(finalPrice.GetSoulsByType(soul) + 1, soul);
                }
            }
            this.BuyPrice = finalPrice;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            OnBought.AddListener(AddRecipe);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OnBought.RemoveListener(AddRecipe);
        }
        private void AddRecipe()
        {
            GameData.Data.BlacksmithData.TryOpenRecipe(recipeId);
        }
        #endregion methods
    }
}