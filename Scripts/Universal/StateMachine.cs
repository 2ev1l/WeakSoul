using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal
{
    public abstract class StateMachine : MonoBehaviour
    {
        #region fields
        public IEnumerable<StateChange> States => states;
        [SerializeField] protected List<StateChange> states = new List<StateChange>();
        protected StateChange currentState;
        #endregion fields

        #region methods
        protected virtual void Start()
        {
            ApplyDefaultState();
            SetStatesAvailability();
        }
        public virtual void ApplyDefaultState() => ApplyState(states[0]);
        public virtual void TryApplyState(StateChange choosedState)
        {
            if (currentState != null && choosedState == currentState && currentState != states[0]) return;
            ApplyState(choosedState);
        }
        public virtual void TryApplyState(int stateId) => TryApplyState(states[stateId]);
        protected virtual void ApplyState(StateChange choosedState)
        {
            currentState = choosedState;
            states.ForEach(x => x.SetActive(currentState == x));
        }
        public virtual void SetStatesAvailability() { }
        #endregion methods
    }
}