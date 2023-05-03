using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;

namespace WeakSoul.Adventure.Map
{
    public class MapReset : MonoBehaviour
    {
        #region fields & properties
        public static MapReset Instance { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            InputController.OnKeyDown += ResetCamera;
        }
        private void OnDisable()
        {
            InputController.OnKeyDown -= ResetCamera;
		}
		private void ResetCamera(KeyCode key)
        {
            if (key != KeyCode.R || SceneManager.GetActiveScene().name != "Adventure") return;
            ResetCamera();
		}
        public void ResetCamera()
        {
			try
			{
				Vector3 position = Player.Instance.transform.position;
				position.z = 0;
				Camera.main.transform.position = position;
			}
			catch { };
		}
        #endregion methods
    }
}