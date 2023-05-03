using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.CutScene
{
	public class SceneInit : MonoBehaviour
	{
		#region fields & properties
		[SerializeField] private int sceneToLoad = 1;
		[SerializeField] private List<CutSceneGroup> cutSceneGroups = new List<CutSceneGroup>();
		public static CutSceneGroup ActiveGroup { get; private set; }
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			ActiveGroup = cutSceneGroups[GameData.Data.CutSceneId];
		}
		[ContextMenu("Get all")]
		private void GetAll()
		{
			cutSceneGroups = Resources.FindObjectsOfTypeAll<CutSceneGroup>().OrderBy(x => x.Id).ToList();
			foreach (var el in cutSceneGroups)
			{
				if (cutSceneGroups.Where(x => x.Id == el.Id).Count() > 1)
				{
					Debug.LogError($"Error - cut scene group {el.Id} id at {el.name}");
				}
			}
		}
		[ContextMenu("Load scene")]
		private void LS() => SceneLoader.Instance.LoadCutSceneFade(0, sceneToLoad);
		#endregion methods
	}
}