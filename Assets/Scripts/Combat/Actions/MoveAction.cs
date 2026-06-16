using System;
using System.Collections;
using Combat;
using Pyrite.Grid;
using UnityEngine;

namespace Pyrite.Combat.Actions
{
    public class MoveAction : CombatAction
    {
        public MoveAction(Skill skill) : base(skill)
        {
        }

        public override bool CanPerform(GameObject actor, Vector2Int targetGridPos)
        {
            // Check if within movement range, not blocked, and cell exists
            if (GridManager.Instance.IsCellOccupied(targetGridPos)) return false;

            // Simple manhattan distance check for AP budget
            var currentGridPos = GridManager.Instance.WorldToGrid(actor.transform.position);
            var distance = Mathf.Abs(currentGridPos.x - targetGridPos.x) +
                           Mathf.Abs(currentGridPos.y - targetGridPos.y);

            return distance > 0;
        }

        public override IEnumerator Execute(GameObject actor, Vector2Int targetGridPos, Action onComplete)
        {
            var worldTarget = GridManager.Instance.GridToWorld(targetGridPos, actor.transform.position.y);
            var moveSpeed = 5f;

            // Smoothly lerp towards cell
            while (Vector3.Distance(actor.transform.position, worldTarget) > 0.05f)
            {
                actor.transform.position =
                    Vector3.MoveTowards(actor.transform.position, worldTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            actor.transform.position = worldTarget;

            // Re-bind occupant in the grid system
            // GridManager.Instance.UpdateOccupant(currentPos, targetGridPos, actor)

            onComplete?.Invoke();
        }
    }
}