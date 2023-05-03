using Data.Adventure;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data.Events
{
    [System.Serializable]
    public class ChestData
    {
        #region fields & properties
        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public int Level => level;
        [Min(0)][SerializeField] private int level;
        public ChestTier Tier => tier;
        [SerializeField] private ChestTier tier;
        public int LanguageId => languageId;
        [Min(0)][SerializeField] private int languageId;
        [Min(0)][SerializeField] private int maxLootCount;
        #endregion fields & properties

        #region methods
        /// <summary>
        /// Doesn't affects anything. You must add all rewards manually after
        /// </summary>
        /// <returns></returns>
        public List<RewardData> GetReward()
        {
            int maxLootCount = this.maxLootCount;
            for (int i = maxLootCount; i > 0; --i)
                if (CustomMath.GetRandomChance(20) && maxLootCount > 0)
                    maxLootCount--;
                else break;
            List<RewardData> result = new();
            for (int i = 0; i < maxLootCount; ++i)
                result.Add(GetRewardByTier(tier));
            return result;
        }
        private RewardData GetRewardByTier(ChestTier tier) => tier switch
        {
            ChestTier.Normal => GetNormalReward(),
            ChestTier.Good => GetGoodReward(),
            ChestTier.Amazing => GetAmazingReward(),
            ChestTier.Bad => GetBadReward(),
            ChestTier.Terrible => GetTerribleReward(),
            ChestTier.Random => GetRandomReward(),
            _ => throw new System.NotImplementedException("Chest Tier"),
        };

        private RewardType GetRandomRewardType() => CustomMath.GetRandomChance() switch
        {
            float i when i <= 25 => RewardType.Item,
            float i when i <= 50 => RewardType.Effect,
            float i when i <= 75 => RewardType.Soul,
            _ => RewardType.Random
        };
        private RewardData GetNormalReward()
        {
            RewardType rewardType = GetRandomRewardType();
            int id = rewardType switch
            {
                RewardType.Item => GetNormalItemReward(),
                RewardType.Soul => GetNormalSoulReward(),
                RewardType.Effect => GetNormalEffectReward(true),
                RewardType.Random => GetNormalRandomReward(),
                _ => throw new System.NotImplementedException("Reward Type"),
            };
            if (id == -1) { id = GetNormalSoulReward(); rewardType = RewardType.Soul; };
            RewardData result = new(rewardType, id, 100, 1);

            return result;
        }
        private int GetNormalItemReward()
        {
            List<Item> allowedItems = ItemsInfo.Instance.Items.Where(x => x.Level <= level).ToList();
            return allowedItems[Random.Range(0, allowedItems.Count)].Id;
        }
        private int GetNormalEffectReward(bool isEffectPositive)
        {
            List<EffectSO> allowedEffects = EffectsInfo.Instance.Effects.Where(x => x.Effect.Level <= level && x.Effect.IsPositive == isEffectPositive).ToList();
            if (allowedEffects.Count == 0)
            {
                allowedEffects = new() { EffectsInfo.Instance.Effects.First() };
                Debug.LogError("Error - Normal effects count is zero; Fixing - Take first");
            }
            return allowedEffects[Random.Range(0, allowedEffects.Count)].Effect.Id;
        }
        private int GetNormalSoulReward()
        {
            return CustomMath.GetRandomChance(70 - 2 * (level + 1)) ? 0 : 1;
        }
        private int GetNormalRandomReward() => CustomMath.GetRandomChance() switch
        {
            float i when i <= 33.3f => GetNormalItemReward(),
            float i when i <= 66.6f => GetNormalEffectReward(true),
            _ => GetNormalSoulReward()
        };

        private RewardData GetGoodReward()
        {
            RewardType rewardType = GetRandomRewardType();
            int id = rewardType switch
            {
                RewardType.Item => GetGoodItemReward(),
                RewardType.Soul => GetGoodSoulReward(),
                RewardType.Effect => GetGoodEffectReward(true),
                RewardType.Random => GetGoodRandomReward(),
                _ => throw new System.NotImplementedException("Reward Type"),
            };
            if (id == -1) { id = GetGoodSoulReward(); rewardType = RewardType.Soul; };
            RewardData result = new(rewardType, id, 100, 1);
            return result;
        }
        private int GetGoodItemReward()
        {
            List<Item> allowedItems = ItemsInfo.Instance.Items.Where(x => Mathf.Abs(x.Level - level) <= 1).ToList();
            if (allowedItems.Count == 0) return GetNormalItemReward();
            return allowedItems[Random.Range(0, allowedItems.Count)].Id;
        }
        private int GetGoodEffectReward(bool isEffectPositive)
        {
            List<EffectSO> allowedEffects = EffectsInfo.Instance.Effects.Where(x => Mathf.Abs(x.Effect.Level - level) <= 1 && x.Effect.IsPositive == isEffectPositive).ToList();
            if (allowedEffects.Count == 0) return GetNormalEffectReward(isEffectPositive);
            return allowedEffects[Random.Range(0, allowedEffects.Count)].Effect.Id;
        }
        private int GetGoodSoulReward()
        {
            return CustomMath.GetRandomChance(80 - 1.5f * (level)) ? 1 : 2;
        }
        private int GetGoodRandomReward() => CustomMath.GetRandomChance() switch
        {
            float i when i <= 33.3f => GetGoodItemReward(),
            float i when i <= 66.6f => GetGoodEffectReward(true),
            _ => GetGoodSoulReward()
        };

        private RewardData GetAmazingReward()
        {
            RewardType rewardType = GetRandomRewardType();
            int id = rewardType switch
            {
                RewardType.Item => GetAmazingItemReward(),
                RewardType.Soul => GetAmazingSoulReward(),
                RewardType.Effect => GetAmazingEffectReward(true),
                RewardType.Random => GetAmazingRandomReward(),
                _ => throw new System.NotImplementedException("Reward Type"),
            };
            if (id == -1) { id = GetAmazingSoulReward(); rewardType = RewardType.Soul; };
            RewardData result = new(rewardType, id, 100, 1);
            return result;
        }
        private int GetAmazingItemReward()
        {
            List<Item> allowedItems = ItemsInfo.Instance.Items.Where(x => (x.Level - level) <= 3 && (x.Level - level) >= 1).ToList();
            if (allowedItems.Count == 0) return GetGoodItemReward();
            return allowedItems[Random.Range(0, allowedItems.Count)].Id;
        }
        private int GetAmazingEffectReward(bool isEffectPositive)
        {
            List<EffectSO> allowedEffects = EffectsInfo.Instance.Effects.Where(x => (x.Effect.Level - level) <= 3 && (x.Effect.Level - level) >= 1 && x.Effect.IsPositive == isEffectPositive).ToList();
            if (allowedEffects.Count == 0) return GetGoodEffectReward(isEffectPositive);
            return allowedEffects[Random.Range(0, allowedEffects.Count)].Effect.Id;
        }
        private int GetAmazingSoulReward()
        {
            return CustomMath.GetRandomChance(50 - 2 * (level)) ? 1 : (CustomMath.GetRandomChance(60) ? 2 : 3);
        }
        private int GetAmazingRandomReward() => CustomMath.GetRandomChance() switch
        {
            float i when i <= 33.3f => GetAmazingItemReward(),
            float i when i <= 66.6f => GetAmazingEffectReward(true),
            _ => GetAmazingSoulReward()
        };

        private RewardData GetBadReward()
        {
            RewardType rewardType = RewardType.Effect;
            int id = CustomMath.GetRandomChance(40) ? GetGoodEffectReward(false) : GetNormalEffectReward(false);
            RewardData result = new(rewardType, id, 100, 1);
            return result;
        }
        private RewardData GetTerribleReward()
        {
            RewardType rewardType = RewardType.Effect;
            int id = CustomMath.GetRandomChance(40) ? GetGoodEffectReward(false) : GetAmazingEffectReward(false);
            RewardData result = new(rewardType, id, 100, 1);
            return result;
        }
        private RewardData GetRandomReward() => CustomMath.GetRandomChance() switch
        {
            float i when i <= 30f => GetNormalReward(),
            float i when i <= 50f => GetGoodReward(),
            float i when i <= 60f => GetAmazingReward(),
            float i when i <= 80f => GetBadReward(),
            float i when i <= 100f => GetTerribleReward(),
            _ => GetNormalReward()
        };
        public ChestData Clone() => new()
        {
            id = id,
            languageId = languageId,
            maxLootCount = maxLootCount,
            tier = tier,
            level = level
        };
        public ChestData(int id, int level, ChestTier tier, int languageId, int maxLootCount)
        {
            this.id = Mathf.Max(id, 0);
            this.level = Mathf.Max(level, 0);
            this.tier = tier;
            this.languageId = Mathf.Max(languageId, 0);
            this.maxLootCount = Mathf.Max(maxLootCount, 0);
        }
        public ChestData() { }
        #endregion methods
    }
}