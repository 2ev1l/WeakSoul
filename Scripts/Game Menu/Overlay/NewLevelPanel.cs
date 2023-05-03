using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class NewLevelPanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Text rewardText;
        [SerializeField] private LanguageLoader levelText;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            SceneLoader.OnStartLoading += ClosePanel;
            SceneLoader.OnSceneLoaded += ClosePanel;
        }
        private void OnDisable()
        {
            SceneLoader.OnStartLoading -= ClosePanel;
            SceneLoader.OnSceneLoaded -= ClosePanel;
        }
        public void Init(int levelId)
        {
            Level level = LevelsInfo.Instance.GetLevel(levelId - 1);
            levelText.AddText($" ({levelId})");
            rewardText.text = "";
            if (level.Reward.RewardIsZero())
            {
                rewardText.text += $"{GetText(24)}";
                return;
            }
            if (level.Reward.SoulLife > 0)
                rewardText.text += $"+ {level.Reward.SoulLife} {GetText(12).Remove(GetText(12).Length - 1)} \n";
            if (level.Reward.SkillPoints > 0)
                rewardText.text += $"+ {level.Reward.SkillPoints} {GetText(25)} \n";
            if (!level.Reward.Stats.IsStatsZero())
                rewardText.text += $"+ {GetText(0)} (???) \n";
            rewardText.GetComponent<TextOutline>().SetAll();
        }
        private string GetText(int id) => LanguageLoader.GetTextByType(TextType.GameMenu, id);
        public void ClosePanel()
        {
            Destroy(gameObject);
        }
        #endregion methods
    }
}