using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoulItemSO", menuName = "ScriptableObjects/SoulItemSO")]
    public class SoulItemSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public SoulItem SoulItem { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}