using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul
{
    public class ShowTextHelp : ShowHelp
    {
        #region fields & properties
        protected override HelpUpdater helpUpdater => HelpTextUpdater.Instance;
        [field: SerializeField] public int Id { get; set; } = 0;
        [field: SerializeField] public TextType TextType { get; set; } = TextType.Help;
        [field: SerializeField] public bool ReverseX { get; set; } = false;
        [HideInInspector] public string AdditionalText { get; set; }
        #endregion fields & properties

        #region methods
        public override void OpenPanel()
        {
            if (Id < 0) return;
            base.OpenPanel();
            HelpTextUpdater.Instance.OpenPanel(Vector3.zero, Id, TextType, AdditionalText, ReverseX);
        }
        #endregion methods
    }
}