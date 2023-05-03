using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using Data;
using System.Linq;

namespace WeakSoul.GameMenu.Blacksmith
{
    public class Craft : CraftUIList
    {
        #region fields & properties
        public static Craft Instance { get; private set; }
        [field: SerializeField] public GameObject ChoosePanel { get; private set; }
        [field: SerializeField] public GameObject MainPanel { get; private set; }
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        public void OnEnable()
        {
            GameData.Data.BlacksmithData.CurrentRecipe.OnOrderChanged += UpdateListData;
            UpdateListData();
        }
        public void OnDisable()
        {
            GameData.Data.BlacksmithData.CurrentRecipe.OnOrderChanged -= UpdateListData;
        }
        private void UpdateListData(int orderCell) => UpdateListData();
        public override void UpdateListData()
        {
            Items = GameData.Data.BlacksmithData.CurrentRecipe.GetItems();
            UpdateListDefault(Items, x => Items.IndexOf(x));
        }
        #endregion methods
    }
}