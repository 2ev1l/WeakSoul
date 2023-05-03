using UnityEngine;

namespace Universal
{
    public abstract class StateChange : MonoBehaviour
    {
        #region methods
        public abstract void SetActive(bool active);
        #endregion methods
    }
}