using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.Adventure.Map
{
    public class PointLoader : Point
    {
        #region fields & properties
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
            Data.OnOpened += GenerateUI;
			Data.OnEventChanged += GenerateUI;
		}
		private void OnDisable()
		{
            Data.OnOpened -= GenerateUI;
            Data.OnEventChanged -= GenerateUI;
		}
		protected override void Start()
        {
            StartCoroutine(WaitForLoad());
        }
        private IEnumerator WaitForLoad()
        {
            while (!isInitialized)
                yield return CustomMath.WaitAFrame();
            GenerateUI();
        }
		public void Load(PointData pointData)
        {
            isInitialized = true;
            data = pointData;
            OnDisable();
            OnEnable();
        }
        protected override void OnTriggerEnter2D(Collider2D collision){}
        #endregion methods
    }
}