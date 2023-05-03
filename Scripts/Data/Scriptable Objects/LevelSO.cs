using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public Level Level { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}