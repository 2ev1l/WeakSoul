using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul
{
    public class CanvasInit : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Canvas mainCanvas;
        #endregion fields & properties

        #region methods
        public virtual void Init()
        {
            
        }
        public virtual void Start()
        {
            mainCanvas.worldCamera = Camera.main;
        }
        #endregion methods
    }
}