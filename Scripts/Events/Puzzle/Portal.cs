using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Universal;

namespace WeakSoul.Events.Puzzle
{
    public class Portal : MonoBehaviour
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> - cutSceneId;
        /// </summary>
        public static UnityAction<int> OnPortalUsed;
        [SerializeField] private GameObject closeButton;
        [SerializeField] private LanguageLoader closeText;

        [Header("Animation")]
        [SerializeField] private SpriteRenderer glitchSpriteRenderer;

        [Header("VFX")]
        [SerializeField] private VFXAnimation VFX;
        [SerializeField] private VisualEffect effectPrefab;
        [SerializeField] private Transform spawnCanvas;
        [SerializeField] private AudioClip cutSceneMusic;
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            bool isCloseAvailable = IsCloseAvailable();
            closeButton.SetActive(isCloseAvailable);
            closeText.Id = isCloseAvailable ? 17 : 16;
        }
        private bool IsCloseAvailable()
        {
            PlayerData playerData = GameData.Data.PlayerData;
            List<int> bossSkills = new() { 9, 20, 28, 38, 46 };
            foreach (var el in bossSkills)
                if (!playerData.Skills.ContainItem(el))
                    return false;
            return true;
        }
        public void ClosePortal()
        {
            int karma = GameData.Data.PlayerData.Stats.Karma;
            PlayerClass playerClass = GameData.Data.PlayerData.Stats.Class;
            int cutSceneId = (karma) switch
            {
                int i when i < -150 => 4,
                int i when i < 0 => 3,
                int i when i < 150 => 5,
                _ => 6
            };
            switch (playerClass)
            {
                case PlayerClass.Omnivorous:
                    if (karma < 100) cutSceneId = 7;
                    break;
                case PlayerClass.Stoic:
                    cutSceneId = karma < 50 ? 8 : 9;
                    break;
                case PlayerClass.PosthumousHero:
                    if (karma > 100) cutSceneId = 10;
                    break;
            }
            OnPortalUsed?.Invoke(cutSceneId);
            StartCoroutine(AnimateUsing(cutSceneId));
            AudioManager.PlayMusic(cutSceneMusic, true);
            SavingUtils.ResetTotalProgress(false);
        }
        private void LoadCutScene(int id) => SceneLoader.Instance.LoadCutSceneFade(2, id);
        public void Leave()
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", 2);
        }
        private IEnumerator AnimateUsing(int cutSceneId)
        {
            StartCoroutine(DoGlitchEffect());
            yield return VFXAnimation.Animate(effectPrefab, spawnCanvas, VFX.VFXs);
            LoadCutScene(cutSceneId);
        }
        private IEnumerator DoGlitchEffect()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 1, 4f);
            while (!vsc.IsChangeEnded)
            {
                Color col = glitchSpriteRenderer.color;
                col.a = vsc.Out;
                glitchSpriteRenderer.color = col;
                yield return CustomMath.WaitAFrame();
            }
            Destroy(vsc);
        }
        #endregion methods
    }
}