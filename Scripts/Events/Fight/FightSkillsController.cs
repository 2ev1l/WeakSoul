using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Universal;
using Universal.Effects;
using WeakSoul.GameMenu.Skills;
using static UnityEngine.Rendering.DebugUI;

namespace WeakSoul.Events.Fight
{
	public class FightSkillsController : SkillsCellController
	{
		#region fields & properties
		public FightCard Card => card;
		[SerializeField] private FightCard card;
		public Transform AnimationTransform => animationTransform;
		[SerializeField] private Transform animationTransform;
		public VisualEffect EffectPrefab => effectPrefab;
		[SerializeField] private VisualEffect effectPrefab;
		[SerializeField] private SpriteRenderer skillAnimationRenderer;
		[SerializeField] private Material defaultSkillAnimationMaterial;
		protected override SkillsInventory SkillsInventory => card.SkillsInventory;
		#endregion fields & properties

		#region methods
		protected override void OnEnable()
		{
			base.OnEnable();
			card.OnCardInit += CheckEquippedCells;
			card.OnCardInit += UpdateCells;
		}
		protected override void OnDisable()
		{
			base.OnDisable();
			card.OnCardInit -= CheckEquippedCells;
			card.OnCardInit -= UpdateCells;
		}
		public IEnumerator UseSkill(Skill skill)
		{
			card.Stats.Stamina -= skill.StaminaPrice;
			SkillBuff buff = skill.SkillBuff.Clone();
			SkillBuff enemyBuff = skill.SkillEnemyBuff.Clone();

			card.Buffes.TryAddBuff(buff);
			card.OwnEnemy.Buffes.TryAddBuff(enemyBuff);

			List<SkillVFX> updatedVFX = SetSkillAnimationPositions(skill.VFXAnimation.Clone().VFXs);
			List<int> finalDamages = new();
			switch (skill.SkillType)
			{
				case SkillType.Attack:
					bool isEvased = false;
					bool isCrit = false;
					StatScale damageScale = buff.GetStatScale(PhysicalStatsType.Damage, ValueIncrease.Multiply);
					while (damageScale != null)
					{
						buff.RemoveStatScale(damageScale);
						card.Buffes.TryRemoveBuffScale(buff, damageScale);
						finalDamages.Add(GetFinalDamage(skill, damageScale, out bool isCurrentCrit, out bool isCurrentEvased));
						if (isCurrentCrit)
							isCrit = true;
						if (isCurrentEvased)
							isEvased = true;
						damageScale = buff.GetStatScale(PhysicalStatsType.Damage, ValueIncrease.Multiply);
					}
					PrepareAttackVFX(finalDamages.Min(), finalDamages.Max(), updatedVFX, isCrit, isEvased);
					break;
				case SkillType.Block: break;
				case SkillType.Evade: break;
				default: throw new System.NotImplementedException($"Skill Type");
			}
			yield return WaitForSkillAnimation(skill, updatedVFX);
			CheckSkillId(skill, finalDamages);
			CheckEnemyItems(finalDamages);
			switch (skill.SkillType)
			{
				case SkillType.Attack: yield return DoEnemyDamage(finalDamages); break;
				case SkillType.Block: break;
				case SkillType.Evade: break;
				default: throw new System.NotImplementedException($"Skill Type");
			}
			CheckPlayerItems(finalDamages);
		}
		private void CheckSkillId(Skill skill, List<int> finalDamages)
		{
			switch (skill.Id)
			{
				case 43:
					foreach (var damage in finalDamages)
					{
						card.Stats.Health += CustomMath.Multiply(damage, 3);
					}
					break;
				default: break;
			}
		}
		private void CheckEnemyItems(List<int> finalDamages)
		{
			if (card.IsPlayer) return;
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			bool IsSoulItem_201 = playerInventory.ContainItem(201);
			bool IsSoulItem_202 = playerInventory.ContainItem(202);
			bool IsSoulItem_208 = playerInventory.ContainItem(208);
			int count = finalDamages.Count;
			for (int i = 0; i < count; ++i)
			{
				int damage = finalDamages[i];
				if (damage == 0) continue;
				if (IsSoulItem_201)
					finalDamages[i] -= CustomMath.Multiply(damage, 10);
				if (IsSoulItem_202)
					finalDamages[i] += CustomMath.Multiply(damage, 10);
				if (IsSoulItem_208)
					finalDamages[i] += CustomMath.Multiply(damage, 10);
			}
		}
		private void CheckPlayerItems(List<int> finalDamages)
		{
			if (!card.IsPlayer) return;
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			bool IsSoulItem_331 = playerInventory.ContainItem(331);
			bool IsSoulItem_332 = playerInventory.ContainItem(332);
			bool IsSoulItem_333 = playerInventory.ContainItem(333);
			bool IsSoulItem_334 = playerInventory.ContainItem(334);
			EntityStats enemyStats = card.OwnEnemy.Stats;
			foreach (int damage in finalDamages)
			{
				if (damage == 0) continue;
				if (IsSoulItem_331) enemyStats.Health -= CustomMath.Multiply(enemyStats.Health, 4);
				if (IsSoulItem_332) enemyStats.Defense -= CustomMath.Multiply(enemyStats.Defense, 5);
				if (IsSoulItem_333) enemyStats.Resistance -= CustomMath.Multiply(enemyStats.Resistance, 5);
				if (IsSoulItem_334) enemyStats.Damage -= CustomMath.Multiply(enemyStats.Damage, 4);
			}


		}
		private void PrepareAttackVFX(int minDamage, int maxDamage, List<SkillVFX> updatedVFX, bool isCrit, bool isEvased)
		{
			int resultMinDamage = card.OwnEnemy.Stats.GetDamageDealtSelf(minDamage, card.Stats.DamageType);
			int resultMaxDamage = card.OwnEnemy.Stats.GetDamageDealtSelf(maxDamage, card.Stats.DamageType);
			if (!isEvased)
			{
				if (resultMinDamage == 0)
					card.OwnEnemy.Stats.OnDamageBlocked?.Invoke(card.Stats.DamageType);
				SetSkillAnimationProperties(updatedVFX, "DamageDealt", resultMaxDamage);
				SetSkillAnimationProperties(updatedVFX, "DamageDealtCrit", isCrit ? -resultMaxDamage : resultMaxDamage);
			}
			else
			{
				card.OwnEnemy.Stats.OnEvasion?.Invoke();
				SetSkillAnimationProperties(updatedVFX, "DamageDealt", 0);
				SetSkillAnimationProperties(updatedVFX, "DamageDealtCrit", 0);
			}
		}
		private IEnumerator WaitForSkillAnimation(Skill skill, List<SkillVFX> updatedVFX)
		{
			float startTime = Time.time;
			if (skill.VFXAnimation.WaitForIconBurn)
				yield return WaitSpriteSkillAnimation(skill);
			else
				StartCoroutine(WaitSpriteSkillAnimation(skill));
			yield return VFXAnimation.Animate(effectPrefab, animationTransform, updatedVFX);
			float waitedTime = Time.time - startTime;
			if (waitedTime < 1.1f)
				yield return new WaitForSeconds(1.1f - waitedTime);
		}
		private List<SkillVFX> SetSkillAnimationProperties(List<SkillVFX> vfxs, string name, int value)
		{
			vfxs.ForEach(x =>
			{
				if (x.ExposedName.Equals(name))
					x.SetParamValue(value);
			});
			return vfxs;
		}
		private List<SkillVFX> SetSkillAnimationPositions(IEnumerable<SkillVFX> vfxs)
		{
			List<SkillVFX> result = vfxs.ToList();
			result.ForEach(x => x.SetPosition(GetSkillAnimationPosition(x.FightPosition), GetSkillAnimationRect(x.FightPosition)));
			return result;
		}
		public Vector3 GetSkillAnimationPosition(FightPosition fightPosition) => fightPosition switch
		{
			FightPosition.Skill => skillAnimationRenderer.transform.position,
			FightPosition.Center => Vector3.zero,
			FightPosition.Owner => card.transform.position,
			FightPosition.Enemy => card.OwnEnemy.transform.position,
			_ => throw new System.NotImplementedException("Fight Position")
		};
		public static Vector2 GetSkillAnimationRect(FightPosition fightPosition) => fightPosition switch
		{
			FightPosition.Skill => new(0.15f, 0.2f),
			FightPosition.Center => Vector2.one * 0.4f,
			FightPosition.Owner => new(0.65f, 1.1f),
			FightPosition.Enemy => new(0.65f, 1.1f),
			_ => throw new System.NotImplementedException("Fight Position")
		};
		private IEnumerator WaitSpriteSkillAnimation(Skill skill)
		{
			skillAnimationRenderer.gameObject.SetActive(true);
			skillAnimationRenderer.sprite = skill.Texture;
			skillAnimationRenderer.material = defaultSkillAnimationMaterial;
			yield return CustomMath.WaitAFrame();
			ValueSmoothChanger vsc = skillAnimationRenderer.gameObject.AddComponent<ValueSmoothChanger>();
			vsc.StartChange(0, 1, 0.4f);
			while (!vsc.IsChangeEnded)
			{
				Color col = skillAnimationRenderer.color;
				col.a = vsc.Out;
				skillAnimationRenderer.color = col;
				yield return CustomMath.WaitAFrame();
			}
			yield return CustomMath.WaitAFrame();
			Destroy(vsc);
			Burn burnEffect = skillAnimationRenderer.gameObject.AddComponent<Burn>();
			burnEffect.StartAnimation(skill.SkillType == SkillType.Attack ? MaterialsInfo.Instance.Fire_Red : MaterialsInfo.Instance.Fire_Blue, 1.7f);
			burnEffect.DecreaseSpriteAlpha(1.8f);
			while (!burnEffect.IsAnimationEnded)
				yield return CustomMath.WaitAFrame();
			skillAnimationRenderer.gameObject.SetActive(false);
			skillAnimationRenderer.material = defaultSkillAnimationMaterial;
			yield return CustomMath.WaitAFrame();
		}
		/// <summary>
		/// List will be modified with values scaled by scaled damage gained.
		/// </summary>
		/// <param name="finalDamages"></param>
		/// <returns></returns>
		private IEnumerator DoEnemyDamage(List<int> finalDamages)
		{
			int count = finalDamages.Count;
			DamageType damageType = card.Stats.DamageType;
			for (int i = 0; i < count; ++i)
			{
				int gained = card.OwnEnemy.Stats.GetDamageSelf(finalDamages[i], damageType);
				finalDamages[i] = gained;
				yield return new WaitForSecondsRealtime(0.2f);
			}
		}
		private int GetFinalDamage(Skill skill, StatScale damageStatScale, out bool isCrit, out bool isEvased)
		{
			#region BaseModifiers
			isEvased = false;
			FightCard enemy = card.OwnEnemy;
			int finalDamage = card.Stats.GetDamageDeal(out isCrit);
			if (skill.SkillBuff.DamageTypeScaler == card.Stats.DamageType)
				finalDamage += damageStatScale.IncreaseType switch
				{
					ValueIncrease.Multiply => CustomMath.GetMultipliedIncrease(finalDamage, damageStatScale.Multiplier),
					ValueIncrease.Additive => Mathf.RoundToInt(damageStatScale.Multiplier),
					_ => throw new System.NotImplementedException("Value increase type")
				};
			else
				finalDamage = CustomMath.Multiply(finalDamage, 30);
			#endregion BaseModifiers

			#region AnotherModifiers
			if (card.IsPlayer)
			{
				PlayerData playerData = GameData.Data.PlayerData;
				ItemsInventory itemsInventory = playerData.Inventory;
				bool IsSoulItem_292 = itemsInventory.ContainItem(292);
				if (IsSoulItem_292 && playerData.Stats.DamageType == DamageType.Magical)
					finalDamage = CustomMath.Multiply(finalDamage, 110);

				bool IsSoulItem_293 = itemsInventory.ContainItem(293);
				if (IsSoulItem_293 && playerData.Stats.DamageType == DamageType.Soul)
					finalDamage = CustomMath.Multiply(finalDamage, 105);

				bool IsSoulItem_294 = itemsInventory.ContainItem(294);
				if (IsSoulItem_294 && playerData.Stats.DamageType == DamageType.Physical)
					finalDamage = CustomMath.Multiply(finalDamage, 110);
			}
			#endregion AnotherModifiers

			#region Evasion
			if (enemy.Buffes.IsActivatorApplied(x => x.ActivateEvasion == true))
			{
				isEvased = enemy.Stats.IsEvased;
				finalDamage = isEvased ? 0 : finalDamage;
			}
			#endregion Evasion

			return finalDamage;
		}
		[ContextMenu("Reset Stamina")]
		private void ResetStamina() => card.Stats.Stamina = card.Stats.StaminaRegen;
		#endregion methods
	}
}