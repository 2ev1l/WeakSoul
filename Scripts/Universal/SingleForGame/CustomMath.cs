using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Universal
{
    public class CustomMath : MonoBehaviour
    {
        #region methods
        public static IEnumerator WaitAFrame()
        {
            yield return Application.isBatchMode ? null : new WaitForEndOfFrame();
        }
        public static Vector2 GetScreenSquare() => (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position) switch
        {
            Vector3 vec when vec.x >= 0 && vec.y >= 0 => Vector2.right + Vector2.up,
            Vector3 vec when vec.x <= 0 && vec.y >= 0 => -Vector2.right + Vector2.up,
            Vector3 vec when vec.x <= 0 && vec.y <= 0 => -Vector2.right - Vector2.up,
            Vector3 vec when vec.x >= 0 && vec.y <= 0 => Vector2.right - Vector2.up,
            _ => -Vector2.right - Vector2.up
        };
        public static Vector3 Project(Vector3 direction, Vector3 projectiable) => direction - Vector3.Dot(direction, projectiable) * projectiable;

        /// <summary>
        /// True if chance gained.
        /// </summary>
        /// <param name="chancePercent">0..100%</param>
        /// <returns></returns>
        public static bool GetRandomChance(float chancePercent) => UnityEngine.Random.Range(0f, 100f) < chancePercent;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Value (0..100) </returns>
        public static float GetRandomChance() => Mathf.Clamp(UnityEngine.Random.Range(0f, 100f), 0.001f, 99.999f);
        /// <summary>
        /// True if chance gained.
        /// </summary>
        /// <param name="chancePercent">0..100%</param>
        /// <returns></returns>
        public static bool GetRandomChance(int chancePercent) => UnityEngine.Random.Range(0, 100) < chancePercent;
        public static int GetRandomFromChancesArray(params float[] chances)
        {
            int finalIndex = chances.Length - 1;
            float maxChance = 100;
            for (int i = 0; i < chances.Length; i++)
            {
                float rnd = UnityEngine.Random.Range(0, maxChance);
                if (rnd <= chances[i])
                {
                    finalIndex = i;
                    return finalIndex;
                }
                else
                {
                    maxChance -= chances[i];
                }
            }
            return -1;
        }
        public static float GetOptimalScreenScale()
        {
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            float scale = (width / (float)height) switch
            {
                float i when i < 1.01f => 0.5f,
                float i when i < 1.26f => 0.6f,
                float i when i < 1.46f => 0.7f,
                float i when i < 1.61f => 0.8f,
                float i when i < 1.75f => 0.9f,
                _ => 1f,
            };
            return scale;
        }
        /// <summary>
        /// Calculating rounded value with percent multiplier. e.g: 10 * 50(%) = 5
        /// </summary>
        /// <param name="value"></param>
        /// <param name="multiplier">0..any%</param>
        /// <returns></returns>
        public static int Multiply(int value, float multiplier) => Mathf.RoundToInt(value * multiplier / 100f);
        public static int GetMultipliedIncrease(int value, float multiplier) => Multiply(value, multiplier) - value;


        public static List<T> FindAllResults<T>(List<T> list, System.Func<T, int> findPattern, FindResult result)
        {
            int value = (result) switch
            {
                FindResult.Max => FindMax(list, findPattern),
                FindResult.Min => FindMin(list, findPattern),
                _ => throw new System.NotImplementedException()
            };
            List<T> newList = new List<T>();

            foreach (var el in list)
                if (findPattern.Invoke(el) == value)
                    newList.Add(el);
            return newList;
        }
        public static List<T> FindAllResults<T>(List<T> list, System.Func<T, float> findPattern, FindResult result)
        {
            float value = (result) switch
            {
                FindResult.Max => FindMax(list, findPattern),
                FindResult.Min => FindMin(list, findPattern),
                _ => throw new System.NotImplementedException()
            };
            List<T> newList = new List<T>();

            foreach (var el in list)
                if (findPattern.Invoke(el) == value)
                    newList.Add(el);
            return newList;
        }
        public static int FindMin<T>(List<T> list, System.Func<T, int> pattern)
        {
            int minValue = int.MaxValue;
            foreach (T el in list)
                if (pattern(el) < minValue)
                    minValue = pattern(el);
            return minValue;
        }
        public static int FindMax<T>(List<T> list, System.Func<T, int> pattern)
        {
            int maxValue = int.MinValue;
            foreach (T el in list)
                if (pattern(el) > maxValue)
                    maxValue = pattern(el);
            return maxValue;
        }
        public static int FindIndexByMax<T>(List<T> list, System.Func<T, int> pattern)
        {
            int maxValue = int.MinValue;
            int id = -1;
            foreach (T el in list)
                if (pattern(el) > maxValue)
                {
                    maxValue = pattern(el);
                    id = list.IndexOf(el);
                }
            return id;
        }
        public static float FindMin<T>(List<T> list, System.Func<T, float> pattern)
        {
            float minValue = float.MaxValue;
            foreach (T el in list)
                if (pattern(el) < minValue)
                    minValue = pattern(el);
            return minValue;
        }
        public static float FindMax<T>(List<T> list, System.Func<T, float> pattern)
        {
            float maxValue = float.MinValue;
            foreach (T el in list)
                if (pattern(el) > maxValue)
                    maxValue = pattern(el);
            return maxValue;
        }
        public static int FindMax<T>(List<T> list, System.Func<T, int> pattern, System.Func<T, bool> args)
        {
            int maxValue = -2147483647;
            foreach (T el in list)
                if (pattern(el) > maxValue && args.Invoke(el))
                    maxValue = pattern(el);
            return maxValue;
        }
        public static List<T> FindAllEquals<T>(List<T> oldList, System.Func<T, bool> predicate)
        {
            List<T> newList = new List<T>();
            foreach (var el in oldList)
                if (predicate.Invoke(el))
                    newList.Add(el);
            return newList;
        }

        public static List<T> CombineLists<T>(List<T> list1, List<T> list2)
        {
            List<T> result = new List<T>();
            try
            {
                foreach (var el in list1)
                    result.Add(el);
            }
            catch { }
            try
            {
                foreach (var el in list2)
                    if (!result.Contains(el))
                        result.Add(el);
            }
            catch { }
                return result;
        }
        public static bool GetLogicalResult(float comparingValue, LogicalOperation operation, float constValue) => operation switch
        {
            LogicalOperation.Less => comparingValue < constValue,
            LogicalOperation.Greater => comparingValue > constValue,
            _ => throw new System.NotImplementedException($"Logical operation {operation}")
        };
        #endregion methods
    }
    public enum FunctionType { Linear, Parabolic, Hyperbolic, Constant }
    public enum FindResult { Min, Max };
    public enum LogicalOperation { Greater, Less };
}