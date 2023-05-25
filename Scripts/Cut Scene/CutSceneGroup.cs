using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.CutScene
{
    [CreateAssetMenu(fileName = "CutSceneGroup", menuName = "ScriptableObjects/CutSceneGroup")]
    public class CutSceneGroup : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public string SceneToLoad { get; private set; } = "Game Menu";
        public List<CutSceneInfo> CutScenes => cutScenes;
        [SerializeField] private List<CutSceneInfo> cutScenes = new List<CutSceneInfo>();
        #endregion fields & properties
    }
}