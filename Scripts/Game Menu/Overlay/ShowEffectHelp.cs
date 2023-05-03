using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.GameMenu
{
    public class ShowEffectHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => EffectHelpUpdater.Instance;
        public Effect Effect { get; set; }
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (Effect == null)
                return;
            base.OpenPanel();
            EffectHelpUpdater.Instance.OpenPanel(Vector3.zero, Effect);
        }
        #endregion methods
    }
}