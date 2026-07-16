using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Represents a skill action that can be performed by an actor.
    /// This is the thing 'attached' to a skill button. It controls the execution of the skill alongside keeping
    /// track of the cooldown and various requirements to use said skill.
    /// </summary>
    [Serializable]
    public abstract class SkillAction
    {
        public Skill Skill { get; private set; }

        private float _cooldownEndTime;

        public float CurrentCooldown => Mathf.Max(0f, _cooldownEndTime - Time.time);
        
        public bool IsOnCooldown => CurrentCooldown > 0f;

        protected SkillAction(Skill skill)
        {
            Skill = skill;
        }

        protected void StartCooldown()
        {
            _cooldownEndTime = Time.time + Skill.cooldown;
        }

        /// <summary>
        /// Check if the action can be legally performed.
        /// </summary>
        public abstract bool CanPerform(GameObject actor, GameObject targetActor);

        /// <summary>
        /// Asynchronously executes the action, invoking onComplete when all visual animations,
        /// movement, or spell effects have fully finished.
        /// </summary>
        /// <param name="actor">The actor performing the action</param>
        /// <param name="targetActor">The target</param>
        /// <param name="onComplete">Action to perform onComplete</param>
        /// <returns></returns>
        public abstract void Execute(GameObject actor, GameObject targetActor, Action onComplete);
    }
}