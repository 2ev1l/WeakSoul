using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.CutScene
{
    [System.Serializable]
    public class CutSceneInfo
    {
        #region fields & properties
        [field: SerializeField] public int TextId { get; private set; }
        [field: SerializeField] public Sprite Background { get; private set; }
        #endregion fields & properties
    }
}