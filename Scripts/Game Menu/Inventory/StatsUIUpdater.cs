using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Inventory
{
    public class StatsUIUpdater : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Text HealthText;
        [SerializeField] private Text DamageText;
        [SerializeField] private LanguageLoader DamageTypeLanguage;
        [SerializeField] private Text DefenseText;
        [SerializeField] private Text ResistanceText;
        [SerializeField] private Text StaminaText;
        [SerializeField] private Text StaminaRegenText;
        [SerializeField] private Text EvasionChanceText;
        [SerializeField] private Text CriticalChanceText;
        [SerializeField] private Text CriticalScaleText;
        [SerializeField] private Text SoulLifeText;
        [SerializeField] private InventoryCellController inventory;

        private bool isDifferenceShown = false;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color badColor;
        [SerializeField] private Color goodColor;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            PlayerStats stats = GameData.Data.PlayerData.Stats;
            inventory.OnCellPreviewStart += ShowStatsDifference;
            inventory.OnCellEquipped += SetStatsBack;
            inventory.OnCellPreviewEnd += SetStatsBack;
            stats.OnHealthChanged += UpdateHealth;
            stats.OnDamageChanged += UpdateDamage;
            stats.OnDamageTypeChanged += UpdateDamageType;
            stats.OnDefenseChanged += UpdateDefense;
            stats.OnResistanceChanged += UpdateResistance;
            stats.OnStaminaChanged += UpdateStamina;
            stats.OnStaminaRegenChanged += UpdateStaminaRegen;
            stats.OnEvasionChanceChanged += UpdateEvasionChance;
            stats.OnCriticalChanceChanged += UpdateCriticalChance;
            stats.OnCriticalScaleChanged += UpdateCriticalScale;
            stats.OnSoulLifeChanged += UpdateSoulLife;
            UpdateAll();
        }
        private void OnDisable()
        {
            PlayerStats stats = GameData.Data.PlayerData.Stats;
            inventory.OnCellPreviewStart -= ShowStatsDifference;
            inventory.OnCellEquipped -= SetStatsBack;
            inventory.OnCellPreviewEnd -= SetStatsBack;
            stats.OnHealthChanged -= UpdateHealth;
            stats.OnDamageChanged -= UpdateDamage;
            stats.OnDamageTypeChanged -= UpdateDamageType;
            stats.OnDefenseChanged -= UpdateDefense;
            stats.OnResistanceChanged -= UpdateResistance;
            stats.OnStaminaChanged -= UpdateStamina;
            stats.OnStaminaRegenChanged -= UpdateStaminaRegen;
            stats.OnEvasionChanceChanged -= UpdateEvasionChance;
            stats.OnCriticalChanceChanged -= UpdateCriticalChance;
            stats.OnCriticalScaleChanged -= UpdateCriticalScale;
            stats.OnSoulLifeChanged -= UpdateSoulLife;
        }

        private void ShowStatsDifference(int cellIndex, int itemId, int startCellIndex)
        {
            if (!ItemsInventory.EquipmentCells.Contains(startCellIndex) && !ItemsInventory.EquipmentCells.Contains(cellIndex))
                return;
            Armor armor = ItemsInfo.Instance.TryGetArmor(itemId);
            Weapon weapon = ItemsInfo.Instance.TryGetWeapon(itemId);
            switch (cellIndex)
            {
                case 16:
                    if (armor == null || armor.ArmorType != ArmorType.Head) return;
                    ShowStatsDifference(armor, true, false); return;
                case 17:
                    if (weapon == null || !weapon.IsPlayerClassAllowed()) return;
                    ShowStatsDifference(weapon, true, true); return;
                case 18:
                    if (armor == null || armor.ArmorType != ArmorType.Body) return;
                    ShowStatsDifference(armor, true, false); return;
                case 19:
                    if (armor == null || armor.ArmorType != ArmorType.Legs) return;
                    ShowStatsDifference(armor, true, false); return;
                default:
                    if (ItemsInventory.EquipmentCells.Contains(startCellIndex))
                    {
                        if (armor != null)
                            ShowStatsDifference(armor, false, false);
                        if (weapon != null)
                            ShowStatsDifference(weapon, false, true);
                        return;
                    }
                    break;
            }
        }
        private void ShowStatsDifference(StatsItem statsItem, bool increase, bool isWeapon)
        {
            ShowStatsDifference(statsItem.Stats, increase, isWeapon);
            isDifferenceShown = true;
        }
        private void ShowStatsDifference(PhysicalStats stats, bool increase, bool isWeapon)
        {
            if (stats.Health > 0)
                AddIntValue(HealthText, stats.Health, stats.Health > 0 && increase);
            if (stats.Health < 0)
                AddIntValue(HealthText, stats.Health, stats.Health < 0 && !increase);

            if (stats.Damage > 0)
                AddIntValue(DamageText, stats.Damage, stats.Damage > 0 && increase);
            if (stats.Damage < 0)
                AddIntValue(DamageText, stats.Damage, stats.Damage < 0 && !increase);

            if (stats.Defense > 0)
                AddIntValue(DefenseText, stats.Defense, stats.Defense > 0 && increase);
            if (stats.Defense < 0)
                AddIntValue(DefenseText, stats.Defense, stats.Defense < 0 && !increase);

            if (isWeapon)
                UpdateDamageType(increase ? stats.DamageType : DamageType.Physical);

            if (stats.Resistance > 0)
                AddIntValue(ResistanceText, stats.Resistance, stats.Resistance > 0 && increase);
            if (stats.Resistance < 0)
                AddIntValue(ResistanceText, stats.Resistance, stats.Resistance < 0 && !increase);

            if (stats.Stamina > 0)
                AddIntValue(StaminaText, stats.Stamina, stats.Stamina > 0 && increase);
            if (stats.Stamina < 0)
                AddIntValue(StaminaText, stats.Stamina, stats.Stamina < 0 && !increase);

            if (stats.StaminaRegen > 0)
                AddIntValue(StaminaRegenText, stats.StaminaRegen, stats.StaminaRegen > 0 && increase);
            if (stats.StaminaRegen < 0)
                AddIntValue(StaminaRegenText, stats.StaminaRegen, stats.StaminaRegen < 0 && !increase);

            if (stats.EvasionChance > 0)
                AddPercentValue(EvasionChanceText, stats.EvasionChance, stats.EvasionChance > 0 && increase);
            if (stats.EvasionChance < 0)
                AddPercentValue(EvasionChanceText, stats.EvasionChance, stats.EvasionChance < 0 && !increase);

            if (stats.CriticalChance > 0)
                AddPercentValue(CriticalChanceText, stats.CriticalChance, stats.CriticalChance > 0 && increase);
            if (stats.CriticalChance < 0)
                AddPercentValue(CriticalChanceText, stats.CriticalChance, stats.CriticalChance < 0 && !increase);

            if (stats.CriticalScale > 0)
                AddPercentValue(CriticalScaleText, stats.CriticalScale, stats.CriticalScale > 0 && increase);
            if (stats.CriticalScale < 0)
                AddPercentValue(CriticalScaleText, stats.CriticalScale, stats.CriticalScale < 0 && !increase);
        }
        private void SetStatsBack()
        {
            if (!isDifferenceShown)
                return;
            UpdateAll();
            isDifferenceShown = true;
        }
        private void SetStatsBack(int cellId) => SetStatsBack();

        private void UpdateAll()
        {
            PlayerStats stats = GameData.Data.PlayerData.Stats;
            UpdateHealth(stats.Health);
            UpdateDamage(stats.Damage);
            UpdateDamageType(stats.DamageType);
            UpdateDefense(stats.Defense);
            UpdateResistance(stats.Resistance);
            UpdateStamina(stats.Stamina);
            UpdateStaminaRegen(stats.StaminaRegen);
            UpdateEvasionChance(stats.EvasionChance);
            UpdateCriticalChance(stats.CriticalChance);
            UpdateCriticalScale(stats.CriticalScale);
            UpdateSoulLife(stats.SoulLife);
        }
        private void UpdateHealth(int value) => UpdateIntText(HealthText, value);
        private void UpdateDamage(int value) => UpdateIntText(DamageText, value);
        private void UpdateDamageType(DamageType value) => UpdateDamageType(DamageTypeLanguage, value);
        private void UpdateDefense(int value) => UpdateIntText(DefenseText, value);
        private void UpdateResistance(int value) => UpdateIntText(ResistanceText, value);
        private void UpdateStamina(int value) => UpdateIntText(StaminaText, value);
        private void UpdateStaminaRegen(int value) => UpdateIntText(StaminaRegenText, value);
        private void UpdateEvasionChance(int value) => UpdatePercentText(EvasionChanceText, value);
        private void UpdateCriticalChance(int value) => UpdatePercentText(CriticalChanceText, value);
        private void UpdateCriticalScale(int value) => UpdatePercentText(CriticalScaleText, value);
        private void UpdateSoulLife(int value) => UpdateIntText(SoulLifeText, value);

        private void UpdateIntText(Text txt, int value)
        {
            txt.color = defaultColor;
            txt.text = value.ToString();
        }
        private void UpdatePercentText(Text txt, int value)
        {
            txt.color = defaultColor;
            txt.text = $"{value}%";
        }
        private void UpdateDamageType(LanguageLoader language, DamageType damageType) => language.Id = damageType switch
        {
            DamageType.Physical => 13,
            DamageType.Magical => 14,
            DamageType.Soul => 15,
            _ => throw new System.NotImplementedException()
        };

        private void AddIntValue(Text txt, int value, bool isGreater)
        {
            value = Mathf.Abs(value);
            txt.color = isGreater ? goodColor : badColor;
            txt.text += isGreater ? $"+{value}" : $"-{value}";
        }
        private void AddPercentValue(Text txt, int value, bool isGreater)
        {
            value = Mathf.Abs(value);
            txt.color = isGreater ? goodColor : badColor;
            txt.text += isGreater ? $"+{value}%" : $"-{value}%";
        }
        #endregion methods
    }
}