using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
    [System.Serializable]
    public class SoulInfo
    {
        #region fields & properties
        [field: SerializeField] public SoulType SoulType { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Sprite Sprite2x { get; private set; }
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}