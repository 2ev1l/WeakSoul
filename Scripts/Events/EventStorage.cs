using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeakSoul.Events
{
    public class EventStorage : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private bool instantiateEffect = true;
        [SerializeField] private List<SpriteData> spritesData;
        [SerializeField] private List<EventEffect> effects;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            if (!instantiateEffect) return;
            TryInstantiateEffect();
        }
        private void TryInstantiateEffect()
        {
            if (!TryGetEffect(out EventEffect effect)) return;
            InstantiateEffect(effect.Prefab, effect.Position);
        }
        public void InstantiateEffect(GameObject prefab, Vector3 position)
        {
            GameObject obj = Instantiate(prefab, position, prefab.transform.rotation);
            obj.SetActive(true);
        }
        private SpriteData GetSpriteData()
        {
            SpriteData data = spritesData.Find(x => x.IsDataAllowed());
            if (data == null)
            {
                print($"No textures for the {EventInfo.Instance.Data.SubZoneData.SubZone} zone in {gameObject.name}. Choosing random...");
                data = spritesData[Random.Range(0, spritesData.Count)];
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>May be null</returns>
        private EventEffect GetEffectData()
        {
            List<EventEffect> data = effects.Where(x => x.IsDataAllowed()).ToList();
            if (data.Count == 0)
                return null;
            return data[Random.Range(0, data.Count)];
        }
        public Sprite GetRandomSprite()
        {
            int rnd = 0;
            SpriteData data = GetSpriteData();
            rnd = Random.Range(0, data.Texutres.Count());
            return data.Texutres.ToList()[rnd];
        }
        public bool TryGetEffect(out EventEffect effect)
        {
            effect = GetEffectData();
            return effect != null;
        }
        #endregion methods
    }
}