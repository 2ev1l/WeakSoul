using Data;
using Data.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.Events.Camp
{
    public class Camp : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private LanguageLoader descriptionLanguage;

        [SerializeField] private Transform spawnCanvas;
        [SerializeField] private StatText textPrefab;

        [Header("Debug")]
        [SerializeField][ReadOnly] CampEvent data;
        #endregion fields & properties

        #region methods
        public void Init(CampEvent @event)
        {
            data = @event;
            descriptionLanguage.Id = data.LanguageDescriptionId;
        }
        public void Rest()
        {
            PlayerStats playerStats = GameData.Data.PlayerData.Stats;
            PhysicalStats appliedStats = data.GetBuff(playerStats);
            CheckPlayerItems(appliedStats);
            playerStats.IncreaseStatsHidden(appliedStats);
            StartCoroutine(RestAnimation(appliedStats));
        }
        private void CheckPlayerItems(PhysicalStats appliedStats)
        {
			ItemsInventory playerInventory = GameData.Data.PlayerData.Inventory;
            bool IsSoulItem_202 = playerInventory.ContainItem(202);
            if (IsSoulItem_202)
            {
                appliedStats.Health += CustomMath.Multiply(appliedStats.Health, 10);
            }
		}
		private IEnumerator RestAnimation(PhysicalStats appliedStats)
        {
            foreach (var el in appliedStats.GetEnabledStatsList())
            {
                float time = StatText.SpawnPrefab(textPrefab, spawnCanvas, Vector3.zero, el, appliedStats.GetStatsByType(el)).AnimationTime;
                yield return new WaitForSecondsRealtime(time / 2f);
            }
            Leave();
        }
        
        public void Leave()
        {
            SceneLoader.Instance.LoadSceneFade("Adventure", 2);
        }
        #endregion methods
    }
}