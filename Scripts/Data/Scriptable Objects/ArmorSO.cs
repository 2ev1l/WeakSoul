using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ArmorSO", menuName = "ScriptableObjects/ArmorSO")]
    public class ArmorSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public Armor Armor { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}