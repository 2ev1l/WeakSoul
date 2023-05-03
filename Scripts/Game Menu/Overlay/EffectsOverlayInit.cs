using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.GameMenu
{
    public class EffectsOverlayInit : CanvasInit
    {
        #region fields & properties
        public static EffectsOverlayInit Instance { get; private set; }
        
        [SerializeField] private GameObject panel;
        [SerializeField] private ItemList effectsOverlay;
        #endregion fields & properties

        #region methods
        public override void Init()
        {
            base.Init();
            Instance = this;
        }
        public override void Start()
        {
            base.Start();
            panel.SetActive(!PlayerEffectsController.DisabledScenes.Contains(SceneManager.GetActiveScene().name));
            if (panel.activeSelf)
                effectsOverlay.UpdateListData();
        }
        #endregion methods
    }
}