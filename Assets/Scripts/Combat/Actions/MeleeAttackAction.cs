using System;
using Player;
using UnityEngine;

namespace Combat.Actions
{
    public class MeleeAttackAction : SkillAction
    {
        public MeleeAttackAction(Skill skill) : base(skill)
        {
        }

        public override bool CanPerform(GameObject actor, GameObject targetActor)
        {
            // Check if skill is still in cooldown
            if (IsOnCooldown) return false;
            
            if (targetActor == null) return false;
            
            var playerRpgController = actor.GetComponent<PlayerRpgController>();
            if (playerRpgController == null) return false;

            if (Skill.manaCost > playerRpgController.PlayerCurrentMana) return false;

            if (Vector3.Distance(actor.transform.position, targetActor.transform.position) <= Skill.range)
            {
                return true;
            }
            return false;
        }

        public override void Execute(GameObject actor, GameObject targetActor, Action onComplete)
        {
            // Initiate skill cooldown
            StartCooldown();
            
            // Trigger Animation
            // if (actor.TryGetComponent<Animator>(out var animator)) animator.SetTrigger("SwordSlash");
            
            DamageInfo.Source = actor;
            DamageInfo.Target = targetActor;
            
            targetActor.GetComponent<Combatant>().TakeDamage(DamageInfo);
            Debug.Log("Attacking enemy for: " + DamageInfo.PhysicalDamage + "");

            onComplete?.Invoke();
        }
    }
}