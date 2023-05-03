using UnityEngine;

namespace Universal
{
    public class CursorSettings : MonoBehaviour
    {
        #region fields & properties
        public static CursorSettings Instance { get; private set; }

        [SerializeField] private Texture2D cursorDefault;
        [SerializeField] private Texture2D cursorPoint;
        [SerializeField] private Texture2D cursorClicked;
        [SerializeField] private Vector2 cursorDefaultOffset;

        [SerializeField] private AudioClip onClickSound;
        [SerializeField] private AudioClip onSelectSound;
        public static CursorState CursorState { get; private set; }

        public static Vector3 MouseDirection { get; private set; } = Vector3.zero;
        public static float MouseWheelDirection { get; private set; } = 0;
        public static Vector3 LastMousePoint { get; private set; } = Vector3.zero;
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            SetDefaultCursor();
        }
        private void OnEnable()
        {
            SceneLoader.OnBlackScreenFading += OnBlackScreenFading;
        }
        private void OnDisable()
        {
            SceneLoader.OnBlackScreenFading -= OnBlackScreenFading;
        }
        private void OnBlackScreenFading(bool fadeUp)
        {
            if (!fadeUp) return;
            SetDefaultCursor();
        }
        public void SetDefaultCursor()
        {
            Cursor.SetCursor(cursorDefault, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Normal;
        }
        public void SetPointCursor()
        {
            Cursor.SetCursor(cursorPoint, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Point;
        }
        public void SetClickedCursor()
        {
            Cursor.SetCursor(cursorClicked, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Hold;
        }

        public void DoClickSound() => AudioManager.PlayClip(onClickSound, AudioType.Sound);

        private void Update()
        {
            Vector3 currentMousePosition = (Input.mousePosition);
            MouseDirection = currentMousePosition - LastMousePoint;
            LastMousePoint = currentMousePosition;
            MouseWheelDirection = Input.GetAxisRaw("Mouse ScrollWheel");
        }
        #endregion methods
    }
}