using System;
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
        public List<CombatAction> CombatActions { get; set; } = new();

        public void Start()
        {
            if (gameObject.CompareTag("Player"))
            {
                // if this is the player, assign a default attack action
                AddCombatAction(new MeleeAttackAction(SkillsDatabase.Instance.GetSkillByName("MeleeStrike")));
            }
        }

        public void AddCombatAction(CombatAction combatAction)
        {
            CombatActions.Add(combatAction);
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