using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CampEventSO", menuName = "ScriptableObjects/CampEventSO")]
    public class CampEventSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public CampEvent CampEvent { get; private set; }
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}