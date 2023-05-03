using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using WeakSoul.Adventure.Map;

namespace WeakSoul.Adventure
{
	public class Compass : MonoBehaviour
	{
		#region fields & properties
		[SerializeField] private GameObject centerDirectionArrow;
		#endregion fields & properties

		#region methods
		private void CheckCompass()
		{
			bool isSoulItem_Compass = GameData.Data.PlayerData.Inventory.ContainItem(7);
			centerDirectionArrow.SetActive(isSoulItem_Compass);
			StartCoroutine(CompassCheck());
		}
		private IEnumerator CompassCheck()
		{
			while (Player.Instance == null)
				yield return CustomMath.WaitAFrame();
			CustomAnimation.LookAt2D(centerDirectionArrow.transform, Player.Instance.transform.position, Vector3.zero);
		}
		private void CheckCompass(int _1, int _2) => CheckCompass();
		private void CheckCompass(int _1, int _2, int _3) => CheckCompass();
		private void OnEnable()
		{
			Player.OnPointChanged += CheckCompass;
			GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckCompass;
			CheckCompass();
		}
		private void OnDisable()
		{
			Player.OnPointChanged -= CheckCompass;
			GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckCompass;
		}
		#endregion methods
	}
}