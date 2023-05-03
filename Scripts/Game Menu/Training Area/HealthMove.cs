using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeakSoul.GameMenu.TrainingArea
{
    public class HealthMove : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] Vector2 localPositionsX;
        [SerializeField] private PanelInfo panelInfo;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            HealthMiniGame.Instance.OnGameEnd += StopMoving;
            HealthMiniGame.Instance.OnGameRestart += Restart;
            StopMoving();
        }
        private void OnDisable()
        {
            HealthMiniGame.Instance.OnGameEnd -= StopMoving;
            HealthMiniGame.Instance.OnGameRestart -= Restart;
            StopMoving();
        }
        private void Restart()
        {
            StopMoving();
            MoveToEnd();
        }
        private void MoveToEnd()
        {
            float step = (localPositionsX.y - localPositionsX.x) * Time.deltaTime;
            Vector3 localPos = transform.localPosition;
            if (panelInfo.CurrentLevelData != null)
                localPos.x += step / panelInfo.CurrentLevelData.TimeDeviation;
            transform.localPosition = localPos;
            Invoke(nameof(MoveToEnd), Time.deltaTime);
        }
        private void StopMoving()
        {
            CancelInvoke(nameof(MoveToEnd));
            Vector3 localPos = transform.localPosition;
            localPos.x = localPositionsX.x;
            transform.localPosition = localPos;
        }
        #endregion methods
    }
}