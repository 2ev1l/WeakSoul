using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using Universal.Effects;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class CraftItem : CraftItemUI, IListUpdater
    {
        #region fields & properties
        [SerializeField] private SpriteRenderer cellSpriteRenderer;
        [SerializeField] private CustomButton customButton;
        [SerializeField] private MaterialRaycastChanger materialRaycastChanger;
        public static int ChoosedCellIndex = -1;
        #endregion fields & properties

        #region methods
        public override void OnListUpdate(int param)
        {
            base.OnListUpdate(param);
            materialRaycastChanger.SetChangedMaterial(ShopInfo.Instance.GoodPrice);
        }
        private void OnEnable()
        {
            customButton.OnClicked += CheckCellClick;
            Forge.OnForgeStart += OnForge;
            materialRaycastChanger.OnMaterialChanged += SetCellMaterial;
        }
        private void OnDisable()
        {
            customButton.OnClicked -= CheckCellClick;
            Forge.OnForgeStart -= OnForge;
            materialRaycastChanger.OnMaterialChanged -= SetCellMaterial;
        }
        protected override CraftSet GetCraftSet() => Craft.Instance.Items[cellIndex];
        private void SetCellMaterial(bool isDefault)
        {
            cellSpriteRenderer.material = isDefault ? materialRaycastChanger.MaterialDefault : materialRaycastChanger.MaterialChanged;
        }
        private void CheckCellClick()
        {
            switch (craftSet.Thing)
            {
                case CraftingThing.None:
                    ChoosedCellIndex = cellIndex;
                    Craft.Instance.ChoosePanel.SetActive(true);
                    Craft.Instance.MainPanel.SetActive(false);
                    break;
                case CraftingThing.Item:
                    int freeCell = GameData.Data.PlayerData.Inventory.GetFreeCell();
                    if (freeCell == -1)
                    {
                        itemSpriteRenderer.material = ShopInfo.Instance.BadPrice;
                        materialRaycastChanger.SetChangedMaterial(ShopInfo.Instance.BadPrice);
                        return;
                    }
                    itemSpriteRenderer.material = ShopInfo.Instance.GoodPrice;
                    materialRaycastChanger.SetChangedMaterial(ShopInfo.Instance.GoodPrice);
                    GameData.Data.PlayerData.Inventory.SetItem(craftSet.Id, freeCell);
                    GameData.Data.BlacksmithData.CurrentRecipe.SetItem(0, CraftingThing.None, cellIndex);
                    break;
                case CraftingThing.Soul:
                    Wallet playerWallet = GameData.Data.PlayerData.Wallet;
                    playerWallet.SetSoulsByType(playerWallet.GetSoulsByType((SoulType)craftSet.Id) + 1, (SoulType)craftSet.Id);
                    GameData.Data.BlacksmithData.CurrentRecipe.SetItem(0, CraftingThing.None, cellIndex);
                    break;
            }
        }
        private void OnForge()
        {
            GameData.Data.BlacksmithData.CurrentRecipe.SetItem(0, CraftingThing.None, cellIndex);
            StartCoroutine(SpriteBurn());
        }
        private IEnumerator SpriteBurn()
        {
            Material mat = itemSpriteRenderer.material;
            GameObject child = itemSpriteRenderer.gameObject;
            Burn burn = child.AddComponent<Burn>();
            burn.StartAnimation();
            while (!burn.IsAnimationEnded)
                yield return CustomMath.WaitAFrame();
            Color col = itemSpriteRenderer.color;
            col.a = 0;
            itemSpriteRenderer.color = col;
            itemSpriteRenderer.material = mat;
        }
        #endregion methods
    }
}