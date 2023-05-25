using Data;
using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using Universal;

namespace WeakSoul.Adventure.Map
{
	public class Player : SingleSceneInstance
	{
		#region fields & properties
		public static Player Instance { get; private set; }
		public static UnityAction<int> OnPointStep;
		/// <summary>
		/// <see cref="{T0}"/> pointId;
		/// </summary>
		private static UnityAction<int> OnCurrentPointIdChanged;
		/// <summary>
		/// <see cref="{T0}"/> oldPointId;
		/// <see cref="{T1}"/> newPointId;
		/// </summary>
		public static UnityAction<int, int> OnPointChanged;
		/// <summary>
		/// <see cref="{T0}"/> from PointId;
		/// </summary>
		public static UnityAction<int> OnStartMoving;
		/// <summary>
		/// <see cref="{T0}"/> to PointId;
		/// </summary>
		public static UnityAction<int> OnMovingTo;
		public static UnityAction<SubZoneData> OnSubZoneChanged;
		public static int CurrentPointId
		{
			get => currentPointId;
			set
			{
				oldPointId = CurrentPointId;
				currentPointId = value;
				OnCurrentPointIdChanged?.Invoke(value);
			}
		}
		private static int currentPointId = 0;
		private static int oldPointId = 0;

		[SerializeField] private BoxCollider2D boxCollider2D;
		public SubZoneData SubZoneData => subZoneData;
		[SerializeField][ReadOnly] private SubZoneData subZoneData = new();
		[SerializeField][ReadOnly] private PointLoader currentPoint;

		[Header("Soul Items")]
		[SerializeField] private Light pointLight;
		[SerializeField] private Light directionalLight;
		private static readonly float lightDefaultRange = 1.5f;
		#endregion fields & properties

		#region methods
		protected override void Awake()
		{
			Instance = this;
			CheckInstances(GetType());
		}
		private void OnEnable()
		{
			PointsInit.Instance.OnEndGeneration += ResetPosition;
			PointsInit.Instance.OnLoaded += OnLoaded;
			OnCurrentPointIdChanged += MoveToPoint;
			OnStartMoving += ResetSubZone;
			OnMovingTo += OpenPoint;
			OnPointChanged += InvokeZoneChange;
			GameData.Data.PlayerData.Inventory.OnInventoryChanged += CheckSoulItems;
			CheckSoulItems();
		}
		private void OnDisable()
		{
			PointsInit.Instance.OnEndGeneration -= ResetPosition;
			PointsInit.Instance.OnLoaded -= OnLoaded;
			OnCurrentPointIdChanged -= MoveToPoint;
			OnStartMoving -= ResetSubZone;
			OnMovingTo -= OpenPoint;
			OnPointChanged -= InvokeZoneChange;
			GameData.Data.PlayerData.Inventory.OnInventoryChanged -= CheckSoulItems;
		}
		private void CheckSoulItems(int _1, int _2, int _3) => CheckSoulItems();
		private void CheckSoulItems()
		{
			CheckDirectionalLight();
			CheckPointLight();
		}
		private void CheckDirectionalLight()
		{
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			bool isSoulItem_LightAll = inventory.ContainItem(78);
			directionalLight.intensity = isSoulItem_LightAll ? 0.0033f : 0f;
		}
		private void CheckPointLight()
		{
			ItemsInventory inventory = GameData.Data.PlayerData.Inventory;
			bool isSoulItem_Burner1 = inventory.ContainItem(9);
			bool isSoulItem_Burner2 = inventory.ContainItem(10);
			bool isSoulItem_Burner3 = inventory.ContainItem(11);
			bool isSoulItem_Burner4 = inventory.ContainItem(12);
			float lightScale = 0f;
			lightScale = isSoulItem_Burner1 ? 0.25f : lightScale;
			lightScale = isSoulItem_Burner2 ? 0.5f : lightScale;
			lightScale = isSoulItem_Burner3 ? 0.75f : lightScale;
			lightScale = isSoulItem_Burner4 ? 1f : lightScale;
			pointLight.range = lightDefaultRange + lightScale;
		}
		private void InvokeZoneChange(int oldPointId, int newPointId)
		{
			OnSubZoneChanged?.Invoke(subZoneData);
		}
		private void ResetSubZone(int oldPointId)
		{
			boxCollider2D.enabled = false;
			subZoneData = new();
			boxCollider2D.enabled = true;
		}
		private void OnLoaded()
		{
			MoveToPoint(CurrentPointId, true, 0f);
			StartCoroutine(ResetCamera());
		}
		private IEnumerator ResetCamera()
		{
			yield return CustomMath.WaitAFrame();
			MapReset.Instance.ResetCamera();
		}
		private void ResetPosition()
		{
			OpenPoint(0);
			currentPointId = 0;
		}
		private void OpenPoint(int newPointId)
		{
			PointsInit.Instance.GetPointData(newPointId).Open();
		}
		private void MoveToPoint(int pointId)
		{
			MoveToPoint(pointId, true, 1f);
		}
		private void MoveToPoint(int pointId, bool invokeAction = true, float time = 1f) => StartCoroutine(MoveTo(pointId, invokeAction, time));
		public void MoveToDirections(params Direction[] directions) => StartCoroutine(MoveTo(directions));
		private IEnumerator MoveTo(params Direction[] directions)
		{
			oldPointId = currentPointId;
			OnStartMoving?.Invoke(oldPointId);

			Point nextPoint = currentPoint;
			foreach (var el in directions)
			{
				Direction current = el;
				if (current == Direction.RND)
					current = (Direction)Random.Range(0, 8);
				nextPoint = PointsInit.Instance.GetPoint(currentPointId);
				DirectionInfo di = nextPoint.Data.DirectionsInfo.GetDirection(current);
				if (di.PointId != -1)
				{
					nextPoint = di.Point;
					currentPointId = di.PointId;
					Vector3 position = nextPoint.Data.Position;
					OnMovingTo?.Invoke(nextPoint.Data.PointId);
					yield return CustomAnimation.MoveTo(position, gameObject.transform, 1);
				}
				yield return CheckMovedZone(nextPoint);
			}
			OnPointChanged?.Invoke(oldPointId, currentPointId);
		}
		private IEnumerator MoveTo(int pointId, bool invokeAction = true, float time = 1f)
		{
			if (invokeAction)
				OnStartMoving?.Invoke(oldPointId);
			OnMovingTo?.Invoke(pointId);
			Point point = PointsInit.Instance.GetPoint(pointId);
			Vector3 position = point.Data.Position;
			yield return CustomAnimation.MoveTo(position, gameObject.transform, time);
			yield return CheckMovedZone(point);
			if (invokeAction)
				OnPointChanged?.Invoke(oldPointId, currentPointId);
		}
		private IEnumerator CheckMovedZone(Point point)
		{
			OnPointStep?.Invoke(point.Data.PointId);
            if (point.Data.ChoosedEvent.SpawnZone != SpawnZone.Water) yield break;
			DirectionInfo di = point.Data.DirectionsInfo.GetNonWaterDirection();
			currentPointId = di.PointId;
			yield return MoveTo(di.PointId, false);
		}
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent(out SubZone subZone))
			{
				if (subZoneData.SubZone != SpawnSubZone.Home)
					subZoneData = subZone.Data;
			}
			if (collision.TryGetComponent(out PointLoader pointLoader))
			{
				if (pointLoader.Data.PointId == currentPointId)
					currentPoint = pointLoader;
			}
		}


		[SerializeField] private int pointToMove;
		[ContextMenu("to point")]
		private void MoveToPoint()
		{
			CurrentPointId = pointToMove;
		}

		[SerializeField] private Direction[] directionToMove;
		[ContextMenu("to direction")]
		private void MoveToDirection()
		{
			MoveToDirections(directionToMove);
		}
		#endregion methods
	}
}