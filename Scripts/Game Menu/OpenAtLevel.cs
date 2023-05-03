using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
    public class OpenAtLevel : MonoBehaviour
    {
        #region fields & properties
        [Min(-1)][SerializeField] private int opensAtLevel;
        protected GameObject Obj => obj;
        [SerializeField] private GameObject obj;
        protected int PlayerLevel => GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged += CheckOpen;
            CheckOpen();
        }
        private void OnDisable()
        {
            GameData.Data.PlayerData.Stats.ExperienceLevel.OnLevelChanged -= CheckOpen;
        }
        private void CheckOpen(int level) => CheckOpen();
        protected virtual void CheckOpen()
        {
            obj.SetActive(PlayerLevel >= opensAtLevel);
        }
        #endregion methods
    }
}