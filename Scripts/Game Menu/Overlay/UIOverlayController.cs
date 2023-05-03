using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WeakSoul.GameMenu
{
    public class UIOverlayController : CanvasInit
    {
        #region fields & properties
        [SerializeField] private GameObject mainPanel;
        private static readonly List<string> disallowedSceneNames = new List<string>() { "Main Menu", "Cut Scene" };
        #endregion fields & properties

        #region methods
        public override void Start()
        {
            base.Start();
            mainPanel.SetActive(!disallowedSceneNames.Contains(SceneManager.GetActiveScene().name));
        }
        #endregion methods
    }
}