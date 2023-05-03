using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EffectSO", menuName = "ScriptableObjects/EffectSO")]
    public class EffectSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public Effect Effect { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}