using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.MainMenu;

namespace WeakSoul.GameMenu
{
    public class MainPanelMusicState : MainPanelState
    {
        #region fields & properties
        [SerializeField] private AudioClip clip;
        #endregion fields & properties

        #region methods
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;

            if (clip == null)
                AudioManager.RemoveAmbient();
            else
                AudioManager.AddAmbient(clip);
        }
        #endregion methods
    }
}