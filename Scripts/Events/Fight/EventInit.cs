using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Fight
{
    public class EventInit : SingleSceneInstance
    {
        #region fields & properties
        public static EventInit Instance { get; private set; }
        [SerializeField] private AudioClip mainMusic;
        [SerializeField] private EventStorage storage;
        [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
        [SerializeField] private List<Sprite> dungeonSprites;

        [SerializeField] private PlayerCard player;
        [SerializeField] private EnemyCard enemy;
        [SerializeField] private GameObject uniqueFightEffect;
        [SerializeField] private GameObject bossFightEffect;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            Instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            Init();
        }
        private void Init()
        {
            AudioManager.AddAmbient(mainMusic, false, 0.4f);
            Sprite rnd = EventInfo.Instance.Data.Event.Id == 2 ? dungeonSprites[Random.Range(0, dungeonSprites.Count)] : storage.GetRandomSprite();
            bgSpriteRenderers.ForEach(x => x.sprite = rnd);
            player.Init(-1);
            try { enemy.Init(EventInfo.Instance.Data.BattleData.Fights.First().EnemyId); }
            catch (System.Exception e)
            {
                Debug.LogError($"Error - Exception on spawn. Fixing - Generate. \n{e}");
                EventInfo.Instance.Data.GenerateBattleData();
                enemy.Init(EventInfo.Instance.Data.BattleData.Fights.First().EnemyId);
            }
            if (enemy.EnemyData.Type != EnemyType.Default)
                storage.InstantiateEffect(uniqueFightEffect, uniqueFightEffect.transform.position);
            if (enemy.EnemyData.Type == EnemyType.Boss)
                storage.InstantiateEffect(bossFightEffect, bossFightEffect.transform.position);
        }
        public void NextBattleOrLeave(bool isLastBattle)
        {
            if (isLastBattle)
            {
                AudioManager.RemoveAmbient();
                SceneLoader.Instance.LoadSceneFade("Adventure", 2f);
            }
            else
                SceneLoader.Instance.LoadSceneFade("Events", 2f);
        }
        [ContextMenu("Fight tutorial on")]
        private void FTON() => GameData.Data.TutorialData.IsFightCompleted = false;
        [ContextMenu("Fight tutorial off")]
        private void FTOFF() => GameData.Data.TutorialData.IsFightCompleted = false;
        [ContextMenu("Save")]
        private void Save() => SavingUtils.SaveGameData();
        #endregion methods
    }
}