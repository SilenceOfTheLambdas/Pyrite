using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Combat.Actions
{
    public class MeleeAttackAction : SkillAction
    {
        private static WaitForSeconds _waitForSeconds0_3 = new(0.3f);
        private static WaitForSeconds _waitForSeconds0_5 = new(0.5f);

        public MeleeAttackAction(Skill skill) : base(skill)
        {
        }

        public override bool CanPerform(GameObject actor, GameObject targetActor)
        {
            // Check if skill is still in cooldown
            if (Skill.isInCooldown) return false;
            
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
            Skill.isInCooldown = true;
            CurrentCooldown = Skill.cooldown;
            CurrentCooldown -= Time.deltaTime;
            if (CurrentCooldown <= 0) Skill.isInCooldown = false;
            
            // Trigger Animation
            // if (actor.TryGetComponent<Animator>(out var animator)) animator.SetTrigger("SwordSlash");
            
            
            Debug.Log("Attacking " + targetActor.name);

            onComplete?.Invoke();
        }
    }
}