using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Universal
{
    [RequireComponent(typeof(Renderer))]
    public class ProgressBar : MonoBehaviour
    {
        #region fields & properties
        public UnityAction<float> OnProgressChanged;
        private Renderer Render
        {
            get
            {
                render = render == null ? GetComponent<Renderer>() : render;
                return render;
            }
        }
        private Renderer render;

        public float Progress
        {
            get => progress;
            set => SetProgress(value);
        }
        [SerializeField] [Range(0f, 1f)] private float progress = 0f;
        [SerializeField] private ProgressBarDirection direction = ProgressBarDirection.Right;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            ChangeLine();
        }
        private void SetProgress(float value)
        {
            progress = value;
            progress = Mathf.Clamp01(progress);
            ChangeLine();
            OnProgressChanged?.Invoke(progress);
        }
        [ContextMenu("Update Line")]
        private void ChangeLine()
        {
            string param = direction switch
            {
                ProgressBarDirection.Right => "_ClipUvRight",
                ProgressBarDirection.Left => "_ClipUvLeft",
                ProgressBarDirection.Up => "_ClipUvUp",
                ProgressBarDirection.Down => "_ClipUvDown",
                _ => throw new System.NotImplementedException()
            };
            Render.material.SetFloat(param, 1 - progress);
        }
        #endregion methods
    }
}