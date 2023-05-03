using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class ProgressText : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private PanelInfo panelInfo;
        [SerializeField] private Text txt;
        [SerializeField] private GameObject expPrefab;
        [SerializeField] private LevelGainText levelPrefab;
        [SerializeField] private Canvas spawn;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            panelInfo.PlayerStatLevel.OnExperienceChanged += CheckProgress;
            panelInfo.PlayerStatLevel.OnExperienceChanged += SpawnExpPrefab;
            panelInfo.PlayerStatLevel.OnLevelChanged += SpawnLevelPrefab;
            CheckProgress(panelInfo.PlayerStatLevel.Experience);
        }
        private void OnDisable()
        {
            panelInfo.PlayerStatLevel.OnExperienceChanged -= CheckProgress;
            panelInfo.PlayerStatLevel.OnExperienceChanged -= SpawnExpPrefab;
            panelInfo.PlayerStatLevel.OnLevelChanged -= SpawnLevelPrefab;
        }
        private void CheckProgress(int experience)
        {
            txt.text = $"{experience}/{(panelInfo.PlayerStatLevel.Level > panelInfo.PlayerStatLevel.MaxLevel ? "Inf" : $"{(panelInfo.CurrentLevelData == null ? "???" : panelInfo.CurrentLevelData.ExpToNext)}")}";
        }
        private void SpawnExpPrefab(int exp)
        {
            GameObject prefab = Instantiate(expPrefab, transform.position, Quaternion.identity, spawn.transform);
            Vector3 pos = prefab.transform.localPosition;
            pos.z = 0;
            prefab.transform.localPosition = pos;
        }
        private void SpawnLevelPrefab(int level)
        {
            LevelGainText prefab = Instantiate(levelPrefab, transform.position, Quaternion.identity, spawn.transform) as LevelGainText;
            Vector3 pos = prefab.transform.localPosition;
            pos.z = 0;
            prefab.transform.localPosition = pos;
            prefab.StatsType = panelInfo.Type;
        }
        #endregion methods
    }
}