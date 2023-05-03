using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ShopCellInfo
    {
        #region fields & properties
        public int Id => id;
        [SerializeField] private int id;
        public Sprite Texture => texture;
        [SerializeField] private Sprite texture;
        [SerializeField] private int startSize;
        [SerializeField] private List<ShopCellValue> values;
        #endregion fields & properties

        #region methods
        public ShopCellValue GetLastAllowedValue()
        {
            int finalSize = startSize;
            int level = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            int currentSize = id switch
            {
                0 => GameData.Data.PlayerData.Inventory.Size,
                1 => GameData.Data.PlayerData.Skills.Size,
                _ => throw new System.NotImplementedException()
            };
            for (int i = 0; i < values.Count; ++i)
            {
                if (values[i].Level > level) return null;
                if (finalSize >= currentSize) return values[i];
                finalSize += values[i].Value;
            }
            throw new System.NotImplementedException();
        }
        #endregion methods
    }
}