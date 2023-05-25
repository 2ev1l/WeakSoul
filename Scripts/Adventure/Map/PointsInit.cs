using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace WeakSoul.Adventure.Map
{
    public class PointsInit : SingleSceneInstance
    {
        #region fields & properties
        public UnityAction OnStartGeneration;
        public UnityAction OnEndGeneration;
        public UnityAction OnLoaded;
        public static PointsInit Instance { get; private set; }

        private static List<Point> generatedPoints = new();
        public static IEnumerable<PointData> GeneratedPointsData => generatedPointsData;
        private static List<PointData> generatedPointsData = new();
        public static bool IsSoulItem_AllPointsOpen { get; private set; }
        public static bool IsBossMustBeBeaten { get; private set; }

        [SerializeField] private Point pointPrefab;
        [SerializeField] private PointLoader pointLoaderPrefab;
        [SerializeField] private Transform spawnCanvas;
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private CanvasGroup blackScreenCanvas;
        [SerializeField] private GameObject finalZone;

        [field: SerializeField] public Material IconDefaultMaterial { get; private set; }
        [field: SerializeField] public Material IconChoosedMaterial { get; private set; }
        [field: SerializeField] public Sprite IconUnknown { get; private set; }

        private List<int> bossesGenerated = new();
        private List<int> citiesGenerated = new();
        private bool isFinalGenerated = false;
        private bool isGenerated = false;
        public bool IsGenerating { get; private set; }
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            IsSoulItem_AllPointsOpen = GameData.Data.PlayerData.Inventory.ContainItem(77);
            AdventureData adventureData = GameData.Data.AdventureData;
            IsBossMustBeBeaten = adventureData.IsBossNotDefeatedWhenMust(out _);
            finalZone.SetActive(adventureData.IsBossDefeated(2));
            StartCoroutine(CheckGeneratedPoints());
            blackScreenCanvas.gameObject.SetActive(true);
        }

        public static void ClearGeneratedData()
        {
            generatedPoints = new();
            generatedPointsData = new();
        }

        private IEnumerator CheckGeneratedPoints()
        {
            IsGenerating = true;
            if (generatedPointsData.Count > 0)
                LoadPoints();
            else
            {
                yield return GeneratePoints();
                DeleteAllPoints();
                LoadPoints();
                OnEndGeneration?.Invoke();
                IsGenerating = false;
            }
            //DEBUG//
            //print(generatedPointsData.Where(x => x.ChoosedEvent.Id == 6).Count() / (float)generatedPointsData.Count() + " blacksmith");
            //print(generatedPointsData.Where(x => x.ChoosedEvent.Id == 3).Count() / (float)generatedPointsData.Count() + " shop");
            //generatedPointsData.Where(x => x.ChoosedEvent.Id == 9).ToList().ForEach(x => print($"{x.ChoosedZone.Value} Boss at {x.PointId}"));
        }
        private void DeleteAllPoints()
        {
            foreach (var point in generatedPoints)
                Destroy(point.gameObject);
            generatedPoints.Clear();
        }
        public Point InstantiatePoint(Vector3 position)
        {
            Point point = Instantiate(pointPrefab, position, Quaternion.identity, spawnCanvas) as Point;
            CorrectPointPosition(point.gameObject);
            return point;
        }
        private PointLoader InstantiateLoadingPoint(Vector3 position)
        {
            PointLoader point = Instantiate(pointLoaderPrefab, position, Quaternion.identity, spawnCanvas) as PointLoader;
            CorrectPointPosition(point.gameObject);
            return point;
        }
        private GameObject CorrectPointPosition(GameObject inst)
        {
            Vector3 pos = inst.transform.localPosition;
            pos.z = 0;
            inst.transform.localPosition = pos;
            inst.transform.SetParent(spawnTransform);
            return inst;
        }
        public int GetFreePointId() => generatedPoints.Count;
        public void AddPoint(Point point)
        {
            generatedPoints.Add(point);
        }
        public void AddPointData(Point point)
        {
            generatedPointsData.Add(point.Data.Clone());
            EndLoading();
        }
        private void EndLoading()
        {
            CancelInvoke(nameof(OnEndLoad));
            Invoke(nameof(OnEndLoad), 0.5f);
        }
        private void OnEndLoad()
        {
            isGenerated = true;
        }
        public Point GetPoint(int index) => generatedPoints.Find(x => x.Data.PointId == index);
        public PointData GetPointData(int index) => generatedPointsData.Find(x => x.PointId == index);
        private void LoadPoints()
        {
            generatedPoints = new();
            for (int i = generatedPointsData.Count - 1; i >= 0; --i)
            {
                PointData data = generatedPointsData[i];
                PointLoader loader = InstantiateLoadingPoint(data.Position);
                generatedPoints.Add(loader);
                loader.Load(data);
            }
            OnLoaded?.Invoke();
            StartCoroutine(LoadUI());
        }
        private IEnumerator LoadUI()
        {
            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(1, 0, 2);
            while (!vsc.IsChangeEnded)
            {
                blackScreenCanvas.alpha = vsc.Out;
                yield return CustomMath.WaitAFrame();
            }
            blackScreenCanvas.gameObject.SetActive(false);
            blackScreenCanvas.alpha = 1;
            Destroy(vsc);
        }
        private IEnumerator GeneratePoints()
        {
            OnStartGeneration?.Invoke();
            isGenerated = false;
            ClearGeneratedData();
            InstantiatePoint(Vector3.zero);
            while (!isGenerated)
                yield return CustomMath.WaitAFrame();
        }

        private bool IsBossGenerated(int bossId) => bossesGenerated.Contains(bossId);
        private bool IsCityGenerated(int cityId) => citiesGenerated.Contains(cityId);
        public bool TryGenerateBoss(int bossId)
        {
            if (IsBossGenerated(bossId) || GameData.Data.AdventureData.IsBossDefeated(bossId))
                return false;
            bossesGenerated.Add(bossId);
            return true;
        }
        public bool TryGenerateCity(int cityId)
        {
            if (IsCityGenerated(cityId))
                return false;
            citiesGenerated.Add(cityId);
            return true;
        }
        public bool TryGenerateFinal()
        {
            if (isFinalGenerated) return false;
            isFinalGenerated = true;
            return true;
        }
        #endregion methods
    }
}