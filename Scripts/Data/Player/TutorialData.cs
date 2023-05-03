using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [System.Serializable]
    public class TutorialData
    {
        #region fields & properties
        [SerializeField] private List<TutorialFlag> tutorialFlags = new();
        public bool IsCompleted
        {
            get => isCompleted;
            set => isCompleted = value;
        }
        [SerializeField] private bool isCompleted = false;
        public bool IsFightCompleted
        {
            get => isFightCompleted;
            set => isFightCompleted = value;
        }
        [SerializeField] private bool isFightCompleted = false;
        
        public static int Progress
        {
            get => progress;
            set => SetProgress(value);
        }
        [NonSerialized] private static int progress = 0;
        public static int FightProgress
        {
            get => fightProgress;
            set => SetFightProgress(value);
        }
        [NonSerialized] private static int fightProgress = 0;
        #endregion fields & properties

        #region methods
        public bool TryAddNewFlag(TutorialFlag flag)
        {
            if(tutorialFlags.Contains(flag)) return false;
            tutorialFlags.Add(flag);
            return true;
        }
        public static void ResetProgresses()
        {
            Progress = 0;
            FightProgress = 0;
        }
        private static void SetProgress(int value)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException("Tutorial progress");
            progress = value;
        }
        private static void SetFightProgress(int value)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException("Tutorial fight progress");
            fightProgress = value;
        }
        #endregion methods
    }
}