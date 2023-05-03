using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    public class EnemiesInfo : MonoBehaviour
    {
        #region fields & properties
        public static EnemiesInfo Instance { get; private set; }
        [SerializeField] private List<EnemySO> enemies;
        public IEnumerable<EnemyData> EnemiesData => enemiesData;
        private List<EnemyData> enemiesData = new();
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
            enemies.ForEach(x => enemiesData.Add(x.EnemyData));
            enemiesData = enemiesData.OrderBy(x => x.Id).ToList();
        }
        public EnemyData GetEnemy(int enemyId) => enemies[enemyId].EnemyData;
        public EnemyData GetBoss(SpawnSubZone bossZone) => enemiesData.Find(x => x.Type == EnemyType.Boss && x.IsAllowedToSpawn(bossZone));
        public IEnumerable<EnemyData> GetAllowedEnemies(SpawnSubZone spawnSubZone) => enemiesData.Where(x => x.IsAllowedToSpawn(spawnSubZone));
        
        [ContextMenu("Get All")]
        private void GetAll()
        {
            enemies = Resources.FindObjectsOfTypeAll<EnemySO>().OrderBy(x => x.EnemyData.Id).ToList();
            foreach (var el in enemies)
            {
                if (enemies.Where(x => x.EnemyData.Id == el.EnemyData.Id).Count() > 1)
                    Debug.Log($"Error enemy id {el.EnemyData.Id} at {el.name}");
                if (!el.EnemyData.ContainHomeZone())
                    Debug.Log($"{el.EnemyData.Id} id - doesn't contain home spawnzone.");
                if (el.EnemyData.Skills.Size != el.EnemyData.Skills.GetFilledItems().Count())
                    Debug.Log($"{el.EnemyData.Id} id - skills size = {el.EnemyData.Skills.Size}, and count = {el.EnemyData.Skills.GetFilledItems().Count()}");
				
                foreach (var reward in el.EnemyData.Rewards)
				{
					if (reward.Count == 0)
						Debug.LogError($"Error reward count at {el.name}");
					if (reward.Chance < 0.01f)
						Debug.LogError($"Error reward chance at {el.name}");
					if (TextData.Instance != null)
					{
						try { LanguageLoader.GetRewardTextByType(reward.Type, reward.Id); }
						catch { Debug.LogError($"Error text in {el.name} - {reward.Type} x {reward.Id}"); }
					}
				}
			}
        }
#if false
        [ContextMenu("Create")]
        private void CreateAll()
        {
            string path = "Assets/Resources/Scriptable Object/Enemies/Enemy ";
            for (int i = 78; i < 109; ++i)
            {
                EnemySO cardSO = ScriptableObject.CreateInstance<EnemySO>();
                cardSO.EnemyData.ChangeID(i);
                string newPath = $"{path}{i}.asset";
				AssetDatabase.CreateAsset(cardSO, newPath);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		#endregion methods
	}
}