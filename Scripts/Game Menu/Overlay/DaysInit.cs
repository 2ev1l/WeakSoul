using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.GameMenu
{
    public class DaysInit : MonoBehaviour
    {
        #region fields & properties
        private static int lastDay = -1;
        [SerializeField] private CanvasGroup daysPanel;
        [SerializeField] private LanguageLoader daysText;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            Check();
        }
        private void OnEnable()
        {
            GameData.Data.OnDaysChanged += ShowPanel;
        }
        private void OnDisable()
        {
            GameData.Data.OnDaysChanged -= ShowPanel;
        }
        private void Check()
        {
            int days = GameData.Data.Days;
            if (days == -1)
            {
                GameData.Data.Days = 0;
                lastDay = days;
                SetTutorialParams();
                return;
            }

            bool isPanelShowed = false;
            if (days != lastDay)
            {
                if (lastDay > 0)
                {
                    InvokeDaysUpdate();
                    isPanelShowed = true;
                }
                lastDay = days;
            }
            if (!isPanelShowed)
            {
                ShowPanel(days);
                StartCoroutine(CheckBugs());
            }
        }
        private void SetTutorialParams()
        {
            GameData.Data.ShopData.GenerateTutorialData();
            ShopInit.Instance.LoadItems();
        }
        private void InvokeDaysUpdate() => GameData.Data.OnDaysChanged?.Invoke(GameData.Data.Days);
        private void ShowPanel(int day) => StartCoroutine(PanelShow(day));
        private IEnumerator PanelShow(int day)
        {
            daysText.AddText($" {day}");
            daysPanel.alpha = 1;
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 1, 1);
            while (!vsc.IsChangeEnded)
            {
                daysPanel.alpha = vsc.Out;
                yield return CustomMath.WaitAFrame();
            }
            yield return new WaitForSecondsRealtime(1f);
            vsc.StartChange(1, 0, 1);
            while (!vsc.IsChangeEnded)
            {
                daysPanel.alpha = vsc.Out;
                yield return CustomMath.WaitAFrame();
            }
            daysPanel.alpha = 0;
            Destroy(vsc);
        }
        private IEnumerator CheckBugs()
        {
            yield return new WaitForSecondsRealtime(3.5f);
            if (!TryGetComponent(out ValueSmoothChanger vsc))
                yield break;
            Destroy(vsc);
            daysPanel.alpha = 0;
            StopAllCoroutines();
        }
        #endregion methods
    }
}