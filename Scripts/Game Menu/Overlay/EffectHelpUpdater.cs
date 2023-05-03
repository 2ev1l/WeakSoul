using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class EffectHelpUpdater : HelpUpdater
    {
        #region fields & properties
        public static EffectHelpUpdater Instance { get; private set; }
        [SerializeField] private LanguageLoader smallDescriptionText;
        [SerializeField] private LanguageLoader bigDescriptionText;
        [SerializeField] private LanguageLoader itemNameText;
        [SerializeField] private PhysicalStatsItemListEffects itemList;
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            base.Init();
            Instance = this;
        }
        public void OpenPanel(Vector3 position, Effect effect)
        {
            if (effect == null) return;
            base.OpenPanel(position);
            itemList.Effect = effect;
            itemNameText.Id = effect.Id;
            bool isStatsZero = effect.Stats.IsStatsZero();
            bigDescriptionText.gameObject.SetActive(isStatsZero);
            smallDescriptionText.Id = isStatsZero ? -1 : effect.Id;
            bigDescriptionText.Id = effect.Id;
            string addText = $"\n{effect.Duration} s." +
                             $"\n{LanguageLoader.GetTextByType(TextType.GameMenu, effect.IsStackable ? 34 : 35)} x{effect.Stacks}";
            if (isStatsZero)
            {
                smallDescriptionText.enabled = false;
                smallDescriptionText.Text.text = addText;
            }
            else
            {
                smallDescriptionText.enabled = true;
                smallDescriptionText.AddText(addText);
            }
        }
        #endregion methods
    }
}