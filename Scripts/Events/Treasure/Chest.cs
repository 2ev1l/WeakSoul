using Data;
using Data.Adventure;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Universal;

namespace WeakSoul.Events.Treasure
{
    public class Chest : MonoBehaviour
    {
        #region fields & properties
        [Header("UI")]
        [SerializeField] private LanguageLoader descriptionLanguage;
        [SerializeField] private Transform spawnCanvas;
        [SerializeField] private ParticleSystem rewardParticles;
        [SerializeField] private SpriteRenderer glitchSprtieRenderer;

        [Header("VFX")]
        [SerializeField] private VisualEffect vfxPrefab;
        [SerializeField] private VFXAnimation openChestVFX;
        [SerializeField] private VFXAnimation badRewardVFX;
        [SerializeField] private AudioClip openClip;

        [Header("Debug")]
        [SerializeField][ReadOnly] private ChestData data;
        #endregion fields & properties

        #region methods
        public void Init(ChestData data)
        {
            this.data = data;
            descriptionLanguage.Id = data.LanguageId;
        }
        public void OpenChest() => StartCoroutine(ChestOpen());
        private IEnumerator ChestOpen()
        {
            List<RewardData> rewards = data.GetReward();
            List<RewardData> gainRewards = new();
            foreach (var el in rewards)
            {
                try
                {
                    el.TryAddReward(out RewardData reward);
                    gainRewards.Add(reward);
                }
                catch { Debug.LogError($"Error reward with {el.Id}-{el.Type}"); }
            }
            StartCoroutine(DoGlitch(0, 1));
            Invoke(nameof(PlayClip), 0.7f);
            yield return VFXAnimation.Animate(vfxPrefab, spawnCanvas, openChestVFX.VFXs);
            DoRewardParticles(gainRewards);
            if (gainRewards.Count == 0 || data.Tier == ChestTier.Bad || data.Tier == ChestTier.Terrible)
                yield return VFXAnimation.Animate(vfxPrefab, spawnCanvas, badRewardVFX.VFXs);
            Invoke(nameof(LoadAdventure), 2f);
        }
        private void PlayClip() => AudioManager.PlayClip(openClip, Universal.AudioType.Sound);
        private IEnumerator DoGlitch(float startValue, float endValue)
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(startValue, endValue, 1);
            while (!vsc.IsChangeEnded)
            {
                Color col = glitchSprtieRenderer.color;
                col.a = vsc.Out;
                glitchSprtieRenderer.color = col;
                yield return CustomMath.WaitAFrame();
            }
            yield return CustomMath.WaitAFrame();
            Destroy(vsc);
        }
        private void DoRewardParticles(List<RewardData> rewards)
        {
            Vector3 pos = transform.position;
            foreach (var el in rewards)
                BurstParticleSystem(InstantiateParticleSystem(), el, pos);
        }
        private ParticleSystem InstantiateParticleSystem()
        {
            ParticleSystem particles = Instantiate(rewardParticles, Vector3.zero, Quaternion.identity, spawnCanvas);
            Vector3 pos = particles.transform.position;
            pos.z = 0;
            particles.transform.position = pos;
            pos = particles.transform.localPosition;
            pos.z = 0;
            particles.transform.localPosition = pos;
            return particles;
        }
        private void BurstParticleSystem(ParticleSystem particles, RewardData reward, Vector3 position)
        {
            particles.textureSheetAnimation.SetSprite(0, reward.GetRewardSprite());
            for (int i = reward.Count; i > 0; i--)
                CustomAnimation.BurstParticlesAt(position, particles);
        }
        public void LoadAdventure()
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
        }
        #endregion methods
    }
}