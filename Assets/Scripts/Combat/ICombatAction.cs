using System;
using System.Collections;
using UnityEngine;

namespace Pyrite.Combat
{
    public interface ICombatAction
    {
        string DisplayName { get; }
        int ApCost { get; }

        /// <summary>
        /// Check if the action can be legally performed.
        /// </summary>
        public bool CanPerform(GameObject actor, Vector2Int targetGridPos);

        /// <summary>
        /// Asynchronously executes the action, invoking onComplete when all visual animations,
        /// movement, or spell effects have fully finished.
        /// </summary>
        /// <param name="actor">The actor performing the action</param>
        /// <param name="targetGridPos"></param>
        /// <param name="onComplete">Action to perform onComplete</param>
        /// <returns></returns>
        public IEnumerator Execute(GameObject actor, Vector2Int targetGridPos, Action onComplete);
    }
}