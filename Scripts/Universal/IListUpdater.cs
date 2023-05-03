using UnityEngine;

namespace Universal
{
    public interface IListUpdater
    {
        public GameObject rootObject { get; }
        public int listParam { get; }
        public void OnListUpdate(int param);
    }
}