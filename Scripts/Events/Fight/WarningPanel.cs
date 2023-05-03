using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class WarningPanel : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Image image;
        [SerializeField][ReadOnly] private int startHP;
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            startHP = GameData.Data.PlayerData.Stats.Health;
        }
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.OnHealthChanged += UpdateUI;
            UpdateUI(GameData.Data.PlayerData.Stats.Health);
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.OnHealthChanged -= UpdateUI;
        }
        private void UpdateUI(int hp)
        {
            StartCoroutine(UpdatePanel(hp));
        }
        private IEnumerator UpdatePanel(int hp)
        {
            float finalAlpha = 1f - ((float)hp / startHP);
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(image.color.a, finalAlpha, 1f);
            while (!vsc.IsChangeEnded)
            {
                Color col = image.color;
                col.a = vsc.Out;
                image.color = col;
                yield return CustomMath.WaitAFrame();
            }
            Destroy(vsc);
        }
        #endregion methods
    }
}