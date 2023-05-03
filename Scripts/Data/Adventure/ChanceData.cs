using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Data.Adventure
{
    [System.Serializable]
    public class ChanceData
    {
        #region fields & properties
        public int Id => id;
        [Min(0)][SerializeField] private int id;
        public float Chance => chance;
        [Range(0,100)][SerializeField] private float chance;
        #endregion fields & properties

        #region methods
        public bool TryGetChance(out int id)
        {
            id = this.id;
            return CustomMath.GetRandomChance(chance);
        }
        #endregion methods
    }
}