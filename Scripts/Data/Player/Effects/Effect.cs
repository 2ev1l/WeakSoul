using UnityEngine;
using UnityEngine.Events;

namespace Data
{
	[System.Serializable]
	public class Effect
	{
		#region fields & properties
		/// <summary>
		/// <see cref="{T0}"/> base effect data
		/// </summary>
		public UnityAction<Effect> OnEffectStacked;
		public UnityAction<int> OnDurationChanged;
		public Sprite Sprite => sprite;
		[SerializeField] private Sprite sprite;

		public int Id => id;
		[Min(0)][SerializeField] private int id;
		public int Level => level;
		[Min(0)][SerializeField] private int level;
		public int Duration => duration;
		[Min(1)][SerializeField] private int duration = 180;
		public bool IsStackable => isStackable;
		[SerializeField] private bool isStackable = false;
		public bool IsPositive => isPositive;
		[SerializeField] private bool isPositive = true;
		public bool CanBuy => canBuy;
		[SerializeField] private bool canBuy = true;
		public bool IsDestroyable => isDestroyable;
		[SerializeField] private bool isDestroyable = true;
		public Wallet Price => price;
		[SerializeField] private Wallet price = new();
		public Wallet RemovePrice => GetRemovePrice();

		public PhysicalStats Stats => stats;
		[SerializeField] private PhysicalStats stats;
		public int Stacks => stacks;
		[HideInInspector][SerializeField] private int stacks = 1;
		#endregion fields & properties

		#region methods
		public void StackFrom(Effect effectData)
		{
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
			int durationTime = effectData.duration;
			if (playerInventory.ContainItem(209))
				durationTime /= 2;
			if (playerInventory.ContainItem(291) && !IsPositive)
				durationTime *= 10;

			if (!isStackable)
			{
				duration = durationTime;
				return;
			}
			duration += durationTime;
			stats.IncreaseStatsHidden(effectData.stats);
			++stacks;
			OnEffectStacked?.Invoke(effectData);
		}
		public void DecreaseDuration(int seconds)
		{
			seconds = Mathf.Max(seconds, 0);
			duration -= seconds;
			OnDurationChanged?.Invoke(duration);
		}
		public void IncreaseDuration(int seconds)
		{
			seconds = Mathf.Max(seconds, 0);
			duration += seconds;
			OnDurationChanged?.Invoke(duration);
		}
		private Wallet GetRemovePrice() => canBuy ? GetPossibleRemovePrice() : GetNonBuyableRemovePrice();
		private Wallet GetPossibleRemovePrice()
		{
			Wallet wallet = new();
			float divideScale = 2f / (float)stacks;
			wallet = ScalePriceByTime(wallet);
			wallet.WeakSouls = DivideSellPrice(price.WeakSouls, divideScale);
			wallet.NormalSouls = DivideSellPrice(price.NormalSouls, divideScale);
			wallet.StrongSouls = DivideSellPrice(price.StrongSouls, divideScale);
			wallet.UniqueSouls = DivideSellPrice(price.UniqueSouls, divideScale);
			wallet.LegendarySouls = DivideSellPrice(price.LegendarySouls, divideScale);
			return wallet;
		}
		private Wallet GetNonBuyableRemovePrice()
		{
			Wallet wallet = price.Clone();
			wallet = ScalePriceByTime(wallet);
			return wallet;
		}
		private Wallet ScalePriceByTime(Wallet input)
		{
			if (isPositive) return input;
			int defaultTime = EffectsInfo.Instance.GetEffect(id).duration;
			int timeLasts = duration;
			if (defaultTime > timeLasts) return input;
			int priceMult = timeLasts / defaultTime;
			input.WeakSouls *= priceMult;
			input.NormalSouls *= priceMult;
			input.StrongSouls *= priceMult;
			input.UniqueSouls *= priceMult;
			input.LegendarySouls *= priceMult;
			return input;
		}
		private int DivideSellPrice(float value, float divideScale) => Mathf.RoundToInt(value / divideScale);
		public Effect Clone()
		{
			Effect effect = new();
			effect.sprite = sprite;
			effect.id = id;
			effect.level = level;
			effect.duration = duration;
			effect.isStackable = isStackable;
			effect.canBuy = canBuy;
			effect.isDestroyable = isDestroyable;
			effect.stats = stats.Clone();
			effect.price = price.Clone();
			effect.stacks = stacks;
			effect.isPositive = isPositive;
			return effect;
		}
		//public void ChangeID(int id) => this.id = id;
		#endregion methods
	}
}