using System;
using System.Collections;
using Combat;
using Player;
using Pyrite.Grid;
using UnityEngine;

namespace Pyrite.Combat.Actions
{
    public class MeleeAttackAction : CombatAction
    {
        private static WaitForSeconds _waitForSeconds0_3 = new(0.3f);
        private static WaitForSeconds _waitForSeconds0_5 = new(0.5f);

        public MeleeAttackAction(Skill skill) : base(skill)
        {
        }

        public override bool CanPerform(GameObject actor, Vector2Int targetGridPos)
        {
            // Check if there is an occupant to attack
            if (!GridManager.Instance.IsCellOccupied(targetGridPos)) return false;

            // We can only perform a melee attack if both target and actor are in adjacent cells
            var actorGridPos = GridManager.Instance.WorldToGrid(actor.transform.position);

            return Vector2Int.Distance(targetGridPos, actorGridPos) <= 1;
        }

        public override IEnumerator Execute(GameObject actor, Vector2Int targetGridPos, Action onComplete)
        {
            // Rotate to face target
            var targetWorldPos = GridManager.Instance.GridToWorld(targetGridPos);
            var direction = (targetWorldPos - actor.transform.position).normalized;
            direction.y = 0;
            actor.transform.rotation = Quaternion.LookRotation(direction);

            // Trigger Animation
            if (actor.TryGetComponent<Animator>(out var animator)) animator.SetTrigger("SwordSlash");

            // Wait for visual strike moment (or yield for animation length)
            yield return _waitForSeconds0_5;

            // Apply damage logic
            var target = GridManager.Instance.GetCellOccupant(targetGridPos);
            if (target != null)
            {
                // TODO: Calculate damage to target
            }

            yield return _waitForSeconds0_3; // buffer to let swing animation finish

            onComplete?.Invoke();
        }
    }
}