using Data;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Universal
{
    public class HelpUpdater : MonoBehaviour
    {
        #region fields & properties
        private Vector3 defaultBodyScale;
        [SerializeField] private float directionScale = 0.9f;
        [SerializeField] private GameObject body;
        [SerializeField] private Transform negativeMirror;
        protected Vector3 startMirror;
        private bool isParamsInitialized = false;
        public bool State { get; private set; }
        #endregion fields & properties

        #region methods
        public virtual void Init()
        {
            if (!isParamsInitialized)
            {
                defaultBodyScale = body.transform.localScale;
                startMirror = negativeMirror.localPosition;
                isParamsInitialized = true;
            }
            body.SetActive(false);
            UpdateBody();
        }
        private void OnEnable()
        {
            SettingsData.Data.OnGraphicsChanged += delegate { UpdateBody(); };
            SettingsData.Data.GraphicsSettings.OnSettingsChanged += delegate { UpdateBody(); };
        }
        private void OnDisable()
        {
            SettingsData.Data.OnGraphicsChanged -= delegate { UpdateBody(); };
            SettingsData.Data.GraphicsSettings.OnSettingsChanged -= delegate { UpdateBody(); };
        }

        private void UpdateBody()
        {
            body.transform.localScale = defaultBodyScale * CustomMath.GetOptimalScreenScale();
        }
        public virtual void OpenPanel(Vector3 position)
        {
            State = true;
            body.SetActive(true);
            TransformBodyPosition(position);
        }
        private void TransformBodyPosition(Vector3 position)
        {
            position += Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position += CursorSettings.MouseDirection * directionScale;
            Vector2 screenSquare = CustomMath.GetScreenSquare();
            Vector2 modifiedOffset = new();
            modifiedOffset *= screenSquare;
            position.x += modifiedOffset.x;
            position.y += modifiedOffset.y;
            position.z = 0;
            body.transform.position = position;

            position = body.transform.localPosition;
            position.z = 0;
            body.transform.localPosition = position;

            position = startMirror;
            position.x *= screenSquare.x;
            position.y *= screenSquare.y;
            position.z = 0;
            negativeMirror.localPosition = position;
        }
        public void HidePanel()
        {
            if (body != null)
            {
                body.SetActive(false);
                State = false;
            }
        }
        #endregion methods
    }
}