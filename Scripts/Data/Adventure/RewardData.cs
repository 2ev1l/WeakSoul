using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu;

namespace Data.Adventure
{
    [System.Serializable]
    public class RewardData
    {
        #region fields & properties
        public RewardType Type => type;
        [SerializeField] private RewardType type;
        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public float Chance => chance;
        [Range(0f, 100f)][SerializeField] private float chance = 0;
        public int Count => count;
        [Min(0)][SerializeField] private int count = 1;
        #endregion fields & properties

        #region methods
        public bool TryAddReward(out RewardData reward, float additionalRewardChance)
        {
            reward = Clone();
            if (!CustomMath.GetRandomChance(additionalRewardChance)) return false;
            return TryAddReward(out reward);
        }
        public bool TryAddReward(out RewardData reward)
        {
            reward = Clone();
            reward.count = 0;
            bool isRewardAdded = false;
            for (int i = 0; i < count; i++)
            {
                if (!CustomMath.GetRandomChance(chance)) continue;
                isRewardAdded = true;
                reward.count++;
                switch (type)
                {
                    case RewardType.Item: AddItemReward(); break;
                    case RewardType.Soul: AddSoulReward(); break;
                    case RewardType.Effect: AddEffectReward(); break;
                    case RewardType.Random: AddRandomReward(out reward); break;
                    default: throw new System.NotImplementedException();
                }
            }
            return isRewardAdded;
        }
        /// <summary>
        /// Don't use on <see cref="RewardType.Random"/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Sprite GetRewardSprite() => (type) switch
        {
            RewardType.Item => ItemsInfo.Instance.GetItem(id).Texture,
            RewardType.Soul => SoulsInfo.Instance.GetInfo((SoulType)id).Sprite,
            RewardType.Effect => EffectsInfo.Instance.GetEffect(id).Sprite,
            _ => throw new System.NotImplementedException()
        };
        private void AddItemReward()
        {
            PlayerData data = GameData.Data.PlayerData;
            int freeCell = data.Inventory.GetFreeCell();
            if (freeCell == -1) return;
            data.Inventory.SetItem(id, freeCell);
        }
        private void AddSoulReward()
        {
            PlayerData data = GameData.Data.PlayerData;
            SoulType soulType = (SoulType)id;
            data.Wallet.SetSoulsByType(data.Wallet.GetSoulsByType(soulType) + 1, soulType);
        }
        private void AddEffectReward()
        {
            PlayerData data = GameData.Data.PlayerData;
            data.Stats.TryAddOrStackEffect(id);
        }
        private void AddRandomReward(out RewardData reward)
        {
            PlayerData data = GameData.Data.PlayerData;
            reward = new();
            reward.type = (RewardType)Random.Range(0, (int)RewardType.Random);
            reward.chance = 100;
            switch (reward.type)
            {
                case RewardType.Item:
                    List<Item> items = new();
                    ItemsInfo.Instance.Items.ForEach(x =>
                    {
                        if (x.Level <= data.Stats.ExperienceLevel.Level)
                            items.Add(x);
                    });
                    int index = Random.Range(0, items.Count);
                    reward.id = items[index].Id;
                    break;
                case RewardType.Soul:
                    reward.id = Random.Range(0, (int)SoulType.Unique);
                    if (reward.id == 0)
                        reward.count = CustomMath.GetRandomChance(70) ? Random.Range(1, 4) : Random.Range(1, 5);
                    if (reward.id == 1)
                        reward.count = CustomMath.GetRandomChance(30) ? Random.Range(1, 3) : 1;
                    break;
                case RewardType.Effect:
                    reward.id = Random.Range(0, EffectsInfo.Instance.Effects.Count);
                    reward.count = CustomMath.GetRandomChance(50) ? Random.Range(1, 3) : 1;
                    break;
            }
            reward.TryAddReward(out reward);
        }
        public RewardData Clone() => new()
        {
            type = this.type,
            id = this.id,
            chance = this.chance,
            count = this.count,
        };
        public RewardData() { }
        public RewardData(RewardType type, int id, float chance, int count)
        {
            this.type = type;
            this.id = id;
            this.chance = chance;
            this.count = count;
        }
        #endregion methods
    }
}