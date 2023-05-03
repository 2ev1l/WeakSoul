using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;
using Universal.Effects;

namespace WeakSoul.GameMenu
{
	public class ClassChoose : MonoBehaviour
	{
		#region fields & properties
		public static UnityAction<ClassChoose> OnClassChoosed;
		[SerializeField] private PlayerClass playerClass;
		[SerializeField] private CustomButton customButton;

		[Header("UI")]
		[SerializeField] private Image raycastImage;
		[SerializeField] private SpriteRenderer mainSpriteRenderer;
		[SerializeField] private SpriteRenderer borderSpriteRenderer;
		[SerializeField] private Canvas textCanvas;
		[SerializeField] private LanguageLoader descriptionLanguage;

		[Header("Animation")]
		[SerializeField] private List<SpriteRenderer> burnObjects;
		[SerializeField] private List<Text> texts;
		[SerializeField] private ValueSmoothChanger scaler;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			OnClassChoosed += ChooseAnimation;
			customButton.OnClicked += ChooseThisClass;
			customButton.OnEnter += ScaleCardUp;
			customButton.OnExit += ScaleCardDown;
			customButton.OnClicked += ScaleCardDown;
		}
		private void OnDisable()
		{
			OnClassChoosed -= ChooseAnimation;
			customButton.OnClicked -= ChooseThisClass;
			customButton.OnEnter -= ScaleCardUp;
			customButton.OnExit -= ScaleCardDown;
			customButton.OnClicked -= ScaleCardDown;
		}
		private void ScaleCardUp()
		{
			mainSpriteRenderer.sortingOrder = -9;
			borderSpriteRenderer.sortingOrder = -9;
			textCanvas.sortingOrder = -8;
			StartCoroutine(ScaleCard(transform.localScale.x, 1.2f));
		}
		private void ScaleCardDown()
		{
			mainSpriteRenderer.sortingOrder = -11;
			borderSpriteRenderer.sortingOrder = -11;
			textCanvas.sortingOrder = -12;
			StartCoroutine(ScaleCard(transform.localScale.x, 0.8f));
		}
		private IEnumerator ScaleCard(float start, float end)
		{
			scaler.StartChange(start, end, 0.5f);
			while (!scaler.IsChangeEnded)
			{
				Vector3 localScale = transform.localScale;
				localScale = scaler.Out * Vector3.one;
				transform.localScale = localScale;
				yield return CustomMath.WaitAFrame();
			}
		}
		private void ChooseThisClass()
		{
			GameData.Data.PlayerData.Stats.SetPlayerClass(playerClass);
			OnClassChoosed?.Invoke(this);
		}
		private void ChooseAnimation(ClassChoose classChoose)
		{
			raycastImage.gameObject.SetActive(false);
			if (classChoose == this)
			{
				GainClassRewardOnChoose();
				StartCoroutine(Burn(MaterialsInfo.Instance.Fire_Blue, 1f));
			}
			else
			{
				StartCoroutine(Burn(MaterialsInfo.Instance.Fire_Red));
			}
		}
		private void GainClassRewardOnChoose()
		{
			PlayerStats playerStats = GameData.Data.PlayerData.Stats;
			switch (playerClass)
			{
				case PlayerClass.Omnivorous:
					playerStats.CriticalChance += 5;
					break;
				case PlayerClass.Impartial:
					playerStats.Damage += 1;
					break;
				case PlayerClass.HaterOfEvil:
					playerStats.Health += 4;
					playerStats.Resistance += 1;
					break;
				case PlayerClass.Stoic:
					playerStats.Health += 3;
					break;
				case PlayerClass.PosthumousHero:
					playerStats.Health -= 2;
					break;
				default: break;
			}
		}
		private IEnumerator Burn(Material burnMaterial, float timeToWait = 0f)
		{
			yield return new WaitForSecondsRealtime(timeToWait);
			AudioManager.PlayClip(AudioStorage.Instance.BurnSound, Universal.AudioType.Sound);
			descriptionLanguage.RemoveTextXML();
			ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
			List<Burn> burnEffects = new();
			burnObjects.ForEach(x => burnEffects.Add(x.gameObject.AddComponent<Burn>()));
			burnEffects.ForEach(x => x.StartAnimation(burnMaterial, 0.9f));
			vsc.StartChange(1, 0, 0.7f);
			while (!burnEffects.Last().IsAnimationEnded && !vsc.IsChangeEnded)
			{
				texts.ForEach(x =>
				{
					Color col = x.color;
					col.a = vsc.Out;
					x.color = col;
				});
				yield return CustomMath.WaitAFrame();
			}
			texts.ForEach(x =>
			{
				Color col = x.color;
				col.a = vsc.Out;
				x.color = col;
			});
			Destroy(vsc);
			burnEffects.ForEach(x => { if (x != null) x.EndAnimation(); });
			burnObjects.ForEach(x =>
			{
				Color col = x.color;
				col.a = 0;
				x.color = col;
			});
			Disable();
		}

		private void Disable()
		{
			gameObject.SetActive(false);
		}
		#endregion methods
	}
}