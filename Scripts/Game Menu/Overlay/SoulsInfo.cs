using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu
{
	public class SoulsInfo : MonoBehaviour
	{
		#region fields & properties
		[SerializeField] private Wallet soulsToSet;
		[field: SerializeField] public List<SoulInfo> Souls { get; private set; }
		public static SoulsInfo Instance { get; private set; }
		[field: SerializeField] public Material NullMaterial { get; private set; }
		[field: SerializeField] public Material NormalMaterial { get; private set; }
		#endregion fields & properties

		#region methods
		public void Init()
		{
			Instance = this;
		}
		public SoulInfo GetInfo(SoulType soulType) => Souls.Find(x => x.SoulType == soulType);
		[ContextMenu("Set souls")]
		private void SS()
		{
			Wallet playerWallet = GameData.Data.PlayerData.Wallet;
			playerWallet.WeakSouls = soulsToSet.WeakSouls;
			playerWallet.NormalSouls = soulsToSet.NormalSouls;
			playerWallet.StrongSouls = soulsToSet.StrongSouls;
			playerWallet.UniqueSouls = soulsToSet.UniqueSouls;
			playerWallet.LegendarySouls = soulsToSet.LegendarySouls;
		}
		#endregion methods
	}
}