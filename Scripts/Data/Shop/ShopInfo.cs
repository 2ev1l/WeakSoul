using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ShopInfo : MonoBehaviour
    {
        #region fields & properties
        public static ShopInfo Instance { get; private set; }
        [field: SerializeField] public Material BadPrice { get; private set; }
        [field: SerializeField] public Material GoodPrice { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        #endregion methods
    }
}