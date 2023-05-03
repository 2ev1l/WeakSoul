using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
    [System.Serializable]
    public class EntityStats : PhysicalStats
    {
        #region fields & properties
        public UnityAction<DamageType> OnDamageBlocked;
        /// <summary>
        /// <see cref="{T0}"/> Damage Gained (>0)
        /// </summary>
        public UnityAction<int> OnDamageGained;
        public UnityAction OnEvasion;
        public UnityAction OnDead;
        public UnityAction OnCrit;

        public bool IsEvased => CustomMath.GetRandomChance(EvasionChance);
        public bool IsDead => Health <= 0;
        public int MaxStamina => StaminaRegen * 3;
        public ExperienceLevel ExperienceLevel => experienceLevel;
        [SerializeField] protected ExperienceLevel experienceLevel = new();
        #endregion fields & properties

        #region methods
        public virtual int GetDamageDeal(out bool isCrit)
        {
            isCrit = CustomMath.GetRandomChance(CriticalChance);
            if (isCrit)
                OnCrit?.Invoke();
            return isCrit ? CustomMath.Multiply(Damage, CriticalScale) : Damage;
        }
        private int GetDamageDealt(int damage, DamageType damageType)
        {
            int dealt = damageType switch
            {
                DamageType.Physical => damage - Defense,
                DamageType.Magical => damage - Resistance,
                DamageType.Soul => damage,
                _ => throw new System.NotImplementedException()
            };
            return Mathf.Max(0, dealt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        /// <returns>What damage will be on <see cref="GetDamageSelf(int, DamageType)"/></returns>
        public int GetDamageDealtSelf(int damage, DamageType damageType)
        {
            int dealt = GetDamageDealt(damage, damageType);
            dealt = Mathf.Min(Health, dealt);
            return dealt;
        }
        /// <summary>
        /// Deals damage to Health. Affected by <see cref="PhysicalStats.defense"/> and <see cref="PhysicalStats.resistance"/>
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        /// <returns>Damage gained</returns>
        public int GetDamageSelf(int damage, DamageType damageType)
        {
            int dealt = GetDamageDealtSelf(damage, damageType);
            return GetUnscaledDamageSelf(dealt, damageType);
        }
        /// <summary>
        /// Deals damage to Health. Doesn't affected by anything
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType">Used for UI only</param>
        /// <returns></returns>
        public int GetUnscaledDamageSelf(int damage, DamageType damageType)
        {
            if (damage > 0)
            {
                Health -= damage;
                OnDamageGained?.Invoke(damage);
            }
            else if (Health <= 0) OnDead?.Invoke();
            return damage;
        }
        /// <summary>
        /// Increase stamina by <see cref="PhysicalStats.StaminaRegen"/> value, with limit of <see cref="MaxStamina"/>
        /// </summary>
        public void ResetStamina() => Stamina = Mathf.Clamp(Stamina + StaminaRegen, 0, MaxStamina);
        public override void SetHealth(int value)
        {
            base.SetHealth(value);
            if (Health <= 0)
                OnDead?.Invoke();
        }
        public virtual new EntityStats Clone()
        {
            EntityStats stats = new();
            stats.Health = Health;
            stats.Damage = Damage;
            stats.DamageType = DamageType;
            stats.Defense = Defense;
            stats.Resistance = Resistance;
            stats.Stamina = Stamina;
            stats.StaminaRegen = StaminaRegen;
            stats.EvasionChance = EvasionChance;
            stats.CriticalChance = CriticalChance;
            stats.CriticalScale = CriticalScale;
            stats.experienceLevel = ExperienceLevel.Clone();
            return stats;
        }
        #endregion methods
    }
}