using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pyrite.Combat.Actions;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Stores a list of currently available actions for a given entity; i.e. player or enemy unit.
    /// Must have a default attack action.
    /// </summary>
    public class ActionsManager : MonoBehaviour
    {
        public static event Action<CombatAction> OnCombatActionAdded;
        public List<CombatAction> CombatActions { get; set; } = new();

        public void Start()
        {
            StartCoroutine(nameof(AssignDefaultAttackAction));
        }

        private IEnumerator AssignDefaultAttackAction()
        {
            if (!gameObject.CompareTag("Player")) yield break;
            
            yield return new WaitForSeconds(0.6f);
            // if this is the player, assign a default attack action
            var attackAction = new MeleeAttackAction(SkillsDatabase.Instance.GetSkillByName("Melee Strike"));
            AddCombatAction(attackAction);
        }

        public void AddCombatAction(CombatAction combatAction)
        {
            CombatActions.Add(combatAction);
            OnCombatActionAdded?.Invoke(combatAction);
        }

        public void RemoveCombatAction(string actionName)
        {
            foreach (var action in CombatActions.ToList())
                if (string.Equals(action.Skill.name, actionName, StringComparison.CurrentCultureIgnoreCase))
                {
                    CombatActions.Remove(action);
                }
        }
    }
}