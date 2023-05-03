using Data;
using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Fight
{
    public class EnemyCardUI : FightCardUI
    {
        #region fields & properties
        public override Sprite Texture => enemyCard.EnemyData.Texture;
        [Header("EnemyCardUI")]
        [SerializeField] private EnemyCard enemyCard;
        [SerializeField] private LanguageLoader nameLanguage;
        [SerializeField] private LanguageLoader descriptionLanguage;
        [SerializeField] private GameObject doorNext;
        [SerializeField] private ParticleSystem rewardParticles;
        [SerializeField] private Transform spawnCanvas;
        
        [Header("Soul Items UI")]
        [SerializeField] private List<SpriteRenderer> skillsPanelSRenderes; 
        [SerializeField] private List<Image> skillsPanelImages;
        [SerializeField] private GameObject statsPanel; 
        #endregion fields & properties

        #region methods
        protected override void OnEnableInitialized()
        {
            base.OnEnableInitialized();
            enemyCard.OnRewardAdded += DoRewardParticles;
        }
        protected override void OnDisableInitialized()
        {
            base.OnDisableInitialized();
            enemyCard.OnRewardAdded -= DoRewardParticles;
        }
        protected override void UpdateUI()
        {
            base.UpdateUI();
            nameLanguage.Id = enemyCard.EnemyData.Id;
            descriptionLanguage.Id = enemyCard.EnemyData.Id;

            bool isSoulItem_Skills =  GameData.Data.PlayerData.Inventory.ContainItem(8);
            bool isSoulItem_Stats = GameData.Data.PlayerData.Inventory.ContainItem(74);
            Color nullCol = new(0, 0, 0, 0);
            if (!isSoulItem_Skills)
            {
                skillsPanelSRenderes.ForEach(x => x.color = nullCol);
                skillsPanelImages.ForEach(x => x.color = nullCol);
            }
            statsPanel.SetActive(isSoulItem_Stats);
        }
        protected override void AfterDeadAnimation()
        {
            doorNext.SetActive(true);
            base.AfterDeadAnimation();
        }
        private void DoRewardParticles(List<RewardData> rewards)
        {
            Vector3 pos = transform.GetChild(0).position;
            foreach (var el in rewards)
            {
                BurstParticleSystem(InstantiateParticleSystem(), el, pos);
            }
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
        #endregion methods
    }
}