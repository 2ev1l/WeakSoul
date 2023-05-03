using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestDataSO", menuName = "ScriptableObjects/ChestDataSO")]
    public class ChestDataSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public ChestData ChestData { get; private set; }
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}