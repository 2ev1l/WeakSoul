using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu
{
    public class SoulsItem : MonoBehaviour, IListUpdater
    {
        #region fields & properties
        public GameObject rootObject => gameObject;
        public int listParam => (int)soulInfo.SoulType;
        private SoulInfo soulInfo;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Text txt;
        protected virtual Wallet wallet => Wallet;
        public Wallet Wallet { get; set; } = new Wallet();
        #endregion fields & properties

        #region methods
        public void OnListUpdate(int param)
        {
            soulInfo = SoulsInfo.Instance.Souls.Find(x => x.SoulType == (Data.SoulType)param);
            int value = wallet.GetSoulsByType(soulInfo.SoulType);
            spriteRenderer.sprite = soulInfo.Sprite;
            txt.text = value > 0 ? $"{value}" : "";
            spriteRenderer.material = value > 0 ? SoulsInfo.Instance.NormalMaterial : SoulsInfo.Instance.NullMaterial;
        }
        #endregion methods

    }
}