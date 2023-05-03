using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class MaterialsInfo : MonoBehaviour
    {
        #region fields & properties
        public static MaterialsInfo Instance { get; private set; }
        [field: SerializeField] public Material Overlay_Default { get; private set; }
        [field: SerializeField] public Material Overlay_Choosed { get; private set; }
        [field: SerializeField] public Material Overlay_Outline { get; private set; }
        [field: SerializeField] public Material Overlay_Good { get; private set; }
        [field: SerializeField] public Material Overlay_Bad { get; private set; }
        [field: SerializeField] public Material Adventure_Card_Default { get; private set; }
        [field: SerializeField] public Material Fire_Red { get; private set; }
        [field: SerializeField] public Material Fire_Blue { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        #endregion methods
    }
}