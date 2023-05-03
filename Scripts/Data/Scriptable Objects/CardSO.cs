using Data.Adventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardSO", menuName = "ScriptableObjects/CardSO")]
    public class CardSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public CardData CardData { get; private set; } = new();
		#endregion fields & properties

		#region methods
		#endregion methods
	}
}