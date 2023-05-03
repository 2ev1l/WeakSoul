using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class EvasionZone : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private MiniGame miniGame;
        [SerializeField] private Vector4 randomRect;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            miniGame.OnGameRestart += RandomizePosition;
            RandomizePosition();
        }
        private void OnDisable()
        {
            miniGame.OnGameRestart -= RandomizePosition;
        }
        private void RandomizePosition()
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = Random.Range(randomRect.x, randomRect.z);
            localPos.y = Random.Range(randomRect.y, randomRect.w);
            transform.localPosition = localPos;
        }
        #endregion methods
    }
}