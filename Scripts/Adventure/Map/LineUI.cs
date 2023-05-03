using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeakSoul.Adventure.Map
{
    public class LineUI : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private List<GameObject> lines = new List<GameObject>();
        [SerializeField] private List<GameObject> texts = new List<GameObject>();
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            lines.ForEach(x => x.SetActive(false));
            texts.ForEach(x => x.SetActive(false));

            List<int> bossesDefeated = GameData.Data.AdventureData.BossesDefeated.ToList();
            lines[0].SetActive(true);
            texts[0].SetActive(true);
            for (int i = 0; i < bossesDefeated.Count; ++i)
            {
                if (i + 1 < lines.Count)
                    lines[i + 1].SetActive(true);
                if (i + 1 < texts.Count)
                    texts[i + 1].SetActive(true);
            }
        }
        #endregion methods
    }
}