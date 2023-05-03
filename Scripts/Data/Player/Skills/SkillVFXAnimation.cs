using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Universal;

namespace Data
{
    [System.Serializable]
    public class SkillVFXAnimation
    {
        #region fields & properties
        public IEnumerable<SkillVFX> VFXs => vfxs;
        [SerializeField] private List<SkillVFX> vfxs = new();
        public bool WaitForIconBurn => waitForIconBurn;
        [SerializeField] private bool waitForIconBurn = true;
        #endregion fields & properties

        #region methods
        public SkillVFXAnimation Clone()
        {
            SkillVFXAnimation vfxa = new();
            vfxs.ForEach(x => vfxa.vfxs.Add(x.Clone()));
            vfxa.waitForIconBurn = waitForIconBurn;
            return vfxa;
        }
        #endregion methods
    }
}