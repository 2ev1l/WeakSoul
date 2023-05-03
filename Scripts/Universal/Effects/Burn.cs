using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Universal.Effects
{
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Animator))]
	public class Burn : MonoBehaviour
	{
		#region fields & properties
		private RuntimeAnimatorController controller;
		private Material burnMaterial;
		private Animator Animator
		{
			get
			{
				animator = animator == null ? gameObject.GetComponent<Animator>() : animator;
				return animator;
			}
		}
		private Animator animator;
		private SpriteRenderer SpriteRenderer
		{
			get
			{
				spriteRenderer = spriteRenderer == null ? GetComponent<SpriteRenderer>() : spriteRenderer;
				return spriteRenderer;
			}
		}
		private SpriteRenderer spriteRenderer;
		private ValueSmoothChanger vsc;
		public bool IsAnimationEnded { get; private set; } = false;
		private float spriteDefaultAlpha = 1f;
		#endregion fields & properties

		#region methods
		public void StartAnimation(float speed)
		{
			StartAnimation(MaterialsInfo.Instance.Fire_Red, speed);
		}
		[ContextMenu("Animate")]
		public void StartAnimation()
		{
			StartAnimation(MaterialsInfo.Instance.Fire_Red);
		}
		public void StartAnimation(Material burnMaterial, float speed = 1f)
		{
			Animator.speed = speed;
			this.burnMaterial = burnMaterial;
			controller = EffectsInfo.Instance.BurnAnimator;
			Animator.runtimeAnimatorController = controller;
			SpriteRenderer.material = this.burnMaterial;
			Animator.Play("Burn");
			Invoke(nameof(EndAnimation), 1f / speed);
		}
		public void DecreaseSpriteAlpha(float speed = 1f) => StartCoroutine(SpriteAlphaDecrease(speed));
		private IEnumerator SpriteAlphaDecrease(float speed = 1f)
		{
			spriteDefaultAlpha = SpriteRenderer.color.a;
			vsc = gameObject.AddComponent<ValueSmoothChanger>();
			vsc.StartChange(SpriteRenderer.color.a, 0, 1f / speed);
			while (!vsc.IsChangeEnded || gameObject != null)
			{
				Color col = SpriteRenderer.color;
				col.a = vsc.Out;
				SpriteRenderer.color = col;
				yield return CustomMath.WaitAFrame();
			}
			Destroy(vsc);
		}
		public void ReturnSpriteAlpha()
		{
			Color col = SpriteRenderer.color;
			col.a = spriteDefaultAlpha;
			SpriteRenderer.color = col;
		}
		public void EndAnimation()
		{
			CancelInvoke(nameof(EndAnimation));
			Destroy(this);
			Destroy(Animator, Time.deltaTime);
			if (vsc != null)
				Destroy(vsc);
			IsAnimationEnded = true;
		}
		#endregion methods
	}
}