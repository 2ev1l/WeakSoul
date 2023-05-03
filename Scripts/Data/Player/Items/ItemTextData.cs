using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ItemTextData
    {
        #region fields & properties
        public string Name => name;
        [SerializeField] private string name;
        public string Description => description;
        [SerializeField] [TextArea(0, 30)] private string description;
        #endregion fields & properties

        #region methods

        #endregion methods
    }
}