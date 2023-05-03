using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.GameMenu.Shop;

namespace WeakSoul.Events.Shop
{
    public class Shop : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private EventStorage storage;
        [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
        [SerializeField] private ShopInit shopInit;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Sprite rnd = storage.GetRandomSprite();
            bgSpriteRenderers.ForEach(x => x.sprite = rnd);
            shopInit.Init(EventInfo.Instance.Data.ShopData);
        }
        public void Leave()
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
        }
        #endregion methods
    }
}