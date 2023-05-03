using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace Data
{
	[System.Serializable]
	public class PlayerStats : EntityStats
	{
		#region fields & properties
		public UnityAction<int> OnSoulLifeChanged;
		public UnityAction<int> OnSkillPointsChanged;
		public UnityAction<int> OnKarmaChanged;
		public UnityAction OnDeadCompletely;
		public UnityAction<Effect> OnEffectAdded;
		public UnityAction<Effect> OnEffectRemoved;

		public override int Health
		{
			get => base.Health;
			set
			{
				if (value <= 0)
				{
					SoulLife -= 1;
				}
				if (SoulLife <= -1)
				{
					health = 0;
					OnHealthChanged?.Invoke(value);
					OnStatsChanged?.Invoke();
					return;
				}
				base.Health = value;
			}
		}
		public int SoulLife
		{
			get => soulLife;
			set => SetSoulLife(value);
		}
		[SerializeField] private int soulLife = 3;
		public int SkillPoints
		{
			get => skillPoints;
			set => SetSkillPoints(value);
		}
		[SerializeField] private int skillPoints = 0;
		public int Karma => karma;
		[SerializeField] private int karma = 0;

		public override int GetDamageDeal(out bool isCrit)
		{
			isCrit = CustomMath.GetRandomChance(CriticalChance);
			if (isCrit)
				OnCrit?.Invoke();
			int damageScaled = Damage;
			if (inventoryLink != null)
			{
				if (inventoryLink.ContainItem(208))
				{
					damageScaled = CustomMath.Multiply(damageScaled, 110);
				}
			}
			return isCrit ? CustomMath.Multiply(damageScaled, CriticalScale) : damageScaled;
		}

		public PlayerClass Class => _class;
		[SerializeField] private PlayerClass _class;
		public IEnumerable<Effect> Effects => effects;
		[SerializeField] private List<Effect> effects = new();
		public TrainingData TrainingData => trainingData;
		[SerializeField] private TrainingData trainingData = new();

		/// <summary>
		/// Need for update changes
		/// </summary>
		[SerializeField] private PhysicalStats EquippedStats = new();
		[NonSerialized] private ItemsInventory inventoryLink;
		#endregion fields & properties

		#region methods
		public void SetPlayerClass(PlayerClass @class) => _class = @class;
		private void SetSoulLife(int value)
		{
			value = Mathf.Max(-1, value);
			soulLife = value;
			OnSoulLifeChanged?.Invoke(value);
			if (value == -1)
				OnDeadCompletely?.Invoke();
			OnStatsChanged?.Invoke();
		}
		private void SetSkillPoints(int value)
		{
			value = Mathf.Max(0, value);
			skillPoints = value;
			OnSkillPointsChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public void ChangeKarmaBy(int value)
		{
			if (Class == PlayerClass.Impartial)
			{
				value /= 2;
			}
			if (inventoryLink != null)
			{
				if (inventoryLink.ContainItem(75))
				{
					value = CustomMath.Multiply(value, 67);
				}
				if (inventoryLink.ContainItem(76))
				{
					value = CustomMath.Multiply(value, 150);
				}
				if (inventoryLink.ContainItem(207))
				{
					value = 0;
				}
			}

			SetKarma(karma + value);
		}
		private void SetKarma(int value)
		{
			karma = value;
			OnKarmaChanged?.Invoke(value);
			OnStatsChanged?.Invoke();
		}
		public bool TryAddOrStackEffect(int effectId)
		{
			Effect effectData = EffectsInfo.Instance.GetEffect(effectId);
			return TryAddOrStackEffect(effectData);
		}
		public bool TryAddOrStackEffect(Effect effectData)
		{
			if (inventoryLink != null)
			{
				bool IsSoulItem_290 = inventoryLink.ContainItem(290);
				if (IsSoulItem_290 && CustomMath.GetRandomChance(50)) return false;
			}
			int effectId = effectData.Id;
			Effect currentEffect = effects.Find(x => x.Id == effectId);
			if (currentEffect == null)
			{
				currentEffect = effectData.Clone();
				if (inventoryLink != null)
				{
					bool IsSoulItem_209 = inventoryLink.ContainItem(209);
					if (IsSoulItem_209)
						currentEffect.DecreaseDuration(currentEffect.Duration / 2);
					bool IsSoulItem_291 = inventoryLink.ContainItem(291);
					if (IsSoulItem_291 && !currentEffect.IsPositive)
						currentEffect.IncreaseDuration(currentEffect.Duration * 9);
				}

				effects.Add(currentEffect);
				IncreaseStats(effectData.Stats);
				OnEffectAdded?.Invoke(effectData);
				return true;
			}
			currentEffect.StackFrom(effectData);
			if (currentEffect.IsStackable)
				IncreaseStats(effectData.Stats);
			OnEffectAdded?.Invoke(effectData);
			return true;
		}
		public void AddOrStackEffects(List<Effect> effectsData)
		{
			foreach (Effect effect in effectsData)
				TryAddOrStackEffect(effect);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="effectId"></param>
		/// <returns>True if removed, False if not</returns>
		public bool TryRemoveEffect(int effectId, bool checkOnDeath)
		{
			Effect effect = effects.Find(x => x.Id == effectId);
			if (!effect.IsDestroyable && effect.Duration > 0) return false;
			RemoveEffect(effect, checkOnDeath);
			return true;
		}
		public void RemoveEffect(Effect effect, bool checkOnDeath)
		{
			effects.Remove(effect);
			if (checkOnDeath)
				DecreaseStats(effect.Stats);
			else
				DecreaseStatsHidden(effect.Stats);
			OnEffectRemoved?.Invoke(effect);
		}
		public void TryRemoveAllEffects(bool checkOnDeath)
		{
			int effectsCount = effects.Count;
			int i = 0;
			while (true)
			{
				try
				{
					if (TryRemoveEffect(effects[i].Id, checkOnDeath)) continue;
					++i;
				}
				catch { break; }
			}
		}
		public void RemoveAllEffects(bool checkOnDeath)
		{
			int effectsCount = effects.Count;
			for (int i = 0; i < effectsCount; ++i)
				RemoveEffect(effects[0], checkOnDeath);
		}
		public void AddAllEffectBuffes()
		{
			foreach (Effect el in effects)
			{
				IncreaseStats(el.Stats);
			}
		}
		public Effect GetEffect(int effectId) => effects.Find(x => x.Id == effectId);
		public PhysicalStats GetEffectsStats()
		{
			PhysicalStats stats = new PhysicalStats();
			foreach (var el in effects)
				stats.IncreaseStatsHidden(el.Stats);
			return stats;
		}
		/// <summary>
		/// Need for update changes
		/// </summary>
		public void Init()
		{
			inventoryLink = GameData.Data.PlayerData.Inventory;
			PhysicalStats newStats = inventoryLink.GetEquippedStats();
			InitSkills();
			newStats.DecreaseStatsHidden(EquippedStats);
			IncreaseStatsHidden(newStats);
		}
		private void InitSkills()
		{
			SkillsInventory skillsInventory = GameData.Data.PlayerData.Skills;
			List<StatsItem> equipped = new() { inventoryLink.BodyArmor, inventoryLink.HeadArmor, inventoryLink.LegsArmor, inventoryLink.Weapon };
			List<int> tempSkills = skillsInventory.TempOpenedSkills.ToList();
			List<int> equippedTempSkills = skillsInventory.Items.ToList();
			for (int i = equippedTempSkills.Count - 1; i >= 0; --i)
			{
				int el = equippedTempSkills[i];
				if (!tempSkills.Contains(el))
					equippedTempSkills[i] = -1;
			}

			GameData.Data.PlayerData.Skills.RemoveAllTempOpenedSkills();
			for (int i = equipped.Count - 1; i >= 0; --i)
			{
				StatsItem item = equipped[i];
				if (item == null || item.SkillId == -1) continue;
				if (!GameData.Data.PlayerData.Skills.TryAddTempOpenedSkill(item.SkillId, true)) continue;
				int index = equippedTempSkills.IndexOf(item.SkillId);
				if (index < 0) continue;
				skillsInventory.SetItem(item.SkillId, index);
			}
		}
		public void StoreLastEquipment()
		{
			EquippedStats = GameData.Data.PlayerData.Inventory.GetEquippedStats();
		}
		public PlayerStats(bool isNull)
		{
			Health = isNull ? 0 : 10;
			Damage = isNull ? 0 : 1;
			Stamina = isNull ? 0 : 1;
			StaminaRegen = isNull ? 0 : 1;
			EvasionChance = isNull ? 0 : 0;
			CriticalScale = isNull ? 0 : 110;
			SoulLife = isNull ? 0 : 0;

			skillPoints = 1;
		}
		public virtual new PlayerStats Clone()
		{
			PlayerStats stats = new(true);
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

			stats.soulLife = soulLife;
			stats.effects = Effects.ToList();
			stats._class = Class;
			stats.EquippedStats = EquippedStats.Clone();
			return stats;
		}
		#endregion methods
	}
}