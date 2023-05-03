using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public EnemyData EnemyData { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}