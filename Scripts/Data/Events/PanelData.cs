using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Events
{
    [System.Serializable]
    public class PanelData
    {
        #region fields & properties
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }
        [SerializeField] private Vector3 position;
        public int PanelId
        {
            get => panelId;
            set => panelId = value;
        }
        [SerializeField] private int panelId;

        #endregion fields & properties

        #region methods
        public PanelData() { }
        public PanelData(Vector3 position, int panelId)
        {
            this.position = position;
            this.panelId = panelId;
        }

        #endregion methods
    }
}