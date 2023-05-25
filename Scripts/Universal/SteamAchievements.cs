using Data;
using Data.Events;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WeakSoul.Adventure.Map;
using WeakSoul.Events.Fight;
using WeakSoul.Events.Puzzle;
using WeakSoul.GameMenu;
using WeakSoul.GameMenu.Blacksmith;
using WeakSoul.GameMenu.Shop;
using WeakSoul.GameMenu.TrainingArea;

namespace Universal
{
    public class SteamAchievements : MonoBehaviour
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            ClassChoose.OnClassChoosed += OnClassChoosed;
            Portal.OnPortalUsed += OnPortalUsed;
            SellPanel.OnItemSold += OnItemSold;
            ShopItem.OnItemBought += OnItemBought;
            Forge.OnForgeStart += OnForgeStart;
            GameData.Data.BlacksmithData.OnCraftsCountChanged += OnCraftsCountChanged;
            GameData.Data.BlacksmithData.OnRecipeOpened += OnRecipeOpened;
            GameData.Data.PlayerData.Inventory.OnSizeChanged += OnInventorySizeChanged;
            GameData.Data.PlayerData.Skills.OnSizeChanged += OnSkillsSizeChanged;
            GameData.Data.PlayerData.Stats.OnEffectAdded += OnEffectAdded;
            GameData.Data.PlayerData.Stats.OnDead += OnPlayerDead;
            GameData.Data.PlayerData.Stats.OnDeadCompletely += OnPlayerDeadCompletely;
            GameData.Data.PlayerData.Stats.OnHealthChanged += OnPlayerHealthChanged;
            GameData.Data.PlayerData.Stats.OnKarmaChanged += OnPlayerKarmaChanged;
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += OnPlayerLevelChanged;
            GameData.Data.PlayerData.Skills.OnSkillOpened += OnPlayerSkillOpened;
            GameData.Data.PlayerData.Skills.OnInventoryChanged += OnPlayerSkillsChanged;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged += OnPlayerItemsChanged;
            GameData.Data.OnDaysChanged += OnDaysChanged;
            MiniGame.OnStatsGained += OnTrainingLevelGained;
            Player.OnPointStep += OnPointStep;
            KarmaReward.OnRewardGained += OnKarmaRewardGained;
            WeakSoul.Adventure.Backpack.SoulItem.OnSoulItemUsed += OnSoulItemUsed;
            WeakSoul.Adventure.Backpack.SoulItem.OnSoulItemDestroyed += OnSoulItemDestroyed;
            EventData.OnEventLoading += OnEventLoading;
            PlayerCard.OnPlayerEscaped += OnPlayerEscapedFromBattle;
            WeakSoul.Events.SceneInit.OnLeave += OnLeaveFromEvent;
            EnemyCard.OnEnemyDefeated += OnEnemyDefeated;
            Card.OnCardChoosed += OnCardChoosed;
        }
        private void OnDisable()
        {
            ClassChoose.OnClassChoosed -= OnClassChoosed;
            Portal.OnPortalUsed -= OnPortalUsed;
            SellPanel.OnItemSold -= OnItemSold;
            ShopItem.OnItemBought -= OnItemBought;
            Forge.OnForgeStart -= OnForgeStart;
            GameData.Data.BlacksmithData.OnCraftsCountChanged -= OnCraftsCountChanged;
            GameData.Data.BlacksmithData.OnRecipeOpened -= OnRecipeOpened;
            GameData.Data.PlayerData.Inventory.OnSizeChanged -= OnInventorySizeChanged;
            GameData.Data.PlayerData.Skills.OnSizeChanged -= OnSkillsSizeChanged;
            GameData.Data.PlayerData.Stats.OnEffectAdded -= OnEffectAdded;
            GameData.Data.PlayerData.Stats.OnDead -= OnPlayerDead;
            GameData.Data.PlayerData.Stats.OnDeadCompletely -= OnPlayerDeadCompletely;
            GameData.Data.PlayerData.Stats.OnHealthChanged -= OnPlayerHealthChanged;
            GameData.Data.PlayerData.Stats.OnKarmaChanged -= OnPlayerKarmaChanged;
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= OnPlayerLevelChanged;
            GameData.Data.PlayerData.Skills.OnSkillOpened -= OnPlayerSkillOpened;
            GameData.Data.PlayerData.Skills.OnInventoryChanged -= OnPlayerSkillsChanged;
            GameData.Data.PlayerData.Inventory.OnInventoryChanged -= OnPlayerItemsChanged;
            GameData.Data.OnDaysChanged -= OnDaysChanged;
            MiniGame.OnStatsGained -= OnTrainingLevelGained;
            Player.OnPointStep -= OnPointStep;
            KarmaReward.OnRewardGained -= OnKarmaRewardGained;
            WeakSoul.Adventure.Backpack.SoulItem.OnSoulItemUsed -= OnSoulItemUsed;
            WeakSoul.Adventure.Backpack.SoulItem.OnSoulItemDestroyed -= OnSoulItemDestroyed;
            EventData.OnEventLoading -= OnEventLoading;
            PlayerCard.OnPlayerEscaped -= OnPlayerEscapedFromBattle;
            WeakSoul.Events.SceneInit.OnLeave -= OnLeaveFromEvent;
            EnemyCard.OnEnemyDefeated -= OnEnemyDefeated;
            Card.OnCardChoosed -= OnCardChoosed;
        }
        private void OnCardChoosed(Card card)
        {
            int choosedCount = CardsPanel.ChoosedCards.Count;
            SetAchievement("ACH_GET_CHOOSE_COUNT_1");
            if (choosedCount >= 10)
                SetAchievement("ACH_GET_CHOOSE_COUNT_10");
            if (choosedCount >= 50)
                SetAchievement("ACH_GET_CHOOSE_COUNT_50");
            if (choosedCount >= 100)
                SetAchievement("ACH_GET_CHOOSE_COUNT_100");
            SetAchievement($"ACH_GET_CHOOSE_{card.Data.Id}");
        }
        private void OnEnemyDefeated(int enemyId)
        {
            SetAchievement("ACH_DEFEAT_ANY");
            SetAchievement($"ACH_DEFEAT_{enemyId}");
        }
        private void OnLeaveFromEvent()
        {
            SetAchievement("ACH_EVENT_LEAVE");
        }
        private void OnPlayerEscapedFromBattle()
        {
            SetAchievement("ACH_EVENT_ESCAPE_BATTLE");
        }
        private void OnEventLoading(MapEvent mapEvent)
        {
            switch (mapEvent.EventType)
            {
                case Data.Events.EventType.Fight:
                    if (mapEvent.Id == 4)
                        SetAchievement("ACH_FIND_UNIQUE");
                    if (mapEvent.Id == 2)
                        SetAchievement("ACH_FIND_DUNGEON");
                    break;
                case Data.Events.EventType.Shop: break;
                case Data.Events.EventType.Loot: SetAchievement("ACH_FIND_TREASURE"); break; 
                case Data.Events.EventType.Blacksmith:   break;
                case Data.Events.EventType.Teleport: SetAchievement("ACH_FIND_TELEPORT"); break;
                case Data.Events.EventType.Camp: SetAchievement("ACH_FIND_CAMP"); break;
                case Data.Events.EventType.Puzzle: break;
                default: break;
            }
        }
        private void OnSoulItemDestroyed(int itemId)
        {
            SetAchievement("ACH_BREAK_SOULITEM_ANY");
        }
        private void OnSoulItemUsed(int itemId)
        {
            SetAchievement("ACH_USE_SOULITEM_ANY");
        }
        private void OnPlayerItemsChanged(int itemId, int newCellId, int oldCellId)
        {
            ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
            List<int> filledItems = playerInventory.GetFilledItems();
            if (itemId == 205)
                SetAchievement("ACH_GET_ITEM_205");
            if (itemId == 206)
                SetAchievement("ACH_GET_ITEM_206");
            if (filledItems.Contains(212) && filledItems.Contains(213))
                SetAchievement("ACH_GET_ITEM_212_213");
            int soulItemsCount = 0;
            foreach (var el in filledItems)
                if (ItemsInfo.Instance.TryGetSoulItem(el) != null)
                    soulItemsCount++;
            if (soulItemsCount >= 5)
                SetAchievement("ACH_GET_SOULITEMS_COUNT_5");
            if (soulItemsCount >= 10)
                SetAchievement("ACH_GET_SOULITEMS_COUNT_10");
            if (soulItemsCount >= 15)
                SetAchievement("ACH_GET_SOULITEMS_COUNT_15");
        }
        private void OnPlayerSkillsChanged(int skillId, int newCellId, int oldCellId)
        {
            SkillsInventory playerSkills = GameData.Data.PlayerData.Skills;
            List<int> filledItems = playerSkills.GetFilledItems();
            if (filledItems.Contains(41) && filledItems.Contains(42))
                SetAchievement("ACH_GET_SKILL_41_42");
            List<SkillType> skillsType = new();
            foreach (var el in filledItems)
                skillsType.Add(SkillsInfo.Instance.GetSkill(el).SkillType);
            if (skillsType.Where(x => x == SkillType.Attack).Count() > 2)
                SetAchievement("ACH_SKILLS_EQUIP_ATTACK_2H");
        }
        private void OnPlayerSkillOpened(int skillId)
        {
            int skillsCount = GameData.Data.PlayerData.Skills.OpenedSkills.Count();
            if (skillsCount >= 10)
                SetAchievement("ACH_GET_SKILLS_COUNT_10");
            if (skillsCount >= 10)
                SetAchievement("ACH_GET_SKILLS_COUNT_20");
        }
        private void OnKarmaRewardGained()
        {
            int karma = GameData.Data.PlayerData.Stats.Karma;
            SetAchievement(karma < 0 ? "ACH_GET_KARMA_REWARD_N" : "ACH_GET_KARMA_REWARD_P");
        }
        private void OnPlayerKarmaChanged(int value)
        {
            if (value < -100)
                SetAchievement("ACH_GET_KARMA_100L");
            if (value < -200)
                SetAchievement("ACH_GET_KARMA_200L");
            if (value > 100)
                SetAchievement("ACH_GET_KARMA_100H");
            if (value > 200)
                SetAchievement("ACH_GET_KARMA_200H");

        }
        private void OnPointStep(int pointId)
        {
            if (GameData.Data.PlayerData.Stats.ExperienceLevel.Level < 4) return;
            PointData pointData = PointsInit.Instance.GetPointData(pointId);
            if (pointData.ChoosedEvent.Id == 12)
                SetAchievement("ACH_ADVENTURE_STEP_ON_12");
        }
        private void OnDaysChanged(int value)
        {
            if (value >= 10)
                SetAchievement("ACH_GET_DAYS_10");
            if (value >= 50)
                SetAchievement("ACH_GET_DAYS_50");
            if (value >= 100)
                SetAchievement("ACH_GET_DAYS_100");
        }
        private void OnPlayerLevelChanged(int level)
        {
            if (level >= 20)
                SetAchievement("ACH_GET_LEVEL_20");
            if (level >= 30)
                SetAchievement("ACH_GET_LEVEL_30");
            if (level >= 40)
                SetAchievement("ACH_GET_LEVEL_40");
            if (level >= 50)
                SetAchievement("ACH_GET_LEVEL_50");
            if (level >= 53)
                SetAchievement("ACH_GET_LEVEL_53");
        }
        private void OnTrainingLevelGained(PhysicalStatsType type)
        {
            StatExperienceLevel level = GameData.Data.PlayerData.Stats.TrainingData.GetLevelByType(type);
            if (level.MaxLevel < level.Level)
                SetAchievement("ACH_TRAINING_ANY_LAST");

            if (type == PhysicalStatsType.Health)
                OnPlayerHealthChanged(GameData.Data.PlayerData.Stats.Health);
        }
        private void OnPlayerHealthChanged(int value)
        {
            string scene = SceneManager.GetActiveScene().name;
            if (scene.Equals("Game Menu"))
            {
                if (value >= 1500)
                    SetAchievement("ACH_GET_HEALTH_RAW_1500");
            }
        }
        private void OnPlayerDeadCompletely()
        {
            SetAchievement("ACH_DIE_FOREVER");
        }
        private void OnPlayerDead()
        {
            SetAchievement("ACH_DIE_COUNT_1");
        }
        private void OnEffectAdded(Effect effect)
        {
            int effectsCount = GameData.Data.PlayerData.Stats.Effects.Count();
            if (effectsCount >= 5)
                SetAchievement("ACH_GET_EFFECT_COUNT_5");
            if (effectsCount >= 10)
                SetAchievement("ACH_GET_EFFECT_COUNT_10");
            SetAchievement(effect.IsPositive ? "ACH_GET_EFFECT_POSITIVE" : "ACH_GET_EFFECT_NEGATIVE");
        }
        private void OnSkillsSizeChanged(int size)
        {
            if (size >= 6)
                SetAchievement("ACH_GET_SKILLS_COUNT_6");
        }
        private void OnInventorySizeChanged(int size)
        {
            if (size >= 16)
                SetAchievement("ACH_GET_INVENTORY_COUNT_16");
        }
        private void OnRecipeOpened(int recipeId)
        {
            int recipesCount = GameData.Data.BlacksmithData.OpenedRecipes.Count();
            if (recipesCount >= 10)
                SetAchievement("ACH_CRAFT_FIND_COUNT_10");
            if (recipesCount >= 50)
                SetAchievement("ACH_CRAFT_FIND_COUNT_50");
            if (recipesCount >= 100)
                SetAchievement("ACH_CRAFT_FIND_COUNT_100");
            if (recipesCount >= 150)
                SetAchievement("ACH_CRAFT_FIND_COUNT_150");
        }
        private void OnCraftsCountChanged(int count)
        {
            SetAchievement("ACH_CRAFT_ANY");
            if (count >= 10)
                SetAchievement("ACH_CRAFT_COUNT_10");
            if (count >= 50)
                SetAchievement("ACH_CRAFT_COUNT_50");
            if (count >= 100)
                SetAchievement("ACH_CRAFT_COUNT_100");
        }
        private void OnForgeStart(int itemId)
        {
            switch (itemId)
            {
                default: break;
            }
        }
        private void OnItemBought(int itemId, ShopItemType type)
        {
            SetAchievement("ACH_BUY_ANY");
        }
        private void OnItemSold(int itemId)
        {
            if (itemId == 3)
                SetAchievement("ACH_SELL_ITEM_3");
        }
        private void OnPortalUsed(int cutSceneId)
        {
            SetAchievement("ACH_COMPLETE_GAME");
            switch (GameData.Data.PlayerData.Stats.Class)
            {
                case PlayerClass.Omnivorous: SetAchievement("ACH_COMPLETE_GAME_O"); break;
                case PlayerClass.Impartial: SetAchievement("ACH_COMPLETE_GAME_I"); break;
                case PlayerClass.HaterOfEvil: SetAchievement("ACH_COMPLETE_GAME_H"); break;
                case PlayerClass.Stoic: SetAchievement("ACH_COMPLETE_GAME_S"); break;
                case PlayerClass.PosthumousHero: SetAchievement("ACH_COMPLETE_GAME_P"); break;
                default: throw new System.NotImplementedException("Player class for achievement");
            }
            switch (cutSceneId)
            {
                case 3: SetAchievement("ACH_COMPLETE_GAME_1"); break;
                case 4: SetAchievement("ACH_COMPLETE_GAME_2"); break;
                case 5: SetAchievement("ACH_COMPLETE_GAME_3"); break;
                case 6: SetAchievement("ACH_COMPLETE_GAME_4"); break;
                case 7: SetAchievement("ACH_COMPLETE_GAME_5"); break;
                case 8: SetAchievement("ACH_COMPLETE_GAME_6"); break;
                case 9: SetAchievement("ACH_COMPLETE_GAME_7"); break;
                case 10: SetAchievement("ACH_COMPLETE_GAME_8"); break;
                default: break;
            }
        }
        private void OnClassChoosed(ClassChoose classChoose) => SetAchievement("ACH_CLASS_CHOOSE");
        public static void SetAchievement(string name)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
        [ContextMenu("Set achievement")]
        private void AchSet() => SetAchievement("ACH_CLASS_CHOOSE");
        #endregion methods
    }
}