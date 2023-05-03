using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.CutScene
{
    public class UILoader : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private LanguageLoader textLanguage;
        [SerializeField] private Text timer;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private CutScenesLoader cutScenesLoader;
        [SerializeField] private Animator blackScreenAnimator;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            cutScenesLoader.OnSceneChanged += OnSceneChange;
            cutScenesLoader.OnSecondLasts += StartBlackScreenAnimation;
        }
        private void OnDisable()
        {
            cutScenesLoader.OnSceneChanged -= OnSceneChange;
            cutScenesLoader.OnSecondLasts -= StartBlackScreenAnimation;
        }
        private void OnSceneChange()
        {
            textLanguage.Id = cutScenesLoader.currentScene.TextId;
            background.sprite = cutScenesLoader.currentScene.Background;
        }
        private void StartBlackScreenAnimation(float timeLasts)
        {
            blackScreenAnimator.speed = 1f / timeLasts;
            blackScreenAnimator.Play("Screen-Move");
        }
        private void Update()
        {
            timer.text = $"{cutScenesLoader.TimeLasts:F1} s.";
        }
        #endregion methods
    }
}