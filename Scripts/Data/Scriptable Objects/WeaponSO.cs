using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public Weapon Weapon { get; private set; } = new();
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}