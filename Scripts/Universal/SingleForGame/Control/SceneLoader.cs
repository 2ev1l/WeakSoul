using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Universal
{
	public class SceneLoader : MonoBehaviour
	{
		#region fields & properties
		public static UnityAction OnSceneLoaded;
		/// <summary>
		/// <see cref="{T0}"/> oldSceneName;
		/// <see cref="{T1}"/> newSceneName;
		/// </summary>
		public static UnityAction<string, string> OnSceneChanged;
		public static UnityAction<bool> OnBlackScreenFading;
		public static UnityAction OnStartLoading;
		public static SceneLoader Instance { get; private set; }
		public static bool IsSceneLoading { get; private set; }
		public static float ScreenFadeTime { get; private set; } = 1f;
		private List<string> SavingScenes { get; } = new List<string>() { "Cut Scene", "Game Menu", "City" };

		[SerializeField] private CanvasGroup fadeCanvas;
		private static string sceneToLoad;

		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			SavingUtils.OnDataReset += DisableLoadingCanvas;
		}
		private void OnDisable()
		{
			SavingUtils.OnDataReset -= DisableLoadingCanvas;
		}
		private void DisableLoadingCanvas() => Instance.fadeCanvas.alpha = 0;
		public void Init()
		{
			Instance = this;
		}
		public void Start()
		{
			IsSceneLoading = false;
			string currentScene = SceneManager.GetActiveScene().name;
			if (SavingScenes.Contains(currentScene))
				GameData.Data.SceneName = currentScene;
			BlackScreenFade(false);
		}
		public void LoadCutSceneFade(float time, int cutSceneId)
		{
			GameData.Data.CutSceneId = cutSceneId;
			LoadSceneFade("Cut Scene", time);
		}
		public void LoadSceneFade(string scene, float time)
		{
			IsSceneLoading = true;

			BlackScreenFade(true, 1f / time);
			LoadScene(scene, time);
			RemoveEvents();
		}
		public void LoadScene(string scene, float time)
		{
			OnStartLoading?.Invoke();
			IsSceneLoading = true;
			sceneToLoad = scene;
			Invoke(nameof(LoadSceneA), time);
			RemoveEvents();
		}
		private void LoadSceneA() => StartCoroutine(LoadScene(sceneToLoad));
		private static IEnumerator LoadScene(string scene)
		{
			string oldScene = SceneManager.GetActiveScene().name;
			IsSceneLoading = true;
			SavingUtils.TrySaveGameData();
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
			yield return asyncLoad;
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
			IsSceneLoading = false;
			OnSceneLoaded?.Invoke();
			OnSceneChanged?.Invoke(oldScene, scene);
		}
		public static void BlackScreenFade(bool fadeUp, float animationSpeed = 1f)
		{
			Instance.fadeCanvas.gameObject.SetActive(true);
			Instance.fadeCanvas.alpha = fadeUp ? 0 : 1;
			Instance.fadeCanvas.blocksRaycasts = fadeUp;
			int finalValue = fadeUp ? 1 : 0;
			Instance.StartCoroutine(BlackScreenAlphaChange(finalValue, 0.9f / animationSpeed));
		}
		private static IEnumerator BlackScreenAlphaChange(int finalValue, float time)
		{
			ValueSmoothChanger vsc = Instance.gameObject.AddComponent<ValueSmoothChanger>();
			vsc.StartChange((finalValue + 1) % 2, finalValue, time);
			bool up = (finalValue + 1) % 2 == 0;
			while (true)
			{
				yield return CustomMath.WaitAFrame();
				OnBlackScreenFading?.Invoke(up);
				Instance.fadeCanvas.alpha = vsc.Out;
				if (vsc.IsChangeEnded)
				{
					Destroy(vsc);
					yield break;
				}
			}
		}
		public static void BlackScreenFadeZero()
		{
			Instance.fadeCanvas.gameObject.SetActive(false);
		}
		private static void RemoveEvents()
		{
			GameObject eventSystem = GameObject.Find("EventSystem");
			if (eventSystem != null)
				eventSystem.SetActive(false);
		}
		public static bool IsBlackScreenFade() => Instance.fadeCanvas.alpha > 0f && Instance.fadeCanvas.alpha < 1f;

		[SerializeField] private string scene;
		[ContextMenu("Load Scene")]
		private void ls() => LoadScene(scene, 0);
		#endregion methods
	}
}