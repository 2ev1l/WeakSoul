using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Blacksmith
{
    public class Blacksmith : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private EventStorage storage;
        [SerializeField] private SpriteRenderer bg;
        [SerializeField] private BuyPanelInit buyPanel;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            bg.sprite = storage.GetRandomSprite();
            buyPanel.Init();
        }
        public void Leave()
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
        }
        #endregion methods
    }
}