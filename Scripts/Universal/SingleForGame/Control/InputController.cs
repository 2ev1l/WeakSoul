using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Universal
{
    public class InputController : MonoBehaviour
    {
        #region fields & properties
        public static UnityAction<KeyCode> OnKeyDown;
        public static List<KeyCode> CheckCodes { get; } = new List<KeyCode>() 
        { KeyCode.Escape, KeyCode.E, KeyCode.S, KeyCode.R };
        #endregion fields & properties

        #region methods
        private void Update()
        {
            for (int i = 0; i < CheckCodes.Count; ++i)
                if (Input.GetKeyDown(CheckCodes[i]))
                    OnKeyDown?.Invoke(CheckCodes[i]);
        }
        #endregion methods
    }
}