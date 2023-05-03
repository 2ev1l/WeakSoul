using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ShopCellValue
    {
        #region fields & properties
        public int Level => level;
        [SerializeField] private int level;
        public int Value => value;
        [SerializeField] private int value;
        public Wallet Price => price;
        [SerializeField] private Wallet price;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}