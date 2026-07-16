using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat.Actions;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Stores a list of currently available actions for a given entity; i.e. player or enemy unit.
    /// Must have a default attack action.
    /// </summary>
    public class ActionsManager : MonoBehaviour
    {
        public static event Action<SkillAction> OnCombatActionAdded;
        public List<SkillAction> CombatActions { get; set; } = new();

        public void Start()
        {
            StartCoroutine(nameof(AssignDefaultActions));
        }

        /// <summary>
        /// Assigns the player with a default attack ability and a movement skill. This is a coroutine that waits for
        /// 0.6 seconds before executing to ensure everything else is in place.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AssignDefaultActions()
        {
            yield return new WaitForSeconds(0.6f);
            
            if (!gameObject.CompareTag("Player")) yield break;
            
            // if this is the player, assign a default attack action
            var attackAction = new MeleeAttackAction(SkillsDatabase.Instance.GetSkillByName("Melee Strike"));
            AddCombatAction(attackAction);
        }

        private void AddCombatAction(SkillAction skillAction)
        {
            CombatActions.Add(skillAction);
            OnCombatActionAdded?.Invoke(skillAction);
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