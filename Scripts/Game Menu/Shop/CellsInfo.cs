using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Shop
{
    public class CellsInfo : SingleSceneInstance
    {
        #region fields & properties
        public static CellsInfo Instance { get; private set; }
        public IEnumerable<ShopCellInfo> Infos => infos;
        [SerializeField] private List<ShopCellInfo> infos;
        #endregion fields & properties

        #region methods
        public ShopCellInfo GetInfo(int id) => infos[id];

        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}