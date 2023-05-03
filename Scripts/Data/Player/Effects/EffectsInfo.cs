using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
	public class EffectsInfo : MonoBehaviour
	{
		#region fields & properties
		public static EffectsInfo Instance { get; private set; }
		[Min(0)][SerializeField] private int effectIdToAdd;
		[field: SerializeField] public List<EffectSO> Effects { get; private set; }
		[field: SerializeField] public RuntimeAnimatorController BurnAnimator { get; private set; }
		#endregion fields & properties

		#region methods
		public void Init()
		{
			Instance = this;
		}
		public Effect GetEffect(int effectId) => Effects[effectId].Effect;
		[ContextMenu("Add effect")]
		private void AddEffect() => GameData.Data.PlayerData.Stats.TryAddOrStackEffect(effectIdToAdd);
		[ContextMenu("Clear all")]
		private void ClearEffects() => GameData.Data.PlayerData.Stats.RemoveAllEffects(true);

		[ContextMenu("Get all")]
		private void Get()
		{
			Effects = new();
			Effects.AddRange(Resources.FindObjectsOfTypeAll<EffectSO>());
			Sort();
		}
		private void Sort()
		{
			Effects = Effects.OrderBy(x => x.Effect.Id).ToList();
		}
#if false
		[ContextMenu("Create")]
		private void CreateAll()
		{
			string path = "Assets/Resources/Scriptable Object/Effects/Effect ";
			for (int i = 22; i < 52; ++i)
			{
				EffectSO effect = ScriptableObject.CreateInstance<EffectSO>();
				effect.Effect.ChangeID(i);
				string newPath = $"{path}{i}.asset";
				AssetDatabase.CreateAsset(effect, newPath);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		#endregion methods
	}
}