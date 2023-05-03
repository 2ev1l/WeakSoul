using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Universal
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MaterialRaycastChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> isDefault;
        /// </summary>
        public UnityAction<bool> OnMaterialChanged;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                spriteRenderer = spriteRenderer != null ? spriteRenderer : GetComponent<SpriteRenderer>();
                return spriteRenderer;
            }
        }
        private SpriteRenderer spriteRenderer;
        public Material MaterialChanged => ChangedMaterial;
        [SerializeField] private Material ChangedMaterial;
        public Material MaterialDefault => DefaultMaterial;
        [SerializeField] private Material DefaultMaterial;
        #endregion fields & properties

        #region methods
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            SetChangedMaterial();
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            SetDefaultMaterial();
        }
        private void OnDisable()
        {
            SetDefaultMaterial();
        }
        public void SetChangedMaterial(Material newMaterial) => ChangedMaterial = newMaterial;
        protected void SetDefaultMaterial()
        {
            SpriteRenderer.material = DefaultMaterial;
            OnMaterialChanged?.Invoke(true);
        }
        protected void SetChangedMaterial()
        {
            SpriteRenderer.material = ChangedMaterial;
            OnMaterialChanged?.Invoke(false);
        }
        #endregion methods
    }
}