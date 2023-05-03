using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
    [System.Serializable]
    public class WitchData
    {
        #region fields & properties
        public UnityAction<IEnumerable<WitchItem>> OnItemsGenerated;
        public IEnumerable<WitchItem> Items => items;
        [SerializeField] private List<WitchItem> items;
        #endregion fields & properties

        #region methods
        public void GenerateItems()
        {
            items = new List<WitchItem>();
            int maxCount = 5;
            int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
            playerLevel -= 5;
            for (int i = EffectsInfo.Instance.Effects.Count - 1; i >= 0; --i)
            {
                if (maxCount <= items.Count) break;
                Effect el = EffectsInfo.Instance.GetEffect(i);
                if (!el.CanBuy || el.Level > playerLevel || CustomMath.GetRandomChance(50)) continue;
                WitchItem item = new(el.Id);
                items.Add(item);
            }

            OnItemsGenerated?.Invoke(Items);
        }
        public void RemoveItem(int id) => items.Remove(items.Find(x => x.Id == id));
        #endregion methods
    }
}