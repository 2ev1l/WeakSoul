using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace Universal
{
    public class CustomAnimation : MonoBehaviour
    {
        #region fields & properties
        public static CustomAnimation Instance { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            Instance = this;
        }
        /// <summary>
        /// Move using dead zones
        /// </summary>
        /// <param name="finalPosition"></param>
        /// <param name="movingObj"></param>
        /// <param name="speed"></param>
        /// <param name="maxWaitingTime"></param>
        /// <returns></returns>
        public static IEnumerator MoveTo(Vector3 finalPosition, GameObject movingObj, float speed = 0.4f, float maxWaitingTime = Mathf.Infinity)
        {
            Transform objTransform = movingObj.transform;
            Vector3 startPosition = objTransform.position;
            Vector3 direction = finalPosition - startPosition;
            float distance = Vector3.Distance(finalPosition, startPosition);
            Vector3 finalPositionX = startPosition + direction;
            float deadZone = 0.01f;
            float waitedTime = 0f;
            float startTime = Time.realtimeSinceStartup;
            while (distance > deadZone && maxWaitingTime > waitedTime)
            {
                objTransform.position = Vector3.Lerp(startPosition, finalPositionX, speed * Time.deltaTime);
                distance = Vector3.Distance(finalPosition, startPosition);
                float deadZoneMax = Vector3.Distance(startPosition, objTransform.position) * 1.04f;
                if (deadZone < deadZoneMax)
                    deadZone = deadZoneMax;
                startPosition = objTransform.position;
                waitedTime += Time.deltaTime;
                yield return CustomMath.WaitAFrame();
            }
            movingObj.transform.position = finalPosition;
        }
        /// <summary>
        /// Move using <see cref="ValueSmoothChanger"/>
        /// </summary>
        /// <param name="finalPosition"></param>
        /// <param name="obj"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator MoveTo(Vector3 finalPosition, Transform obj, float time)
        {
            Vector3 startPosition = obj.transform.position;
            ValueSmoothChanger vscX = Instance.gameObject.AddComponent<ValueSmoothChanger>();
            ValueSmoothChanger vscY = Instance.gameObject.AddComponent<ValueSmoothChanger>();
            vscX.StartChange(startPosition.x, finalPosition.x, time);
            vscY.StartChange(startPosition.y, finalPosition.y, time);
            while (!vscX.IsChangeEnded && !vscY.IsChangeEnded)
            {
                Vector3 pos = obj.transform.position;
                pos.x = vscX.Out;
                pos.y = vscY.Out;
                obj.transform.position = pos;
                yield return CustomMath.WaitAFrame();
            }
            obj.transform.position = finalPosition;
            yield return CustomMath.WaitAFrame();
            Destroy(vscX);
            Destroy(vscY);
        }
        public static IEnumerator MoveToLocal(Vector3 finalLocalPosition, Transform currentObject, float speed = 1f)
        {
            float duration = 1f / speed;
            float lerp = Time.deltaTime;
            float squareLerp = 0f;
            float deadZone = 0f;
            float distance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
            float startDistance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
            Vector3 lastPosition = currentObject.localPosition;
            while (duration >= lerp && distance > deadZone)
            {
                float currentProgress = lerp / duration;
                if (lerp < duration / 1.67f)
                    squareLerp = currentProgress * 2.4f;
                else
                    squareLerp = currentProgress * 2.4f * Mathf.Pow((1.7f - currentProgress), 2);

                float step = startDistance * Time.deltaTime * speed;
                currentObject.localPosition += squareLerp * step * currentObject.right;
                float maxDeadZone = Vector3.Distance(currentObject.localPosition, lastPosition) * 1.04f;
                if (maxDeadZone > deadZone) deadZone = maxDeadZone;
                lerp += Time.deltaTime;
                lastPosition = currentObject.localPosition;
                distance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
                yield return CustomMath.WaitAFrame();
            }
            currentObject.localPosition = finalLocalPosition;
        }
        public static IEnumerator SetTextAlpha(Text txt, float from, float to, float sec)
        {
            Color cols = txt.color;
            cols.a = from;
            txt.color = cols;
            float lerp = 0;
            float colDistance = Mathf.Abs(from - to);
            float colPerSec = (float)colDistance / sec;
            if (from < to)
                while (txt.color.a < to)
                {
                    try
                    {
                        Color col = txt.color;
                        col.a += Time.deltaTime * colPerSec;
                        txt.color = col;
                        lerp += Time.deltaTime;
                    }
                    catch { yield break; }
                    yield return CustomMath.WaitAFrame();
                    if (lerp > sec) break;
                }
            else
                while (txt.color.a > to)
                {
                    try
                    {
                        Color col = txt.color;
                        col.a -= Time.deltaTime * colPerSec;
                        txt.color = col;
                        lerp += Time.deltaTime;
                    }
                    catch { yield break; }
                    yield return CustomMath.WaitAFrame();
                    if (lerp > sec) break;
                }
            cols = txt.color;
            cols.a = to;
            txt.color = cols;
        }
        public static void LookAt2D(Transform lookingObject, Vector3 offsetPosition, Vector3 targetPosition)
        {
			Vector3 targetDirection = targetPosition - offsetPosition;
			targetDirection.z = 0;
			lookingObject.up = targetDirection;
		}
        public static void BurstParticlesAt(Vector3 position, ParticleSystem particleSystem)
        {
            particleSystem.transform.position = position;
            Burst burst = particleSystem.emission.GetBurst(0);
            int particlesCount = Random.Range(burst.minCount, burst.maxCount);
            Vector3 startScale = particleSystem.transform.localScale;
            float optimalScale = CustomMath.GetOptimalScreenScale();
            particleSystem.transform.localScale = new Vector3(startScale.x * optimalScale, startScale.y * optimalScale, startScale.z * optimalScale);
            particleSystem.Emit(particlesCount);
            particleSystem.transform.localScale = startScale;
        }
        public static float GetNormalizedAnimatorTime(Animator animator, int layer) => Mathf.Clamp(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime, 0, 1);
        #endregion methods
    }
}