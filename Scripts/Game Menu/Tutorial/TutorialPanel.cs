using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Universal;

namespace WeakSoul.GameMenu
{
    public abstract class TutorialPanel : MonoBehaviour
    {
        #region fields & properties
        protected GameObject TextPanel => textPanel;
        [SerializeField] private GameObject textPanel;
        [SerializeField] private LanguageLoader languageText;
        protected abstract int Progress { get; }
        #endregion fields & properties

        #region methods
        protected virtual void Start()
        {
            CheckTutorial();
        }
        protected abstract void CheckTutorial();
        public void CheckTutorialStep(int progress) => TryCheckTutorialStep(progress);
        public bool TryCheckTutorialStep(int progress)
        {
            if (Progress != progress) return false;
            CheckTutorialStep();
            return true;
        }
        public abstract void CheckTutorialStep();
        public virtual void HidePanel() => textPanel.SetActive(false);
        public virtual void ShowPanel(int languageId, string additionalText = "")
        {
            textPanel.SetActive(true);
            languageText.Id = languageId;
            languageText.AddText(additionalText);
        }
        protected VideoPlayer InitializePlayer(VideoPlayer playerPrefab, Transform spawnCanvas, VideoClip clip)
        {
            VideoPlayer spawnedPlayer = SpawnVideoPlayer(playerPrefab, spawnCanvas);
            spawnedPlayer.clip = clip;
            spawnedPlayer.Play();
            WaitForPlayerActive(spawnedPlayer);
            return spawnedPlayer;
        }
        protected VideoPlayer SpawnVideoPlayer(VideoPlayer playerPrefab, Transform spawnCanvas)
        {
            VideoPlayer player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, spawnCanvas);
            Vector3 localPos = player.transform.localPosition;
            localPos.z = 0;
            player.transform.localPosition = localPos;
            return player;
        }
        protected void WaitForPlayerActive(VideoPlayer player) => StartCoroutine(WaitForPlayer(player));
        private IEnumerator WaitForPlayer(VideoPlayer player)
        {
            while (player != null)
            {
                if (!player.isPrepared)
                    yield return CustomMath.WaitAFrame();
                else
                    break;
            }
            if (player == null) yield break;

            ValueSmoothChanger vsc = gameObject.AddComponent<ValueSmoothChanger>();
            vsc.StartChange(0, 1, 1);
            SpriteRenderer sr = player.transform.GetComponentInChildren<SpriteRenderer>();
            while (!vsc.IsChangeEnded)
            {
                if (sr == null) break;
                Color col = sr.color;
                col.a = vsc.Out;
                sr.color = col;
                yield return CustomMath.WaitAFrame();
            }
            Destroy(vsc);
        }
        #endregion methods
    }
}