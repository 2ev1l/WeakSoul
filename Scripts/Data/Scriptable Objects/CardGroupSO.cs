using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardGroupSO", menuName = "ScriptableObjects/CardGroupSO")]
    public class CardGroupSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public CardGroup CardGroup { get; private set; }
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}