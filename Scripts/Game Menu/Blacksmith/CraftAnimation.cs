using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;

namespace WeakSoul.GameMenu.Blacksmith
{
	public class CraftAnimation : MonoBehaviour
	{
		#region fields & properties
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private Image itemImage;
		[SerializeField] private Image raycastImage;
		[SerializeField] private Sprite badResult;
		#endregion fields & properties

		#region methods
		private void Start()
		{
			canvasGroup.alpha = 0;
		}
		private void OnEnable()
		{
			Forge.OnForgeStart += StartAnimtion;
		}
		private void OnDisable()
		{
			Forge.OnForgeStart -= StartAnimtion;
			StopAllCoroutines();
		}
		public void StartAnimtion()
		{
			itemImage.sprite = Forge.ForgeResultSprite;
			AudioManager.PlayClip(Forge.ForgeResultSprite == badResult ? AudioStorage.Instance.MessySound : AudioStorage.Instance.SuccessCraftSound, Universal.AudioType.Sound);
			raycastImage.raycastTarget = true;
			StartCoroutine(CanvasAlpha());
			StartCoroutine(ImageAlpha());
		}
		private IEnumerator ImageAlpha()
		{
			Color col = itemImage.color;
			col.a = 0;
			itemImage.color = col;
			ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
			vsc.StartChange(0, 1, 2f);
			while (!vsc.IsChangeEnded)
			{
				col.a = vsc.Out;
				itemImage.color = col;
				yield return CustomMath.WaitAFrame();
			}
			col.a = vsc.Out;
			itemImage.color = col;
			yield return CustomMath.WaitAFrame();
			vsc.StartChange(1, 0, 1f);
			while (!vsc.IsChangeEnded)
			{
				col.a = vsc.Out;
				itemImage.color = col;
				yield return CustomMath.WaitAFrame();
			}
			col.a = vsc.Out;
			itemImage.color = col;
			Destroy(vsc);
		}
		private IEnumerator CanvasAlpha()
		{
			canvasGroup.alpha = 1;
			ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
			vsc.StartChange(0, 1, 1.5f);
			while (!vsc.IsChangeEnded)
			{
				canvasGroup.alpha = vsc.Out;
				yield return CustomMath.WaitAFrame();
			}
			yield return CustomMath.WaitAFrame();
			vsc.StartChange(1, 0, 1.5f);
			while (!vsc.IsChangeEnded)
			{
				canvasGroup.alpha = vsc.Out;
				yield return CustomMath.WaitAFrame();
			}
			canvasGroup.alpha = 0;
			Destroy(vsc);
			raycastImage.raycastTarget = false;
		}
		#endregion methods
	}
}