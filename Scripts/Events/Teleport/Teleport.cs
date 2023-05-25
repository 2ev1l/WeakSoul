using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Teleport
{
    public class Teleport : MonoBehaviour
    {
        #region fields & properties
        [Header("Animation")]
        [SerializeField] private SpriteRenderer glitchSpriteRenderer;

        [Header("VFX")]
        [SerializeField] private VFXAnimation teleportVFX;
        [SerializeField] private VisualEffect effectPrefab;
        [SerializeField] private Transform spawnCanvas;

        [Header("Debug")]
        [SerializeField][ReadOnly] private int pointId;
        #endregion fields & properties

        #region methods
        public void Init(int pointId) => this.pointId = pointId;
        public void Use() => StartCoroutine(UseTeleport());
        public void Leave() => LoadAdventure();
        private void LoadAdventure(float time = 2f)
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", time);
        }
        private IEnumerator UseTeleport()
        {
            StartCoroutine(DoGlitchEffect());
            
            yield return VFXAnimation.Animate(effectPrefab, spawnCanvas, teleportVFX.VFXs);
            Player.CurrentPointId = pointId;
            LoadAdventure(0.4f);
        }
        private IEnumerator DoGlitchEffect()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 1, 4f);
            while(!vsc.IsChangeEnded)
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