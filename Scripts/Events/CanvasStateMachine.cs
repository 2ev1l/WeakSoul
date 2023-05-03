using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Events
{
    public class CanvasStateMachine : StateMachine
    {
        #region fields & properties

        #endregion fields & properties

        #region methods
        protected override void Start()
        {
            SetStatesAvailability();
        }
        #endregion methods
    }
}