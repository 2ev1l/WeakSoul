using UnityEngine;
using Data;
using WeakSoul.MainMenu;
using WeakSoul.GameMenu;
using WeakSoul.GameMenu.Inventory;
using WeakSoul;
using WeakSoul.GameMenu.Skills;
using System.Collections.Generic;
using WeakSoul.GameMenu.Shop;
using WeakSoul.Adventure;
using Data.Events;
using WeakSoul.Adventure.Map;

namespace Universal
{
    public class SingleGameInstance : MonoBehaviour
    {
        #region fields
        private static bool isInitialized = false;
        private bool isMain = false;

        [SerializeField] private SavingUtils savingUtils;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioStorage audioStorage;
        [SerializeField] private TimeController timeController;
        [SerializeField] private CursorSettings cursorSettings;
        [SerializeField] private CustomAnimation customAnimation;
        [SerializeField] private TextData textData;
        [SerializeField] private DeadScreen deadScreen;
        [SerializeField] private OverlayController overlayController;
        [SerializeField] private PlayerStatsController playerStatsController;
        [SerializeField] private EquipmentUI equipmentUI;
		[SerializeField] private MapReset mapReset;
		[SerializeField] private SteamAchievements steamAchievements;

		[SerializeField] private PhysicalStatsItemListStorage statsStorage;
        [SerializeField] private ItemsInfo itemsInfo;
        [SerializeField] private SkillsInfo skillsInfo;
        [SerializeField] private EffectsInfo effectsInfo;
        [SerializeField] private RecipesInfo recipesInfo;
        [SerializeField] private ShopInfo shopInfo;
        [SerializeField] private LevelsInfo levelsInfo;
        [SerializeField] private SoulsInfo soulsStorage;
        [SerializeField] private MaterialsInfo materialsInfo;
        [SerializeField] private EventInfo eventInfo;
        [SerializeField] private EnemiesInfo enemiesInfo;

        [SerializeField] private List<CanvasInit> canvasInitList;
        [SerializeField] private List<CellController> cellControllers;
        [SerializeField] private List<HelpUpdater> helpUpdaters;
        #endregion fields

        #region methods
        private void OnEnable()
        {
            SavingUtils.OnDataReset += InitAfterReset;
            SceneLoader.OnSceneLoaded += Awake;
        }
        private void OnDisable()
        {
            SavingUtils.OnDataReset -= InitAfterReset;
            SceneLoader.OnSceneLoaded -= Awake;
        }

        private void Awake()
        {
            if (!isInitialized)
            {
                isMain = true;
                isInitialized = true;
                OnInitialize();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (!isMain)
                {
                    DestroyImmediate(gameObject);
                    return;
                }
                OnLoad();
            }
        }
        private void OnInitialize()
        {
            savingUtils.Init();
            InitAfterReset();
        }
        private void OnLoad()
        {
            sceneLoader.Start();
            audioManager.Start();
            savingUtils.Start();
            StartCoroutine(textData.Start());
            overlayController.Start();
            deadScreen.Start();
            canvasInitList.ForEach(x => x.Start());
        }
        public void InitAfterReset()
        {
            sceneLoader.Init(); ChangeObjectState(sceneLoader);
            audioStorage.Init();
            audioManager.Init();
            timeController.Init();
            customAnimation.Init();
            cursorSettings.Init(); ChangeObjectState(cursorSettings);
            textData.Init(); ChangeObjectState(textData);
            itemsInfo.Init(); ChangeObjectState(itemsInfo);
            skillsInfo.Init(); ChangeObjectState(skillsInfo);
            effectsInfo.Init(); ChangeObjectState(effectsInfo);
            recipesInfo.Init(); ChangeObjectState(recipesInfo);
            soulsStorage.Init(); ChangeObjectState(soulsStorage);
            statsStorage.Init(); ChangeObjectState(statsStorage);
            shopInfo.Init(); ChangeObjectState(shopInfo);
            levelsInfo.Init(); ChangeObjectState(levelsInfo);
            playerStatsController.Init(); ChangeObjectState(playerStatsController);
            materialsInfo.Init(); ChangeObjectState(materialsInfo);
            eventInfo.Init();
            enemiesInfo.Init(); ChangeObjectState(enemiesInfo);
			mapReset.Init(); ChangeObjectState(mapReset);

            ChangeObjectState(steamAchievements);
			ChangeObjectState(equipmentUI);

            canvasInitList.ForEach(x => x.Init()); canvasInitList.ForEach(x => ChangeObjectState(x));
            cellControllers.ForEach(x => x.Init()); cellControllers.ForEach(x => ChangeObjectState(x));
            helpUpdaters.ForEach(x => x.Init()); helpUpdaters.ForEach(x => ChangeObjectState(x));
		}
        private void ChangeObjectState(Component component)
        {
            if (!component.gameObject.activeSelf) return;
            component.gameObject.SetActive(false);
            component.gameObject.SetActive(true);
        }
        #endregion methods
    }
}