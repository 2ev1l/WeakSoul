using Data;
using Data.Adventure;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;
using Universal.Effects;
using WeakSoul.Events;
using WeakSoul.GameMenu.Skills;

namespace WeakSoul.Adventure.Map
{
	public class Card : MonoBehaviour
	{
		#region fields & properties
		public static UnityAction<Card> OnCardChoosed;
		/// <summary>
		/// <see cref="{T0}"/> burned card;
		/// </summary>
		private static UnityAction<Card> OnBurnEnd;

		[Header("UI")]
		[SerializeField] private SpriteRenderer cardBG;
		[SerializeField] private SpriteRenderer cardOutline;
		[SerializeField] private Image raycastImage;
		[SerializeField] private CustomButton customButton;
		[SerializeField] private LanguageLoader nameLanguage;
		[SerializeField] private LanguageLoader descriptionLanguage;

		[Header("Animation")]
		[SerializeField] private GameObject chooseEffect;
		[SerializeField] private string animatorChooseState;
		[SerializeField] private string animatorBackState;
		[SerializeField] private Animator animator;
		[SerializeField] private ParticleSystem rewardParticles;
		[SerializeField] private List<SpriteRenderer> burnObjects;
		[SerializeField] private List<Text> texts;
		[SerializeField] private List<SpriteRenderer> alphaObjects;
		[SerializeField] private StatText statPrefab;
		[SerializeField] private Transform spawnCanvas;

		[Header("Debug")]
		[SerializeField][ReadOnly] private List<Burn> burnEffects;
		public CardData Data
		{
			get => data;
			set
			{
				data = value;
				UpdateUI();
			}
		}
		private CardData data;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			customButton.OnClicked += ChooseThisCard;
			customButton.OnEnter += EnableChooseEffect;
			customButton.OnExit += DisableChooseEffect;
			OnCardChoosed += CheckChoosedCard;
			OnBurnEnd += CheckClosePanel;
			DisableChooseEffect();
		}
		private void OnDisable()
		{
			customButton.OnClicked -= ChooseThisCard;
			customButton.OnEnter -= EnableChooseEffect;
			customButton.OnExit -= DisableChooseEffect;
			OnCardChoosed -= CheckChoosedCard;
			OnBurnEnd -= CheckClosePanel;
		}
		private void EnableChooseEffect()
		{
			animator.Play(animatorChooseState, 0, 1f - CustomAnimation.GetNormalizedAnimatorTime(animator, 0));
			chooseEffect.SetActive(true);
		}
		private void DisableChooseEffect()
		{
			animator.Play(animatorBackState, 0, 1f - CustomAnimation.GetNormalizedAnimatorTime(animator, 0));
			chooseEffect.SetActive(false);
		}
		private void ChooseThisCard()
		{
			customButton.OnEnter -= EnableChooseEffect;
			customButton.OnExit -= DisableChooseEffect;
			OnCardChoosed?.Invoke(this);
		}
		private void CheckChoosedCard(Card card)
		{
			DisableCard();
			if (card == this)
			{
				card.Data.GetStatsReward();
				animator.Play(animatorBackState, 0, 1f - CustomAnimation.GetNormalizedAnimatorTime(animator, 0));
			}
			else
			{
				StartCoroutine(CardBurn(true, MaterialsInfo.Instance.Fire_Red));
				CardsPanel.Instance.DisableBackButton();
			}
		}
		private void CheckClosePanel(Card card)
		{
			if (card == this) return;
			StartCoroutine(CheckClosePanel());
		}
		private IEnumerator CheckClosePanel()
		{
			AddReward();
			yield return new WaitForSecondsRealtime(0.3f);
			yield return CardBurn(false, MaterialsInfo.Instance.Fire_Blue);
			yield return new WaitForSecondsRealtime(0.2f);
			CheckCardActions();
			gameObject.SetActive(false);
		}
		private void CheckCardActions()
		{
			PlayerData player = GameData.Data.PlayerData;
			ItemsInventory playerInventory = player.Inventory;
			bool IsSoulItem_201 = playerInventory.ContainItem(201);
			bool IsSoulItem_202 = playerInventory.ContainItem(202);

			foreach (StatScale el in data.StatsScale)
			{
				int increased = el.CalculateIncreasedValue(player.Stats);
				if (el.StatsType == PhysicalStatsType.Health)
				{
					if (IsSoulItem_201 && increased < 0)
						increased = CustomMath.Multiply(increased, 90);
					if (IsSoulItem_202)
						increased = CustomMath.Multiply(increased, 110);
				}
				player.Stats.IncreaseStatsByType(el.StatsType, increased);

				Vector3 pos = transform.position + Vector3.up * Random.Range(-0.5f, 2f) + Vector3.right * Random.Range(-0.5f, 0.5f);
				StatText.SpawnPrefab(statPrefab, spawnCanvas, pos, el.StatsType, increased);
			}
			if (player.Stats.Health == 0)
			{
				Player.Instance.MoveToDirections();
				return;
			}

			if (data.EventChance.TryGetChance(out int eventId))
			{
				EventInfo.Instance.Data.LoadEvent(eventId);
				return;
			}

			foreach (var el in data.CardGroupsChance)
			{
				if (!el.TryGetChance(out int groupId)) continue;
				if (!CardsInfo.Instance.GetGroup(groupId).IsGroupAllowed()) continue;

				CardsPanel.Instance.SetNextCards(groupId);
				break;
			}

			if (data.MoveDirections.Count() > 0)
			{
				Player.Instance.MoveToDirections(data.MoveDirections.ToArray());
			}
			else
			{
				Player.Instance.MoveToDirections();
			}
		}
		private void AddReward()
		{
			bool isGoodClipPlayed = false;
			bool isBadClipPlayed = false;
			if (data.Experience > 0)
			{
				isGoodClipPlayed = true;
				AudioManager.PlayClip(AudioStorage.Instance.GoodRandomSound, Universal.AudioType.Sound);
			}
			else if (data.Experience < 0)
			{
				isBadClipPlayed = true;
				AudioManager.PlayClip(AudioStorage.Instance.BadRandomSound, Universal.AudioType.Sound);
			}

			if (data.Rewards.Count() == 0) return;
			bool isRewardAdded = false;
			foreach (var el in data.Rewards)
			{
				if (!el.TryAddReward(out RewardData reward)) continue;
				isRewardAdded = true;
				BurstParticleSystem(reward);
			}
			if (!isGoodClipPlayed && !isBadClipPlayed)
				AudioManager.PlayClip(isRewardAdded ? AudioStorage.Instance.GoodRandomSound : AudioStorage.Instance.BadRandomSound, Universal.AudioType.Sound);
		}
		private void BurstParticleSystem(RewardData reward)
		{
			ParticleSystem ps = Instantiate(rewardParticles, rewardParticles.transform.parent.parent);
			ps.textureSheetAnimation.SetSprite(0, reward.GetRewardSprite());
			for (int i = reward.Count; i > 0; i--)
				CustomAnimation.BurstParticlesAt(transform.position, ps);
		}
		public void EnableCard()
		{
			ActivateCard();
		}
		private void ActivateCard()
		{
			gameObject.SetActive(true);
			raycastImage.enabled = true;
			cardOutline.material = MaterialsInfo.Instance.Adventure_Card_Default;
			cardBG.material = MaterialsInfo.Instance.Adventure_Card_Default;
			texts.ForEach(x =>
			{
				Color col = x.color;
				col.a = 1;
				x.color = col;
			});
			alphaObjects.ForEach(x =>
			{
				Color col = x.color;
				col.a = 1;
				x.color = col;
			});
			burnObjects.ForEach(x =>
			{
				Color col = x.color;
				col.a = 1;
				x.color = col;
			});
		}
		private void DisableCard()
		{
			raycastImage.enabled = false;
		}
		private IEnumerator CardBurn(bool disableObject, Material burnMaterial)
		{
			AudioManager.PlayClip(AudioStorage.Instance.BurnSound, Universal.AudioType.Sound);
			ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
			burnEffects = new();
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
				alphaObjects.ForEach(x =>
				{
					Color col = x.color;
					col.a = vsc.Out / 2f;
					x.color = col;
				});
				yield return CustomMath.WaitAFrame();
			}
			texts.ForEach(x =>
			{
				Color col = x.color;
				col.a = 0;
				x.color = col;
			});
			alphaObjects.ForEach(x =>
			{
				Color col = x.color;
				col.a = 0;
				x.color = col;
			});
			if (disableObject)
				gameObject.SetActive(false);
			Destroy(vsc);
			burnEffects.ForEach(x => x.EndAnimation());
			burnObjects.ForEach(x =>
			{
				Color col = x.color;
				col.a = 0;
				x.color = col;
			});
			DisableCard();
			OnBurnEnd?.Invoke(this);
		}
		private void UpdateUI()
		{
			cardBG.sprite = data.Texture;
			nameLanguage.Id = data.Id;
			descriptionLanguage.Id = data.Id;
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			bool isSoulItem_Direction = inventory.ContainItem(13);
			bool isSoulItem_DirectonCount = inventory.ContainItem(14);
			bool isSoulItem_Reward = inventory.ContainItem(82);
			string addText = data.GetDirectionText(isSoulItem_DirectonCount, isSoulItem_Direction);
			addText += isSoulItem_Reward ? data.GetRewardText() : "";
			descriptionLanguage.AddText(addText);
		}
		#endregion methods
	}
}