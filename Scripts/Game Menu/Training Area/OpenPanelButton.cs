using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class OpenPanelButton : CustomButton
    {
        #region fields & properties
        [SerializeField] private PanelInfo panelInfo;
        [SerializeField] private Image image;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private Sprite lockIcon;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += CheckButtonEnableAbility;
            CheckButtonEnableAbility(GameData.Data.PlayerData.Stats.ExperienceLevel.Level);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= CheckButtonEnableAbility;
        }
        private void CheckButtonEnableAbility(int playerLevel)
        {
            bool active = (playerLevel >= panelInfo.CurrentLevelData.Id && playerLevel >= panelInfo.PlayerStatLevel.Level) || panelInfo.PlayerStatLevel.Level > panelInfo.PlayerStatLevel.MaxLevel;

            spriteRenderer.sprite = active ? defaultIcon : lockIcon;
            image.enabled = active;
        }
        #endregion methods
    }
}