using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events
{
	public class SceneInit : SingleSceneInstance
	{
		#region fields & properties
		public static SceneInit Instance { get; private set; }
		[SerializeField] private StateMachine canvasStateMachine;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
			GameData.CanSaveData = false;
			CheckChoosedZone();
		}
		private void CheckChoosedZone()
		{
			canvasStateMachine.TryApplyState(canvasStateMachine.States.Cast<EventState>().Where(x => x.EventType.Equals(EventInfo.Instance.Data.Event.EventType)).First());
		}
		public void IncreasePlayerExp()
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			ChangePlayerExp(CustomMath.Multiply(playerLevel, 50));
		}
		public void DecreasePlayerExp()
		{
			int playerLevel = GameData.Data.PlayerData.Stats.ExperienceLevel.Level;
			ChangePlayerExp(-CustomMath.Multiply(playerLevel, 20));
		}
		private void ChangePlayerExp(int value) => GameData.Data.PlayerData.Stats.ExperienceLevel.Experience += value;
		#endregion methods
	}
}