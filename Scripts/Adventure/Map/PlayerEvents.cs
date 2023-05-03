using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.Adventure.Map
{
    public class PlayerEvents : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Player player;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            Player.OnPointChanged += GetEvents;
        }
        private void OnDisable()
        {
            Player.OnPointChanged -= GetEvents;
        }
        private void GetEvents(int oldPointId, int newPointId)
        {

        }
        #endregion methods
    }
}