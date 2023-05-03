using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class PhysicalStats
	{
		#region fields & properties
		public UnityAction OnStatsChanged;
		public UnityAction<PhysicalStats> OnStatsIncreased;
		public UnityAction<int> OnHealthChanged;
		public UnityAction<int> OnDamageChanged;
		public UnityAction<DamageType> OnDamageTypeChanged;
		public UnityAction<int> OnDefenseChanged;
		public UnityAction<int> OnResistanceChanged;
		public UnityAction<int> OnStaminaChanged;
		public UnityAction<int> OnStaminaRegenChanged;
		public UnityAction<int> OnEvasionChanceChanged;
		public UnityAction<int> OnCriticalChanceChanged;
		public UnityAction<int> OnCriticalScaleChanged;

		public virtual int Health
		{
			get => health;
			set => SetHealth(value);
		}
		[SerializeField] protected int health = 0;
		public int Damage
		{
			get => damage;
			set => SetDamage(value);
		}
		[SerializeField] private int damage = 0;
		public DamageType DamageType
		{
			get => damageType;
			set => SetDamageType(value);
		}
		[SerializeField] private DamageType damageType = DamageType.Physical;
		public int Defense
		{
			get => defense;
			set => SetDefense(value);
		}
		[SerializeField] private int defense = 0;
		public int Resistance
		{
			get => resistance;
			set => SetResistance(value);
		}
		[SerializeField] private int resistance = 0;
		public int Stamina
		{
			get => stamina;
			set => SetStamina(value);
		}
		[SerializeField] private int stamina = 0;
		public int StaminaRegen
		{
			get => staminaRegen;
			set => SetStaminaRegen(value);
		}
		[SerializeField] private int staminaRegen = 0;
		public int EvasionChance
		{
			get => evasionChance;
			set => SetEvasionChance(value);
		}
		[SerializeField] private int evasionChance = 0;
		public int CriticalChance
		{
			get => criticalChance;
			set => SetCriticalChance(value);
		}
		[SerializeField] private int criticalChance = 0;
		public int CriticalScale
		{
			get => criticalScale;
			set => SetCriticalScale(value);
		}
		[SerializeField] private int criticalScale = 0;

		[System.NonSerialized]
		private static readonly List<PhysicalStatsType> statsList = new List<PhysicalStatsType>()
		{
			PhysicalStatsType.Health,
			PhysicalStatsType.Damage,
			PhysicalStatsType.DamageType,
			PhysicalStatsType.Defense,
			PhysicalStatsType.Resistance,
			PhysicalStatsType.Stamina,
			PhysicalStatsType.StaminaRegen,
			PhysicalStatsType.EvasionChance,
			PhysicalStatsType.CriticalChance,
			PhysicalStatsType.CriticalScale,
		};
		#endregion fields & properties

		#region methods
		public virtual void SetHealth(int value)
		{
			health = value;
			OnHealthChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetDamage(int value)
		{
			if (damage == value) return;
			damage = value;
			OnDamageChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetDamageType(DamageType value)
		{
			if (damageType == value)
				return;
			damageType = value;
			OnDamageTypeChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetDefense(int value)
		{
			if (defense == value)
				return;
			defense = value;
			OnDefenseChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetResistance(int value)
		{
			if (resistance == value)
				return;
			resistance = value;
			OnResistanceChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetStamina(int value)
		{
			if (stamina == value)
				return;
			stamina = value;
			OnStaminaChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetStaminaRegen(int value)
		{
			if (staminaRegen == value)
				return;
			staminaRegen = value;
			OnStaminaRegenChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetEvasionChance(int value)
		{
			if (evasionChance == value)
				return;
			evasionChance = value;
			OnEvasionChanceChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetCriticalChance(int value)
		{
			if (criticalChance == value)
				return;
			criticalChance = value;
			OnCriticalChanceChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public virtual void SetCriticalScale(int value)
		{
			if (criticalScale == value)
				return;
			criticalScale = value;
			OnCriticalScaleChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}

		public int GetStatsByType(PhysicalStatsType statsType) => statsType switch
		{
			PhysicalStatsType.Health => Health,
			PhysicalStatsType.Damage => Damage,
			PhysicalStatsType.DamageType => (int)DamageType,
			PhysicalStatsType.Defense => Defense,
			PhysicalStatsType.Resistance => Resistance,
			PhysicalStatsType.Stamina => Stamina,
			PhysicalStatsType.StaminaRegen => StaminaRegen,
			PhysicalStatsType.EvasionChance => EvasionChance,
			PhysicalStatsType.CriticalChance => CriticalChance,
			PhysicalStatsType.CriticalScale => CriticalScale,
			_ => throw new System.NotImplementedException()
		};
		public string GetStatsTextByType(PhysicalStatsType statsType) => statsType switch
		{
			PhysicalStatsType.Health => Health.ToString(),
			PhysicalStatsType.Damage => Damage.ToString(),
			PhysicalStatsType.DamageType => GetDamageTypeText(),
			PhysicalStatsType.Defense => Defense.ToString(),
			PhysicalStatsType.Resistance => Resistance.ToString(),
			PhysicalStatsType.Stamina => Stamina.ToString(),
			PhysicalStatsType.StaminaRegen => StaminaRegen.ToString(),
			PhysicalStatsType.EvasionChance => EvasionChance.ToString() + "%",
			PhysicalStatsType.CriticalChance => CriticalChance.ToString() + "%",
			PhysicalStatsType.CriticalScale => CriticalScale.ToString() + "%",
			_ => throw new System.NotImplementedException()
		};
		public string GetDamageTypeText() => DamageType switch
		{
			DamageType.Physical => LanguageLoader.GetTextByType(TextType.GameMenu, 13),
			DamageType.Magical => LanguageLoader.GetTextByType(TextType.GameMenu, 14),
			DamageType.Soul => LanguageLoader.GetTextByType(TextType.GameMenu, 15),
			_ => throw new NotImplementedException()
		};
		public static int GetStatsLanguageIdByType(PhysicalStatsType statsType) => statsType switch
		{
			PhysicalStatsType.Health => 2,
			PhysicalStatsType.Damage => 3,
			PhysicalStatsType.DamageType => 4,
			PhysicalStatsType.Defense => 5,
			PhysicalStatsType.Resistance => 6,
			PhysicalStatsType.Stamina => 7,
			PhysicalStatsType.StaminaRegen => 8,
			PhysicalStatsType.EvasionChance => 9,
			PhysicalStatsType.CriticalChance => 10,
			PhysicalStatsType.CriticalScale => 11,
			_ => throw new System.NotImplementedException()
		};
		public bool IsStatsZero() => Health == 0 && Damage == 0 && Defense == 0 && Resistance == 0 && Stamina == 0 && StaminaRegen == 0 && EvasionChance == 0 && CriticalChance == 0 && CriticalScale == 0;

		public virtual List<PhysicalStatsType> GetStatsList() => statsList;
		public virtual List<PhysicalStatsType> GetEnabledStatsList()
		{
			List<PhysicalStatsType> stats = new List<PhysicalStatsType>();
			foreach (var el in statsList)
			{
				if (GetStatsByType(el) == 0) continue;
				stats.Add(el);
			}
			return stats;
		}
		public virtual List<PhysicalStatsType> GetEnabledStatsListInherit(ItemType itemType)
		{
			List<PhysicalStatsType> stats = GetEnabledStatsList();
			if (itemType == ItemType.Weapon && !stats.Contains(PhysicalStatsType.DamageType))
				stats.Add(PhysicalStatsType.DamageType);
			return stats;
		}

		/// <summary>
		/// Doesn't check subvalues e.g. <see cref="EntityStats.OnDead"/>, but still affects UI.
		/// Faster than <see cref="IncreaseStats"/>
		/// </summary>
		/// <param name="addingStats"></param>
		/// <returns></returns>
		public PhysicalStats IncreaseStatsHidden(PhysicalStats addingStats)
		{
			health += addingStats.Health;
			damage += addingStats.Damage;
			defense += addingStats.Defense;
			resistance += addingStats.Resistance;
			stamina += addingStats.Stamina;
			staminaRegen += addingStats.StaminaRegen;
			evasionChance += addingStats.EvasionChance;
			criticalChance += addingStats.CriticalChance;
			criticalScale += addingStats.CriticalScale;
			OnStatsChanged?.Invoke();
			OnStatsIncreased?.Invoke(addingStats);
			return this;
		}
		public PhysicalStats IncreaseStats(PhysicalStats addingStats)
		{
			if (addingStats.Damage != 0) Damage += addingStats.Damage;
			if (addingStats.Defense != 0) Defense += addingStats.Defense;
			if (addingStats.Resistance != 0) Resistance += addingStats.Resistance;
			if (addingStats.Stamina != 0) Stamina += addingStats.Stamina;
			if (addingStats.StaminaRegen != 0) StaminaRegen += addingStats.StaminaRegen;
			if (addingStats.EvasionChance != 0) EvasionChance += addingStats.EvasionChance;
			if (addingStats.CriticalChance != 0) CriticalChance += addingStats.CriticalChance;
			if (addingStats.CriticalScale != 0) CriticalScale += addingStats.CriticalScale;
			if (addingStats.Health != 0) Health += addingStats.Health;
			OnStatsChanged?.Invoke();
			OnStatsIncreased?.Invoke(addingStats);
			return this;
		}

		/// <summary>
		/// Doesn't check subvalues e.g. <see cref="EntityStats.OnDead"/>, but still affects UI.
		/// Faster than <see cref="DecreaseStats"/>
		/// </summary>
		/// <param name="decreasingStats"></param>
		/// <returns></returns>
		public PhysicalStats DecreaseStatsHidden(PhysicalStats decreasingStats)
		{
			health -= decreasingStats.Health;
			damage -= decreasingStats.Damage;
			defense -= decreasingStats.Defense;
			resistance -= decreasingStats.Resistance;
			stamina -= decreasingStats.Stamina;
			staminaRegen -= decreasingStats.StaminaRegen;
			evasionChance -= decreasingStats.EvasionChance;
			criticalChance -= decreasingStats.CriticalChance;
			criticalScale -= decreasingStats.CriticalScale;
			OnStatsChanged?.Invoke();
			return this;
		}
		public PhysicalStats DecreaseStats(PhysicalStats decreasingStats)
		{
			if (decreasingStats.Damage != 0) Damage -= decreasingStats.Damage;
			if (decreasingStats.Defense != 0) Defense -= decreasingStats.Defense;
			if (decreasingStats.Resistance != 0) Resistance -= decreasingStats.Resistance;
			if (decreasingStats.Stamina != 0) Stamina -= decreasingStats.Stamina;
			if (decreasingStats.StaminaRegen != 0) StaminaRegen -= decreasingStats.StaminaRegen;
			if (decreasingStats.EvasionChance != 0) EvasionChance -= decreasingStats.EvasionChance;
			if (decreasingStats.CriticalChance != 0) CriticalChance -= decreasingStats.CriticalChance;
			if (decreasingStats.CriticalScale != 0) CriticalScale -= decreasingStats.CriticalScale;
			if (decreasingStats.Health != 0) Health -= decreasingStats.Health;
			OnStatsChanged?.Invoke();
			return this;
		}
		public PhysicalStats IncreaseStatsByType(PhysicalStatsType type, int Value)
		{
			switch (type)
			{
				case PhysicalStatsType.Health: Health += Value; break;
				case PhysicalStatsType.Damage: Damage += Value; break;
				case PhysicalStatsType.Defense: Defense += Value; break;
				case PhysicalStatsType.Resistance: Resistance += Value; break;
				case PhysicalStatsType.Stamina: Stamina += Value; break;
				case PhysicalStatsType.StaminaRegen: StaminaRegen += Value; break;
				case PhysicalStatsType.EvasionChance: EvasionChance += Value; break;
				case PhysicalStatsType.CriticalChance: CriticalChance += Value; break;
				case PhysicalStatsType.CriticalScale: CriticalScale += Value; break;
				default: throw new System.NotImplementedException();
			}
			return this;
		}
		public PhysicalStats DecreaseStatsByType(PhysicalStatsType type, int Value)
		{
			switch (type)
			{
				case PhysicalStatsType.Health: Health -= Value; break;
				case PhysicalStatsType.Damage: Damage -= Value; break;
				case PhysicalStatsType.Defense: Defense -= Value; break;
				case PhysicalStatsType.Resistance: Resistance -= Value; break;
				case PhysicalStatsType.Stamina: Stamina -= Value; break;
				case PhysicalStatsType.StaminaRegen: StaminaRegen -= Value; break;
				case PhysicalStatsType.EvasionChance: EvasionChance -= Value; break;
				case PhysicalStatsType.CriticalChance: CriticalChance -= Value; break;
				case PhysicalStatsType.CriticalScale: CriticalScale -= Value; break;
				default: throw new System.NotImplementedException();
			}
			return this;
		}

		/// <summary>
		/// All stats will be set to 0. Doesn't trigger anything.
		/// </summary>
		public void ResetStats()
		{
			health = 0;
			damage = 0;
			defense = 0;
			resistance = 0;
			stamina = 0;
			staminaRegen = 0;
			evasionChance = 0;
			criticalChance = 0;
			criticalScale = 0;
		}
		public virtual PhysicalStats Clone()
		{
			PhysicalStats stats = new();
			stats.health = Health;
			stats.damage = Damage;
			stats.damageType = DamageType;
			stats.defense = Defense;
			stats.resistance = Resistance;
			stats.stamina = Stamina;
			stats.staminaRegen = StaminaRegen;
			stats.evasionChance = EvasionChance;
			stats.criticalChance = CriticalChance;
			stats.criticalScale = CriticalScale;
			return stats;
		}
		#endregion methods
	}
}