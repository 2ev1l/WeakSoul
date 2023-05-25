using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Adventure.Backpack
{
    public class CardsList : ItemList
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            CardsPanel.OnCardAdded += UpdateListData;
            UpdateListData();
        }
        private void OnDisable()
        {
            CardsPanel.OnCardAdded -= UpdateListData;
        }
        public override void UpdateListData()
        {
            UpdateListDefault(CardsPanel.ChoosedCards, x => x);
        }
        #endregion methods
    }
}