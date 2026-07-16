using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// A database containing instances of Scriptable Objects pertaining to the various skills available in the game.
    /// </summary>
    public class SkillsDatabase : MonoBehaviour
    {
        public static SkillsDatabase Instance { get; private set; }
        public List<Skill> skills;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else
                Instance = this;
        }

        /// <summary>
        /// Attempts to find and return a Skill with a given name.
        /// </summary>
        /// <param name="skillName">The name of the skill to look for, case-insensitive</param>
        /// <returns>Skill of one of found, null otherwise</returns>
        public Skill GetSkillByName(string skillName)
        {
            foreach (var skill in skills)
            {
                if (string.Equals(skillName, skill.skillName, StringComparison.CurrentCultureIgnoreCase))
                    return skill;
            }
            return null;
        }
    }
}