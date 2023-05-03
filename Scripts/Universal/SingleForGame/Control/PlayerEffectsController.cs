using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class PlayerEffectsController : MonoBehaviour
    {
        #region fields & properties
        public static readonly List<string> DisabledScenes = new List<string>() { "Main Menu", "Cut Scene" };
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            GameData.Data.OnTimePlayedChanged += DecreaseEffectsTime;
        }
        private void OnDisable()
        {
            GameData.Data.OnTimePlayedChanged -= DecreaseEffectsTime;
        }
        private void DecreaseEffectsTime(int currentTime, int increasedTime)
        {
            if (DisabledScenes.Contains(SceneManager.GetActiveScene().name)) return;

            IEnumerable<Effect> effects = GameData.Data.PlayerData.Stats.Effects;
            int effectsCount = effects.Count();
            try
            {
                foreach (Effect el in effects)
                {
                    el.DecreaseDuration(increasedTime);
                    if (el.Duration <= 0)
                        GameData.Data.PlayerData.Stats.RemoveEffect(el, true);
                }
            }
            catch { }
        }
        #endregion methods
    }
}