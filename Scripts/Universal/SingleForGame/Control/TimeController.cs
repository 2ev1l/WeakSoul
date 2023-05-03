using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal
{
    public class TimeController : MonoBehaviour
    {
        #region fields & properties
        public static TimeController Instance { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            CancelInvoke(nameof(UpdateTime));
            UpdateTime();
        }
        private void UpdateTime()
        {
            GameData.Data.TimePlayed++;
            Invoke(nameof(UpdateTime), 1);
        }
        #endregion methods
    }
}