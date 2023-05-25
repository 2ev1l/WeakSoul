using Data;
using Data.Events;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Events.Puzzle
{
	public class EventInit : SingleSceneInstance
	{
		#region fields & properties
		public static EventInit Instance { get; private set; }
		[SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
		[SerializeField] private Sprite portalSprite;

		[SerializeField] private StateMachine panelStateMachine;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		private void Start()
		{
			Init();
		}
		private void Init()
		{
			Sprite eventSprite = (EventInfo.Instance.Data.Event.Id) switch
			{
				20 => portalSprite,
                _ => throw new System.NotImplementedException("Map event id for Puzzle")
            };
			SetState();
			bgSpriteRenderers.ForEach(x => x.sprite = eventSprite);
		}
		private void SetState()
		{
			int eventId = EventInfo.Instance.Data.Event.Id;
            foreach (var el in panelStateMachine.States.Cast<MainPanelIdState>())
            {
                if (el.Id == eventId)
                {
					panelStateMachine.TryApplyState(el);
					return;
                }
            }
			Debug.LogError($"Error - can't find {eventId} event id for states in puzzle");
        }
		#endregion methods
	}
}