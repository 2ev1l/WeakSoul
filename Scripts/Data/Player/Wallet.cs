using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class Wallet
    {
        #region fields & properties
        public UnityAction<SoulType> OnSoulsChanged;

        public int WeakSouls
        {
            get => weakSouls;
            set => SetSoulsByType(value, SoulType.Weak);
        }
        [SerializeField] private int weakSouls = 0;
        public int NormalSouls
        {
            get => normalSouls;
            set => SetSoulsByType(value, SoulType.Normal);
        }
        [SerializeField] private int normalSouls = 0;
        public int StrongSouls
        {
            get => strongSouls;
            set => SetSoulsByType(value, SoulType.Strong);
        }
        [SerializeField] private int strongSouls = 0;
        public int UniqueSouls
        {
            get => uniqueSouls;
            set => SetSoulsByType(value, SoulType.Unique);
        }
        [SerializeField] private int uniqueSouls = 0;
        public int LegendarySouls
        {
            get => legendarySouls;
            set => SetSoulsByType(value, SoulType.Legendary);
        }
        [SerializeField] private int legendarySouls = 0;
        #endregion fields & properties

        #region methods
        public void SetSoulsByType(int value, SoulType soulType)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException("souls");
            switch (soulType)
            {
                case SoulType.Weak: weakSouls = value; break;
                case SoulType.Normal: normalSouls = value; break;
                case SoulType.Strong: strongSouls = value; break;
                case SoulType.Unique: uniqueSouls = value; break;
                case SoulType.Legendary: legendarySouls = value; break;
                default: throw new System.NotImplementedException();
            }
            OnSoulsChanged?.Invoke(soulType);
        }
        public bool IsWalletZero() => WeakSouls == 0 && normalSouls == 0 && strongSouls == 0 && uniqueSouls == 0 && legendarySouls == 0;
        public int GetSoulsByType(SoulType soulType) => soulType switch
        {
            SoulType.Weak => weakSouls,
            SoulType.Normal => normalSouls,
            SoulType.Strong => strongSouls,
            SoulType.Unique => uniqueSouls,
            SoulType.Legendary => legendarySouls,
            _ => throw new System.NotImplementedException()
        };
        public void DecreaseValues(Wallet wallet)
        {
            WeakSouls -= wallet.WeakSouls;
            NormalSouls -= wallet.NormalSouls;
            StrongSouls -= wallet.StrongSouls;
            UniqueSouls -= wallet.UniqueSouls;
            LegendarySouls -= wallet.LegendarySouls;
        }
        public void IncreaseValues(Wallet wallet)
        {
            WeakSouls += wallet.WeakSouls;
            NormalSouls += wallet.NormalSouls;
            StrongSouls += wallet.StrongSouls;
            UniqueSouls += wallet.UniqueSouls;
            LegendarySouls += wallet.LegendarySouls;
        }
        public bool CanBuyThis()
        {
            Wallet playerWallet = GameData.Data.PlayerData.Wallet;
            return playerWallet.WeakSouls >= WeakSouls &&
                    playerWallet.NormalSouls >= NormalSouls &&
                    playerWallet.StrongSouls >= StrongSouls &&
                    playerWallet.UniqueSouls >= UniqueSouls &&
                    playerWallet.LegendarySouls >= LegendarySouls;
        }
        public Wallet Clone()
        {
            Wallet wallet = new Wallet();
            wallet.weakSouls = weakSouls;
            wallet.normalSouls = normalSouls;
            wallet.strongSouls = strongSouls;
            wallet.uniqueSouls = uniqueSouls;
            wallet.legendarySouls = legendarySouls;
            return wallet;
        }
        #endregion methods
    }
}