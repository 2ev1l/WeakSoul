using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Universal;

namespace Data
{
    [System.Serializable]
    public class SkillVFX: VFXData
    {
        #region fields & properties
        public FightPosition FightPosition => fightPosition;
        [SerializeField] private FightPosition fightPosition;
        public bool RandomizePosition => randomizePosition;
        [SerializeField] private bool randomizePosition;
        #endregion fields & properties

        #region methods
        public void SetPosition(Vector3 position, Vector2 additiveRandomRect = new Vector2())
        {
            base.SetPosition(position);
            if (!randomizePosition) return;
            position.x += Random.Range(-additiveRandomRect.x, additiveRandomRect.x);
            position.y += Random.Range(-additiveRandomRect.y, additiveRandomRect.y);
            base.SetPosition(position);
        }
        public new SkillVFX Clone()
		{
            SkillVFX cl = new()
            {
                fightPosition = fightPosition,
                randomizePosition = randomizePosition,
            };
            cl.ExposeValues(base.Clone());
            return cl;
        }
        #endregion methods
    }
}