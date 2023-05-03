using System.Linq;
using UnityEngine;

namespace Universal
{
    public abstract class SingleSceneInstance : MonoBehaviour
    {
        #region methods
        //set instance & check
        protected abstract void Awake();
        protected void CheckInstances(System.Type type)
        {
            if (GameObject.FindObjectsOfType(type).Count() > 1) 
                throw new System.DataMisalignedException($"{type}");
        }
        #endregion methods
    }
}