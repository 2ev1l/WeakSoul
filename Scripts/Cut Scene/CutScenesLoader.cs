using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;
using WeakSoul.MainMenu;

namespace WeakSoul.CutScene
{
	public class CutScenesLoader : MonoBehaviour
	{
		#region fields & properties
		public UnityAction OnSceneChanged;
		public UnityAction<float> OnSecondLasts;

		private bool isSecondInvoked;
		public float TimeLasts { get; private set; }
		public int ActiveSceneId { get; private set; } = -1;
		public string CurrentText => LanguageLoader.GetTextByType(TextType.CutScene, CurrentScene.TextId);
		public CutSceneInfo CurrentScene => SceneInit.ActiveGroup.CutScenes[ActiveSceneId];
		#endregion fields & properties

		#region methods
		private void Start()
		{
			ChangeScene();
		}
		private void OnEnable()
		{
			SettingsPanelInit.Instance.MenuBackButton.OnClicked += CancelWait;
		}
		private void OnDisable()
		{
			SettingsPanelInit.Instance.MenuBackButton.OnClicked -= CancelWait;
		}
		private void ChangeScene()
		{
			if (++ActiveSceneId >= SceneInit.ActiveGroup.CutScenes.Count)
			{
				SceneLoader.Instance.LoadSceneFade(SceneInit.ActiveGroup.SceneToLoad, 0f);
				return;
			}
			isSecondInvoked = false;
			TimeLasts = Mathf.Max(CurrentText.Length * .25f, 3f);
			OnSceneChanged?.Invoke();
			Wait();
		}
		private void CancelWait() => CancelInvoke(nameof(Wait));
		public void SkipScene()
		{
			CancelWait();
			ChangeScene();
		}
		private void Wait()
		{
			if (TimeLasts > 0)
			{
				TimeLasts -= Time.deltaTime;
				Invoke(nameof(Wait), Time.deltaTime);
				if (TimeLasts <= 1 && !isSecondInvoked)
				{
					OnSecondLasts?.Invoke(TimeLasts);
					isSecondInvoked = true;
				}
			}
			else
				ChangeScene();
		}
		#endregion methods
	}
}